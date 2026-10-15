using UnityEngine;

namespace Agame.Run.Combat
{
    [System.Serializable]
    public class ChestRarityBuildStats
    {
        public float weight = 100f;
        public int maxHP = 2;
        /// <summary>Cash earned each time an item of this rarity is found, max-level items included.</summary>
        public double itemFindCash = 100;
    }

}