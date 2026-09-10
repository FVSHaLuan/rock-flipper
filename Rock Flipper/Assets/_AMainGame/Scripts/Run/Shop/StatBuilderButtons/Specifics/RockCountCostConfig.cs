using Agame.Balancing;
using Agame.Run.Combat;
using OneLine;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RockCountCostConfig : ExtendedMonoBehaviourRun, ICostConfig
    {
        [SerializeField]
        private RockTier rockTier;

        [Space]
        [SerializeField]
        private Currency currency;
        [SerializeField, OneLineWithHeader]
        private List<TieredExponentialPrice> prices;

        public Currency Currency => currency;

        public CurrencyAmount GetNextLevelCost(int currentLevel)
        {
            var tierBuildStats = BuildStats.GetRockTierBuildStats(rockTier);
            var priceLevel = tierBuildStats.GetPriceLevelSinceLastCapIncrease(currentLevel, out var priceSet);
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