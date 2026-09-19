using System.Collections.Generic;

namespace Agame.Run.Stats
{
    /// <summary>
    /// Tracks a history of previous "max count" cap values for a stat that can have its cap raised
    /// (e.g. a rock tier's max owned count, the flipper bot max count). Used to reset a shop's
    /// price-scaling level after every cap increase, instead of letting the price compound forever
    /// across every past cap increase.
    /// </summary>
    [System.Serializable]
    public class MaxCountCapBreakpoints
    {
        private List<int> breakpoints;

        /// <summary>
        /// Snapshots the current max count as a breakpoint. Call this right before raising the max
        /// count (e.g. from a skill/shop upgrade) so <see cref="GetPriceLevelSinceLastCapIncrease"/>
        /// can later reset the price-scaling level relative to this cap.
        /// </summary>
        public void RecordBreakpoint(int currentMaxCount)
        {
            if (breakpoints == null)
            {
                breakpoints = new List<int>();
            }
            breakpoints.Add(currentMaxCount);
        }

        /// <summary>
        /// Converts an absolute owned-count into the level to feed into the shop's price-scaling formula,
        /// resetting the count relative to the most recent cap breakpoint the owned count has passed.
        /// This keeps prices from compounding across every past cap increase.
        /// </summary>
        /// <param name="ownedCount">The current total owned count.</param>
        public int GetPriceLevelSinceLastCapIncrease(int ownedCount, out int priceSet)
        {
            ///
            priceSet = 0;

            ///
            if (breakpoints == null)
            {
                priceSet = 0;
                return ownedCount;
            }

            ///
            int priceLevel = ownedCount;
            for (int i = 1; i < breakpoints.Count; i++)
            {
                var previousCapBreakpoint = breakpoints[i - 1];
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
