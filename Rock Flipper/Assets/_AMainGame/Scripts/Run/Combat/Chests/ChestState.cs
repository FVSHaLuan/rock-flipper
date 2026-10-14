using UnityEngine;

namespace Agame.Run.Combat
{
    [System.Serializable]
    public struct ChestState
    {
        public ChestRarity rarity;
        /// <summary>
        /// Unique chests only: the item it drops, picked when the chest dropped (<see cref="UniqueChestDropRules"/>).
        /// Empty for other rarities and for Unique chests spawned by dev tools, which drop a random Unique item.
        /// </summary>
        public string uniqueItemId;
        /// <summary>
        /// Unique chests only: the background that dropped it. Only meaningful when <see cref="HasUniqueItem"/>.
        /// </summary>
        public int uniqueBackgroundId;

        public bool HasUniqueItem => !string.IsNullOrEmpty(uniqueItemId);
    }
}
