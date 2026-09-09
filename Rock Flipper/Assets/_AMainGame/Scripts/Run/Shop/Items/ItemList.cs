using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemList : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private List<ItemView> itemViews;

        protected void Start()
        {
            ViewItems();
        }

        private void ViewItems()
        {
            ///
            var itemDataManager = RunEntry.itemDataManager;

            // Initialize the item views with the corresponding item data
            for (int i = 0; i < itemViews.Count; i++)
            {
                var itemView = itemViews[i];

                ///
                if (i >= itemDataManager.AllItemCount)
                {
                    itemView.gameObject.SetActive(false);
                    continue;
                }

                ///
                itemView.gameObject.SetActive(true);
                var itemData = itemDataManager.GetItemByIndex(i);
                itemView.SetItemData(itemData);
            }
        }
    }

}