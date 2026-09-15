using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockChestLevelExp : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.chestLevelExp += addingLevel * buildValuePerLevel;
        }
    }
}
