using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockLevelExp : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.levelExp += addingLevel * buildValuePerLevel;
        }
    }
}
