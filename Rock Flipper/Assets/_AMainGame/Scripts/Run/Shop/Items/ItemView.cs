using Agame.Balancing;
using Agame.Run.Combat;
using Agame.UI.ToolTips;
using GD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    [RequireComponent(typeof(ToolTipTriggerer))]
    public class ItemView : ExtendedMonoBehaviourRun, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private Image itemIconImage;
        [SerializeField]
        private GameObject lockedItemIcon;
        [SerializeField]
        private GameObject progressBarWrapper;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private GameObject maxedText;
        [SerializeField]
        private UnifiedText levelText;
        [SerializeField, Tooltip("Shown while the item is unseen (found or leveled up since the player last saw it).")]
        private GameObject noticeBadge;
        [SerializeField, Tooltip("Shown in demo and playtest builds when the item can't be found there (placeholder or not available in the demo).")]
        private GameObject naInDemoObject;
        [SerializeField, Min(0f), Tooltip("Real-time seconds the view must stay hovered or selected to mark an unseen item seen.")]
        private float markSeenHoverDuration = 2f;

        private Item item;
        private ToolTipTriggerer toolTipTriggerer;
        private RunData subscribedRunData;
        private FoundItemAnimation subscribedFoundItemAnimation;
        private bool isPointerOver;
        private bool isSelected;
        private float hoverTime;

        private ToolTipTriggerer ToolTipTriggerer
        {
            get
            {
                if (toolTipTriggerer == null)
                {
                    toolTipTriggerer = GetComponent<ToolTipTriggerer>();
                }
                return toolTipTriggerer;
            }
        }

        protected void Start()
        {
            subscribedRunData = RunData;
            subscribedRunData.OnItemStateChanged += RunData_OnItemStateChanged;
            subscribedFoundItemAnimation = RunEntry.foundItemAnimation;
            subscribedFoundItemAnimation.OnItemRevealed += FoundItemAnimation_OnItemRevealed;
        }

        protected void OnDestroy()
        {
            // RunData outlives this scene, so drop the handler even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnItemStateChanged -= RunData_OnItemStateChanged;
                subscribedRunData = null;
            }
            if (subscribedFoundItemAnimation != null)
            {
                subscribedFoundItemAnimation.OnItemRevealed -= FoundItemAnimation_OnItemRevealed;
                subscribedFoundItemAnimation = null;
            }
        }

        protected void OnDisable()
        {
            isPointerOver = false;
            isSelected = false;
            hoverTime = 0f;
        }

        protected void Update()
        {
            ///
            if (item == null
                || !(isPointerOver || isSelected)
                || !RunEntry.foundItemAnimation.GetRevealedItemState(item.ItemId).unseen)
            {
                hoverTime = 0f;
                return;
            }

            ///
            hoverTime += Time.unscaledDeltaTime;
            if (hoverTime >= markSeenHoverDuration)
            {
                hoverTime = 0f;
                RunData.MarkItemSeen(item.ItemId);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            isPointerOver = true;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            isPointerOver = false;
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            isSelected = true;
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            isSelected = false;
        }

        private void RunData_OnItemStateChanged(ItemState obj)
        {
            if (item != null && obj.itemId == item.ItemId)
            {
                ViewItemState();
            }
        }

        private void FoundItemAnimation_OnItemRevealed(string itemId)
        {
            if (item != null && itemId == item.ItemId)
            {
                ViewItemState();
            }
        }

        public void SetItem(Item item)
        {
            ///
            this.item = item;

            ///
            itemIconImage.sprite = item.ItemIcon;
            itemIconImage.color = item.Rarity.GetForegroundColor();
            naInDemoObject.SetActive(!item.IsDroppableInCurrentBuild);

            ///
            ViewItemState();
        }

        public void ViewItemState()
        {
            // Not RunData's state: a found item shows here only once it lands on the Items tab button.
            var itemState = RunEntry.foundItemAnimation.GetRevealedItemState(item.ItemId);
            var maxLevel = GameBalance.GetItemMaxLevel(item.Rarity);

            ///
            if (itemState.level >= maxLevel)
            {
                progressBarWrapper.SetActive(false);
                maxedText.SetActive(true);
                lockedItemIcon.SetActive(false);
                itemIconImage.gameObject.SetActive(true);
            }
            else if (itemState.level == 0 && itemState.exp == 0)
            {
                progressBarWrapper.SetActive(false);
                maxedText.SetActive(false);
                lockedItemIcon.SetActive(true);
                itemIconImage.gameObject.SetActive(false);
            }
            else
            {
                progressBarWrapper.SetActive(true);
                maxedText.SetActive(false);
                lockedItemIcon.SetActive(false);
                itemIconImage.gameObject.SetActive(true);
                levelText.SetText($"Lv. {itemState.level}");
                var expRequired = GameBalance.GetRequiredExpForNextItemLevel(item.Rarity, itemState.level);
                progressBar.SetValue((float)itemState.exp / expRequired);
            }

            ///
            noticeBadge.SetActive(itemState.unseen);

            ///
            UpdateToolTipContent(itemState, maxLevel);
        }

        private void UpdateToolTipContent(ItemState itemState, int maxLevel)
        {
            ///
            if (itemState.level == 0 && itemState.exp == 0)
            {
                ToolTipTriggerer.MainText = "<b>???</b>\nNot found yet.";
                return;
            }

            ///
            var rarityColor = item.Rarity.GetForegroundColor();
            var content = $"<b>{rarityColor.WrapText(item.ItemName)}</b>\n";
            content += $"{rarityColor.WrapText(item.Rarity)}\n";
            if (itemState.level >= maxLevel)
            {
                content += $"Lv. {itemState.level} (maxed)\n";
            }
            else
            {
                var expRequired = GameBalance.GetRequiredExpForNextItemLevel(item.Rarity, itemState.level);
                content += $"Lv. {itemState.level}\nExp: {itemState.exp}/{expRequired}\n";
            }
            var cashName = entry.currencyConfigManager.GetConfig(Currency.CASH).CurrencyName;
            content += $"Cash earned: {cashName}{itemState.earnedCash.ToLargeNumberString()}\n";

            ///
            var skillDescriptor = item.BuildAgent != null ? item.BuildAgent.GetComponent<SkillDescriptor>() : null;
            if (skillDescriptor != null)
            {
                content += $"\n{skillDescriptor.GetDescription(item.BuildValue)}";
                var extraDescription = skillDescriptor.GetExtraDescription();
                if (!string.IsNullOrWhiteSpace(extraDescription))
                {
                    content += $"\n{extraDescription}";
                }
            }

            ///
            content += "\n\n<i>Effects scale with the item's level.</i>";

            ///
            ToolTipTriggerer.MainText = content;
        }
    }

}