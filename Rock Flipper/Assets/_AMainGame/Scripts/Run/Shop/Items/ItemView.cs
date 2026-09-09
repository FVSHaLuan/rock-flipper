using Agame.Run.Combat;
using GD;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
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

        private ItemData itemData;

        protected void Start()
        {
            RunData.OnItemStateChanged += RunData_OnItemStateChanged;
        }

        private void RunData_OnItemStateChanged(ItemState obj)
        {
            if (itemData != null && obj.itemId == itemData.ItemId)
            {
                ViewItemState();
            }
        }

        public void SetItemData(ItemData itemData)
        {
            ///
            this.itemData = itemData;

            ///
            itemIconImage.sprite = itemData.ItemIcon;
            itemIconImage.color = itemData.Rarity.GetForegroundColor();

            ///
            ViewItemState();
        }

        public void ViewItemState()
        {
            var itemState = RunData.GetItemState(itemData.ItemId);
            var maxLevel = gameBalance.GetItemMaxLevel(itemData.Rarity);

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
                var expRequired = gameBalance.GetRequiredExpForNextItemLevel(itemData.Rarity, itemState.level);
                progressBar.SetValue((float)itemState.exp / expRequired);
            }
        }
    }

}