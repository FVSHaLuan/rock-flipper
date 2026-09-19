using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RockCountCostConfig : TieredCapScalingCostConfig
    {
        [SerializeField]
        private RockTier rockTier;

        protected override int GetPriceLevelSinceLastCapIncrease(int currentLevel, out int priceSet)
        {
            var tierBuildStats = BuildStats.GetRockTierBuildStats(rockTier);
            return tierBuildStats.GetPriceLevelSinceLastCapIncrease(currentLevel, out priceSet);
        }
    }
}
