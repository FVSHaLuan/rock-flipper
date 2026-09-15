using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockPurity : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.purity += addingLevel * (float)buildValuePerLevel;
        }
    }
}
