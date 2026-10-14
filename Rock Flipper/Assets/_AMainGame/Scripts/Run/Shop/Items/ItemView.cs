using Agame.Run.Combat;
using Agame.UI.ToolTips;
using GD;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    [RequireComponent(typeof(ToolTipTriggerer))]
    public class ItemView : ExtendedMonoBehaviourRun
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

        private Item item;
        private ToolTipTriggerer toolTipTriggerer;
        private RunData subscribedRunData;

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
        }

        protected void OnDestroy()
        {
            // RunData outlives this scene, so drop the handler even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnItemStateChanged -= RunData_OnItemStateChanged;
                subscribedRunData = null;
            }
        }

        private void RunData_OnItemStateChanged(ItemState obj)
        {
            if (item != null && obj.itemId == item.ItemId)
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

            ///
            ViewItemState();
        }

        public void ViewItemState()
        {
            var itemState = RunData.GetItemState(item.ItemId);
            var maxLevel = gameBalance.GetItemMaxLevel(item.Rarity);

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
                var expRequired = gameBalance.GetRequiredExpForNextItemLevel(item.Rarity, itemState.level);
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
            content += itemState.level >= maxLevel ? $"Lv. {itemState.level} (maxed)\n" : $"Lv. {itemState.level}\n";

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