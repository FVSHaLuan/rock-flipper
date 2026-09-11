using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Stats
{
    [System.Serializable]
    public class RockTierBuildStats
    {
        /// <summary>
        /// History of previous <see cref="maxCount"/> values, snapshotted (via <see cref="RecordMaxCountCapBreakpoint"/>)
        /// right before each cap increase. Used by <see cref="GetPriceLevelSinceLastCapIncrease"/> to reset the
        /// shop price-scaling level after every cap increase, instead of letting it compound forever.
        /// </summary>
        private List<int> maxCountCapBreakpoints;

        public bool unlocked = false;
        public int count;
        public int maxCount = 10;
        public float purity = 0.1f;
        public double landingCash = 1;
        public double breakingCashMultiplier = 2;
        public float purityCashMultiplier = 3;
        /// <summary>
        /// Cooldown time before rock can flip again after landed
        /// </summary>
        public float landingCooldown = 0.5f;
        public double chestLevelExp = 1;
        public double levelExp = 1;

        /// <summary>
        /// Snapshots the current <see cref="maxCount"/> as a breakpoint. Call this right before raising
        /// <see cref="maxCount"/> (e.g. from a skill/shop upgrade) so <see cref="GetPriceLevelSinceLastCapIncrease"/>
        /// can later reset the price-scaling level relative to this cap.
        /// </summary>
        public void RecordMaxCountCapBreakpoint()
        {
            if (maxCountCapBreakpoints == null)
            {
                maxCountCapBreakpoints = new List<int>();
            }
            maxCountCapBreakpoints.Add(maxCount);
        }


        /// <summary>
        /// Converts an absolute owned-count into the level to feed into the shop's price-scaling formula,
        /// resetting the count relative to the most recent cap breakpoint the owned count has passed.
        /// This keeps prices from compounding across every past cap increase.
        /// </summary>
        /// <param name="ownedCount">The tier's current total owned rock count.</param>
        public int GetPriceLevelSinceLastCapIncrease(int ownedCount, out int priceSet)
        {
            ///
            priceSet = 0;

            ///
            if (maxCountCapBreakpoints == null)
            {
                priceSet = 0;
                return ownedCount;
            }

            ///
            int priceLevel = ownedCount;
            for (int i = 1; i < maxCountCapBreakpoints.Count; i++)
            {
                var previousCapBreakpoint = maxCountCapBreakpoints[i - 1];
                if (ownedCount >= previousCapBreakpoint)
                {
                    priceLevel = ownedCount - previousCapBreakpoint;
                    priceSet++;
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