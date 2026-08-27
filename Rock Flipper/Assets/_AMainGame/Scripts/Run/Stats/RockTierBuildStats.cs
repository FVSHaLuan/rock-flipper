using UnityEngine;

namespace Agame.Run.Stats
{
    [System.Serializable]
    public class RockTierBuildStats
    {
        public int count;
        public int maxCount = 10;
        public float purity = 0.1f;
        public double landingCash = 1;
        public double breakingCash = 5;
        public float purityCashMultiplier = 3;
        /// <summary>
        /// Cooldown time before rock can flip again after landed
        /// </summary>
        public float landingCooldown = 2;
    }

}