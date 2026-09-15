using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockBreakingCashMultiplier : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.breakingCashMultiplier += addingLevel * buildValuePerLevel;
        }
    }
}
