using UnityEngine;

namespace Agame.Run.Shop
{
    [System.Serializable]
    public struct ItemState
    {
        public string itemId;
        public int level;
        public int exp;
        /// <summary>
        /// The item was found or leveled up since the player last saw it. Drives the item notice badges
        /// (<see cref="ItemView"/>, <see cref="ItemsTabButtonNoticeBadge"/>).
        /// </summary>
        public bool unseen;
        /// <summary>
        /// Total cash finding this item has earned (<see cref="Combat.ChestRarityBuildStats.itemFindCash"/> per find).
        /// </summary>
        public double earnedCash;

        public bool IsFound => level > 0 || exp > 0;
    }

}
