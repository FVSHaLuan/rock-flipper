using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class ReduceRockLandingCooldown : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.landingCooldown -= addingLevel * (float)buildValuePerLevel;
        }
    }
}
