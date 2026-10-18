using System.Collections;
using System.Collections.Generic;
using FH.Core.Architecture.Pool;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows a found item (e.g. dropped by an opening chest) with an <see cref="ItemAvatar"/>: it pops up from where it
    /// was found, hovers for a moment, then flies to the Items tab button, shrinking as its name fades out.
    /// On arrival it plays the item arrival sound and the target's <see cref="NoticeEffect"/>, if it has one among its children.
    /// <para>
    /// A find goes through <see cref="FindItem"/>: it's saved in <see cref="RunData"/> right away, so quitting before the
    /// item arrives loses nothing, but the items UI (item grid, notice badges) keeps showing the item as it was before the
    /// find until the item reaches the target. That UI reads <see cref="GetRevealedItemState"/> and
    /// <see cref="HasRevealedUnseenItems"/> instead of <see cref="RunData"/>, and refreshes on <see cref="OnItemRevealed"/>.
    /// </para>
    /// </summary>
    public class FoundItemAnimation : ExtendedMonoBehaviourRun
    {
        /// <summary>
        /// An item find already saved in <see cref="RunData"/> but not shown in the items UI yet. See <see cref="FindItem"/>.
        /// </summary>
        public class UnrevealedFind
        {
            public Item Item { get; }
            public ItemState StateBeforeFind { get; }
            /// <summary>The find marked the item unseen (found or leveled it up).</summary>
            public bool MarkedUnseen { get; internal set; }

            public UnrevealedFind(Item item, ItemState stateBeforeFind)
            {
                Item = item;
                StateBeforeFind = stateBeforeFind;
            }
        }

        /// <summary>
        /// Called with the item id when a find reaches the target and the items UI may show it.
        /// </summary>
        public event System.Action<string> OnItemRevealed;

        [SerializeField]
        private ItemAvatar avatarPrototype;
        [SerializeField, Tooltip("Shows the cash earned by the find as the item appears.")]
        private GeneralPoolMemberSimplifiedEffect cashFloatingTextPrototype;
        [SerializeField, Tooltip("Where the cash floating text appears, relative to where the item appears.")]
        private Vector2 cashFloatingTextOffset;
        [SerializeField, Tooltip("Where the item flies to: the Items tab button. A UI element in any canvas render mode, or a world-space transform.")]
        private Transform target;

        [Header("Appear")]
        [SerializeField, Min(0)]
        private float appearDuration = 0.35f;
        [SerializeField, Tooltip("How far up the item rises from where it was found while appearing.")]
        private float appearRise = 0.6f;
        [SerializeField, Tooltip("The item's scale over the appearing (0 to 1), relative to the avatar prefab's scale.")]
        private AnimationCurve appearScale = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.7f, 1.2f),
            new Keyframe(1f, 1f));

        [Header("Hover")]
        [SerializeField, Min(0)]
        private float hoverDuration = 0.6f;

        [Header("Flight")]
        [SerializeField, Min(0)]
        private float flightDuration = 0.55f;
        [SerializeField, Tooltip("Flight progress (0 to 1) over the flight time (0 to 1). Starts slow by default, so the item speeds up toward the button.")]
        private AnimationCurve flightProgress = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(1f, 1f, 2f, 2f));
        [SerializeField, Tooltip("How far the flight path bulges upward at its middle.")]
        private float flightArcHeight = 1f;
        [SerializeField, Tooltip("The item's scale on arrival, relative to the avatar prefab's scale.")]
        private float flightEndScale = 0.3f;
        [SerializeField, Min(0), Tooltip("Seconds the name takes to fade out once the flight starts.")]
        private float nameFadeDuration = 0.15f;

        [Space]
        [SerializeField]
        private UnityEvent onAppeared;
        [SerializeField, Tooltip("Called each time an item reaches the target, e.g. to bump the Items tab button.")]
        private UnityEvent onArrived;

        private Canvas targetCanvas;
        private NoticeEffect targetNoticeEffect;
        private readonly List<ItemAvatar> activeAvatars = new List<ItemAvatar>();
        // oldest first
        private readonly List<UnrevealedFind> unrevealedFinds = new List<UnrevealedFind>();

        protected void Start()
        {
            targetCanvas = target.GetComponentInParent<Canvas>(true);
            if (targetCanvas != null)
            {
                targetCanvas = targetCanvas.rootCanvas;
            }
            targetNoticeEffect = target.GetComponentInChildren<NoticeEffect>(true);
        }

        protected void OnDisable()
        {
            // the animation coroutines die with this component, so their avatars have to go back to the pool here
            foreach (var avatar in activeAvatars)
                avatar.TryReturnToPoolAndDeactivate();
            activeAvatars.Clear();

            // Their flights can't arrive anymore. Not revealed one by one: this only happens as the scene unloads,
            // when RunEntry may already be gone. The finds are saved in RunData either way.
            unrevealedFinds.Clear();
        }

        /// <summary>
        /// Adds <paramref name="exp"/> to <paramref name="item"/> in <see cref="RunData"/> right away, but holds the find
        /// back from the items UI until <see cref="Play"/>'s flight reaches the target.
        /// Pass the returned find to <see cref="Play"/>.
        /// </summary>
        public UnrevealedFind FindItem(Item item, int exp)
        {
            // Held back before the exp is added, so the UI refreshing on RunData's change already sees it held back.
            var find = new UnrevealedFind(item, RunData.GetItemState(item.ItemId));
            unrevealedFinds.Add(find);
            find.MarkedUnseen = RunData.AddItemExp(item.ItemId, exp);
            return find;
        }

        /// <summary>
        /// Plays the animation for <paramref name="find"/> (from <see cref="FindItem"/>), found at <paramref name="origin"/>
        /// (world position), and reveals it in the items UI on arrival. Several can play at once.
        /// </summary>
        public void Play(UnrevealedFind find, Vector3 origin)
        {
            ///
            var avatar = CurrentGeneralPool.TakeInstance(avatarPrototype, this);
            activeAvatars.Add(avatar);
            avatar.SetItem(find.Item);
            avatar.transform.position = origin;
            avatar.transform.localScale = Vector3.zero;
            avatar.gameObject.SetActive(true);

            ///
            StartCoroutine(Animate(avatar, origin, find));
        }

        /// <summary>
        /// <paramref name="itemId"/>'s state as the items UI shows it: its <see cref="RunData"/> state, minus the finds
        /// that haven't reached the target yet.
        /// </summary>
        public ItemState GetRevealedItemState(string itemId)
        {
            ///
            var itemState = RunData.GetItemState(itemId);

            // The oldest unrevealed find of this item holds the state the player last saw.
            foreach (var find in unrevealedFinds)
            {
                if (find.Item.ItemId != itemId)
                {
                    continue;
                }

                ///
                itemState.level = find.StateBeforeFind.level;
                itemState.exp = find.StateBeforeFind.exp;
                // Still unseen only if it was before the find and the player hasn't seen it since.
                itemState.unseen &= find.StateBeforeFind.unseen;
                break;
            }
            return itemState;
        }

        /// <summary>
        /// Whether the items UI shows at least one item as unseen (<see cref="GetRevealedItemState"/>).
        /// </summary>
        public bool HasRevealedUnseenItems
        {
            get
            {
                ///
                if (!RunData.HasUnseenItems)
                {
                    return false;
                }
                if (unrevealedFinds.Count == 0)
                {
                    return true;
                }

                ///
                var itemManager = RunEntry.itemManager;
                for (int i = 0; i < itemManager.AllItemCount; i++)
                {
                    if (GetRevealedItemState(itemManager.GetItemByIndex(i).ItemId).unseen)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void Reveal(UnrevealedFind find)
        {
            ///
            if (!unrevealedFinds.Remove(find))
            {
                return;
            }

            // The player may have marked the item seen while the find was held back (e.g. with "mark all seen").
            if (find.MarkedUnseen)
            {
                RunData.MarkItemUnseen(find.Item.ItemId);
            }

            ///
            OnItemRevealed?.Invoke(find.Item.ItemId);
        }

        private IEnumerator Animate(ItemAvatar avatar, Vector3 origin, UnrevealedFind find)
        {
            var avatarTransform = avatar.transform;
            var normalScale = avatarPrototype.transform.localScale;

            // Appear
            var appearEnd = origin + Vector3.up * appearRise;
            EarnFindCash(find.Item, appearEnd);
            onAppeared?.Invoke();
            for (var elapsed = 0f; elapsed < appearDuration; elapsed += Time.deltaTime)
            {
                var progress = elapsed / appearDuration;
                avatarTransform.position = Vector3.Lerp(origin, appearEnd, 1f - (1f - progress) * (1f - progress));
                avatarTransform.localScale = normalScale * appearScale.Evaluate(progress);
                yield return null;
            }
            avatarTransform.position = appearEnd;
            avatarTransform.localScale = normalScale * appearScale.Evaluate(1f);

            // Hover
            yield return new WaitForSeconds(hoverDuration);

            // Flight
            var flightStart = avatarTransform.position;
            var hoverScale = avatarTransform.localScale;
            for (var elapsed = 0f; elapsed < flightDuration; elapsed += Time.deltaTime)
            {
                var progress = flightProgress.Evaluate(elapsed / flightDuration);
                var flightEnd = GetTargetPosition(flightStart.z);
                avatarTransform.position = Vector3.LerpUnclamped(flightStart, flightEnd, progress)
                    + Vector3.up * (Mathf.Sin(progress * Mathf.PI) * flightArcHeight);
                avatarTransform.localScale = Vector3.LerpUnclamped(hoverScale, normalScale * flightEndScale, progress);
                avatar.SetNameAlpha(nameFadeDuration > 0f ? 1f - Mathf.Clamp01(elapsed / nameFadeDuration) : 0f);
                yield return null;
            }

            // Arrival
            activeAvatars.Remove(avatar);
            avatar.TryReturnToPoolAndDeactivate();
            Reveal(find);
            RunEntry.combatSoundEffectManager.PlayItemArrival();
            if (targetNoticeEffect != null)
            {
                targetNoticeEffect.Play();
            }
            onArrived?.Invoke();
        }

        /// <summary>
        /// Earns the cash for finding <paramref name="item"/> (per its rarity, whatever its item level) and shows it as a
        /// floating text near <paramref name="position"/>. Earned as the item appears, so a find whose item never
        /// appears (quitting mid-animation) earns no cash.
        /// </summary>
        private void EarnFindCash(Item item, Vector3 position)
        {
            ///
            var amount = BuildStats.GetChestRarityBuildStats(item.Rarity).itemFindCash;
            if (amount <= 0)
            {
                return;
            }
            RunData.AddCurrency(Currency.CASH, amount);
            RunData.AddItemEarnedCash(item.ItemId, amount);

            ///
            if (cashFloatingTextPrototype == null)
            {
                return;
            }
            var text = entry.currencyConfigManager.GetConfig(Currency.CASH).CurrencyName + amount.ToLargeNumberString();
            RunEntry.floatingTextManager.Spawn(cashFloatingTextPrototype, (Vector2)position + cashFloatingTextOffset, text);
        }

        /// <summary>
        /// The target's position seen by the gameplay camera, at depth <paramref name="z"/>.
        /// Read every frame, since the target can move (layout changes, screen resizes).
        /// </summary>
        private Vector3 GetTargetPosition(float z)
        {
            var targetPosition = target.position;
            if (targetCanvas != null)
            {
                var canvasCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(canvasCamera, targetPosition);
                var gameplayCamera = RunEntry.gameplayCamera;
                screenPoint.z = z - gameplayCamera.transform.position.z;
                targetPosition = gameplayCamera.ScreenToWorldPoint(screenPoint);
            }
            targetPosition.z = z;
            return targetPosition;
        }
    }

}
