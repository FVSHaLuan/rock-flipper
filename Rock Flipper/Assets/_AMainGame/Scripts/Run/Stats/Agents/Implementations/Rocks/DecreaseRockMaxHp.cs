using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class DecreaseRockMaxHp : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.maxHp -= addingLevel * (int)buildValuePerLevel;
        }
    }

}