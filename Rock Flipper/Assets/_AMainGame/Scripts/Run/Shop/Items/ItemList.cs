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
            var itemManager = RunEntry.itemManager;

            // Initialize the item views with the corresponding item prefabs
            for (int i = 0; i < itemViews.Count; i++)
            {
                var itemView = itemViews[i];

                ///
                if (i >= itemManager.AllItemCount)
                {
                    itemView.gameObject.SetActive(false);
                    continue;
                }

                ///
                itemView.gameObject.SetActive(true);
                var item = itemManager.GetItemByIndex(i);
                itemView.SetItem(item);
            }
        }
    }

}