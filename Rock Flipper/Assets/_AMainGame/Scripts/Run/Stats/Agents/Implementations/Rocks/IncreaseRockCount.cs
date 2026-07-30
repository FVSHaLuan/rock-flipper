using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockCount : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.count += addingLevel * (int)buildValuePerLevel;
        }
    }
}