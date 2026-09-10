using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Stats
{
    [System.Serializable]
    public class RockTierBuildStats
    {
        private List<int> maxCountThresholds;

        public bool unlocked = false;
        public int count;
        public int maxCount = 10;
        public float purity = 0.1f;
        public double landingCash = 1;
        public double breakingCash = 5;
        public float purityCashMultiplier = 3;
        /// <summary>
        /// Cooldown time before rock can flip again after landed
        /// </summary>
        public float landingCooldown = 0.5f;
        public double chestLevelExp = 1;
        public double levelExp = 1;

        public void AddNewMaxCountThreshold()
        {
            if (maxCountThresholds == null)
            {
                maxCountThresholds = new List<int>();
            }
            maxCountThresholds.Add(maxCount);
        }


        public int GetCountPriceLevel(int countLevel)
        {
            if (maxCountThresholds == null)
            {
                return countLevel;
            }

            ///
            int priceLevel = countLevel;
            for (int i = 1; i < maxCountThresholds.Count; i++)
            {
                var previousMaxCountThreshold = maxCountThresholds[i - 1];
                if (countLevel >= previousMaxCountThreshold)
                {
                    priceLevel = count - previousMaxCountThreshold;
                }
                else
                {
                    break;
                }
            }

            ///
            return priceLevel;
        }
    }

}