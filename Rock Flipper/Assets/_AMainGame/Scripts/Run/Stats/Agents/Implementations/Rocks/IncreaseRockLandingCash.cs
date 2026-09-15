using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRockLandingCash : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            rockTierBuildStats.landingCash += addingLevel * buildValuePerLevel;
        }
    }

}