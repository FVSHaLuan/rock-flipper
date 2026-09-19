using Agame.Balancing;
using OneLine;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shared logic for a shop cost config whose price scales with <see cref="TieredExponentialPrice"/>
    /// and resets its scaling level after every cap increase (see <see cref="Agame.Run.Stats.MaxCountCapBreakpoints"/>).
    /// Subclasses only need to say which stat's price-level-since-cap-increase to use.
    /// </summary>
    public abstract class TieredCapScalingCostConfig : ExtendedMonoBehaviourRun, ICostConfig
    {
        [SerializeField]
        private Currency currency;
        [SerializeField, OneLineWithHeader]
        private List<TieredExponentialPrice> prices;

        public Currency Currency => currency;

        protected abstract int GetPriceLevelSinceLastCapIncrease(int currentLevel, out int priceSet);

        public CurrencyAmount GetNextLevelCost(int currentLevel)
        {
            var priceLevel = GetPriceLevelSinceLastCapIncrease(currentLevel, out var priceSet);
            if (priceSet >= prices.Count)
            {
                throw new System.Exception("Not enough prices config!");
            }

            ///
            var amount = prices[priceSet].GetPrice(priceLevel);
            return new CurrencyAmount(currency, System.Math.Round(amount));
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_LogSampleCosts")]
        private void Editor_LogSampleCosts()
        {
            for (int i = 0; i < prices.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var cost = prices[i].GetPrice(j);
                    Debug.Log($"PriceSet: {i}, PriceLevel: {j}, Cost: {System.Math.Round(cost)}");
                }
            }
        }
#endif
    }
}
