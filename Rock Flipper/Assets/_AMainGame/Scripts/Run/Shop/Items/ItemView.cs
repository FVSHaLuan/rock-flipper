using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemView : ExtendedMonoBehaviourRun
    {
        private ItemData itemData;

        public void SetItemData(ItemData itemData)
        {
            this.itemData = itemData;
        }
    }

}