using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    /// <summary>
    /// Items the player hasn't seen since they were found or leveled up (<see cref="Run.Shop.ItemState.unseen"/>).
    /// </summary>
    public partial class RunData
    {
        public bool HasUnseenItems
        {
            get
            {
                ///
                if (itemStates == null)
                {
                    return false;
                }

                ///
                foreach (var itemState in itemStates.Values)
                {
                    if (itemState.unseen)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void MarkItemSeen(string itemId)
        {
            ///
            var itemState = GetItemState(itemId);
            if (!itemState.unseen)
            {
                return;
            }

            ///
            itemState.unseen = false;
            SetItemState(itemId, itemState);
        }

        public void MarkAllItemsSeen()
        {
            ///
            if (itemStates == null)
            {
                return;
            }

            // Collected first: SetItemState writes to the dictionary being enumerated.
            var unseenItemIds = new List<string>();
            foreach (var pair in itemStates)
            {
                if (pair.Value.unseen)
                {
                    unseenItemIds.Add(pair.Key);
                }
            }

            ///
            foreach (var itemId in unseenItemIds)
            {
                MarkItemSeen(itemId);
            }
        }
    }

}
