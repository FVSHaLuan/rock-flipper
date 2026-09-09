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
        private GameObject progressBarWrapper;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private GameObject maxedText;

        private ItemData itemData;

        public void SetItemData(ItemData itemData)
        {
            ///
            this.itemData = itemData;

            ///
            itemIconImage.sprite = itemData.ItemIcon;
        }
    }

}