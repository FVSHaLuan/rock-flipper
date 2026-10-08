using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockCriticalLandingChance : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.criticalLandingChance += addingLevel * (float)buildValuePerLevel;
        }
    }
}
