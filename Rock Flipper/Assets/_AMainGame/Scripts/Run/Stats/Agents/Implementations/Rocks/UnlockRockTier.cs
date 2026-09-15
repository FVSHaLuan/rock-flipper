using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class UnlockRockTier : RockBuildAgent
    {
        protected override void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            if ((currentLevel + addingLevel) > 0)
            {
                rockTierBuildStats.unlocked = true;
            }
        }
    }
}