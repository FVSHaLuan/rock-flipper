using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockPurityCashMultiplier : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.purityCashMultiplier += addingLevel * (float)buildValuePerLevel;
        }
    }
}
