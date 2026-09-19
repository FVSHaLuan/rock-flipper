using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockMaxCount : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            for (int i = 0; i < addingLevel; i++)
            {
                rockTierBuildStats.RecordMaxCountCapBreakpoint();
                rockTierBuildStats.maxCount += (int)buildValuePerLevel; 
            }
        }
    }
}