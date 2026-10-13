using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Unlocks a random locked background for stars. The first unlock (the player's first background
    /// besides background 0) costs <see cref="firstUnlockCost"/>, every later one costs <see cref="unlockCost"/>.
    /// Backgrounds marked as not available in the demo are never picked in demo and playtest builds.
    /// After paying, an animation flashes random locked items of <see cref="backgroundList"/> one by one, then
    /// flashes the picked background's item a few times, and only then unlocks it.
    /// </summary>
    public class UnlockBackgroundButton : ExtendedMonoBehaviourRun
    {
        public const Currency CostCurrency = Currency.STAR;
        private const string UnlockText = "Unlock Random";
        private const string NotInDemoText = "MORE IN FULL GAME";

        [SerializeField]
        private Button button;
        [SerializeField]
        private UnifiedText costText;

        [Space]
        [SerializeField, Min(0)]
        private double firstUnlockCost = 0;
        [SerializeField, Min(0)]
        private double unlockCost = 1;

        [Header("Unlock Animation")]
        [SerializeField, Tooltip("Optional. The parent of the BackgroundListItems. Without it, backgrounds unlock without the animation.")]
        private Transform backgroundList;
        [SerializeField, Min(0), Tooltip("How many random locked items the flash stops at before the picked one.")]
        private int roamingStopCount = 10;
        [SerializeField, Min(0), Tooltip("How long, in real seconds, the flash stays at each random item.")]
        private float roamingStopDuration = 0.12f;
        [SerializeField, Min(0), Tooltip("How many times the picked item flashes before it unlocks.")]
        private int finalFlashCount = 3;
        [SerializeField, Min(0), Tooltip("How long, in real seconds, each final flash stays on, and then off.")]
        private float finalFlashDuration = 0.15f;

        private readonly List<int> lockedBackgroundIds = new List<int>();
        private readonly List<int> allLockedBackgroundIds = new List<int>();
        private readonly List<int> roamingStopIds = new List<int>();
        private RunData subscribedRunData;
        private Coroutine unlockAnimation;
        private int animatingBackgroundId = -1;
        private BackgroundListItem flashingItem;

        private bool IsAnimating => animatingBackgroundId >= 0;

        /// <summary>
        /// Whether the player can unlock a background right now: a locked background other than the one being
        /// unlocked by the animation can be picked in the current build, and the player has enough stars for it.
        /// Also works while this button is inactive.
        /// </summary>
        public bool CanUnlock
        {
            get
            {
                CollectLockedBackgroundIds();
                var unlockableCount = lockedBackgroundIds.Count;
                if (lockedBackgroundIds.Contains(animatingBackgroundId))
                {
                    unlockableCount--;
                }
                return unlockableCount > 0 && RunData.IsEnough(GetNextUnlockCost());
            }
        }

        protected override void ExtendedAwake()
        {
            base.ExtendedAwake();
            button.onClick.AddListener(Button_OnClick);
        }

        protected void OnEnable()
        {
            ///
            subscribedRunData = RunData;
            subscribedRunData.OnCurrencyValueModifiedThisFrame += RunData_OnCurrencyValueModifiedThisFrame;
            subscribedRunData.OnBackgroundUnlocked += RunData_OnBackgroundUnlocked;

            ///
            UpdateView();
        }

        protected void OnDisable()
        {
            ///
            var runData = subscribedRunData;

            // RunData outlives this scene, so drop the handlers even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnCurrencyValueModifiedThisFrame -= RunData_OnCurrencyValueModifiedThisFrame;
                subscribedRunData.OnBackgroundUnlocked -= RunData_OnBackgroundUnlocked;
                subscribedRunData = null;
            }

            // The animation stops with this, and the stars are already spent, so the background unlocks right away.
            if (IsAnimating)
            {
                FinishUnlock(runData);
            }
        }

        private void RunData_OnCurrencyValueModifiedThisFrame(Currency currency)
        {
            if (currency == CostCurrency)
            {
                UpdateView();
            }
        }

        private void RunData_OnBackgroundUnlocked(int backgroundId)
        {
            UpdateView();
        }

        private CurrencyAmount GetNextUnlockCost()
        {
            // Background 0 is always unlocked, so a count of 1 means no background has been bought yet.
            // The background being unlocked by the animation is already paid for, so it counts as unlocked.
            var unlockedCount = RunData.UnlockedBackgroundCount + (IsAnimating ? 1 : 0);
            var amount = unlockedCount <= 1 ? firstUnlockCost : unlockCost;
            return new CurrencyAmount(CostCurrency, amount);
        }

        /// <summary>
        /// Collects the locked backgrounds the player can unlock in the current build into
        /// <see cref="lockedBackgroundIds"/>, and every locked background, including those not available in the
        /// current build, into <see cref="allLockedBackgroundIds"/>. Returns whether any background is locked at all.
        /// </summary>
        private bool CollectLockedBackgroundIds()
        {
            lockedBackgroundIds.Clear();
            allLockedBackgroundIds.Clear();
            var anyLocked = false;
            var backgroundManager = RunEntry.combatBackgroundManager;
            for (int id = 1; id < GameConst.BackgroundCount; id++)
            {
                if (RunData.IsBackgroundUnlocked(id))
                {
                    continue;
                }

                ///
                anyLocked = true;
                allLockedBackgroundIds.Add(id);
                if (backgroundManager.IsBackgroundAvailableInCurrentBuild(id))
                {
                    lockedBackgroundIds.Add(id);
                }
            }
            return anyLocked;
        }

        private void UpdateView()
        {
            ///
            var anyLocked = CollectLockedBackgroundIds();
            if (lockedBackgroundIds.Count == 0)
            {
                costText.SetText(anyLocked ? NotInDemoText : "ALL UNLOCKED");
                button.interactable = false;
                return;
            }

            ///
            var cost = GetNextUnlockCost();
            costText.SetText($"{UnlockText} - {cost.GetFormattedString()}");
            button.interactable = !IsAnimating && RunData.IsEnough(cost);
        }

        private void Button_OnClick()
        {
            ///
            if (IsAnimating)
            {
                return;
            }

            ///
            CollectLockedBackgroundIds();
            if (lockedBackgroundIds.Count == 0)
            {
                return;
            }

            ///
            if (!RunData.SpendCurrency(GetNextUnlockCost()))
            {
                return;
            }

            ///
            var backgroundId = lockedBackgroundIds[Random.Range(0, lockedBackgroundIds.Count)];
            if (backgroundList == null)
            {
                RunData.UnlockBackground(backgroundId);
                return;
            }

            ///
            animatingBackgroundId = backgroundId;
            button.interactable = false;
            CollectRoamingStopIds(backgroundId);
            unlockAnimation = StartCoroutine(UnlockAnimation());
        }

        /// <summary>
        /// Picks the random locked items the flash stops at before reaching <paramref name="backgroundId"/>, among
        /// every locked background, including those that can't be picked in the current build. No item gets 2 stops
        /// in a row, and the last stop isn't the picked background, so every step visibly moves the flash. With only
        /// the picked background locked, there are no stops.
        /// </summary>
        private void CollectRoamingStopIds(int backgroundId)
        {
            ///
            roamingStopIds.Clear();
            if (allLockedBackgroundIds.Count < 2)
            {
                return;
            }

            // Picked backwards from the picked background, each stop differing from the one after it.
            var nextId = backgroundId;
            for (int i = 0; i < roamingStopCount; i++)
            {
                var excludedIndex = allLockedBackgroundIds.IndexOf(nextId);
                var index = Random.Range(0, allLockedBackgroundIds.Count - 1);
                if (index >= excludedIndex)
                {
                    index++;
                }
                nextId = allLockedBackgroundIds[index];
                roamingStopIds.Add(nextId);
            }
            roamingStopIds.Reverse();
        }

        private IEnumerator UnlockAnimation()
        {
            ///
            foreach (var id in roamingStopIds)
            {
                SetFlashingItem(id);
                yield return new WaitForSecondsRealtime(roamingStopDuration);
            }
            SetFlashingItem(-1);

            ///
            for (int i = 0; i < finalFlashCount; i++)
            {
                SetFlashingItem(animatingBackgroundId);
                yield return new WaitForSecondsRealtime(finalFlashDuration);
                SetFlashingItem(-1);
                yield return new WaitForSecondsRealtime(finalFlashDuration);
            }

            ///
            unlockAnimation = null;
            FinishUnlock(RunData);
        }

        /// <summary>
        /// Shows the Flash Background of the list item of <paramref name="backgroundId"/> only, or of no item for -1.
        /// </summary>
        private void SetFlashingItem(int backgroundId)
        {
            ///
            if (flashingItem != null)
            {
                flashingItem.SetFlashing(false);
                flashingItem = null;
            }

            // A list item's background id is its sibling index.
            if (backgroundId >= 0 && backgroundId < backgroundList.childCount)
            {
                flashingItem = backgroundList.GetChild(backgroundId).GetComponent<BackgroundListItem>();
                if (flashingItem != null)
                {
                    flashingItem.SetFlashing(true);
                }
            }
        }

        private void FinishUnlock(RunData runData)
        {
            ///
            if (unlockAnimation != null)
            {
                StopCoroutine(unlockAnimation);
                unlockAnimation = null;
            }
            SetFlashingItem(-1);

            // Unlocking refreshes the view through OnBackgroundUnlocked.
            var backgroundId = animatingBackgroundId;
            animatingBackgroundId = -1;
            runData.UnlockBackground(backgroundId);
        }
    }

}
