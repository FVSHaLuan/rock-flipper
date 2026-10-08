using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockCriticalCashMultiplier : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.criticalCashMultiplier += addingLevel * (float)buildValuePerLevel;
        }
    }
}
