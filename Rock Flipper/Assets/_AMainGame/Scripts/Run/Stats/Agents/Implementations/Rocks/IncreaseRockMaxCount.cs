using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockMaxCount : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.RecordMaxCountCapBreakpoint();
            rockTierBuildStats.maxCount += addingLevel * (int)buildValuePerLevel;
        }
    }
}