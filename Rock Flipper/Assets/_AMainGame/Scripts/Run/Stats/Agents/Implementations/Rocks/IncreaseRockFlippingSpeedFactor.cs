using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockFlippingSpeedFactor : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.flippingSpeedFactor += addingLevel * (float)buildValuePerLevel;
        }
    }
}
