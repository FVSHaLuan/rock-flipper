using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public abstract class RockBuildAgent : BuildAgent
    {
        [SerializeField]
        private RockTier rockTier;

        protected abstract void Apply(RockTierBuildStats rockTierBuildStats, int currentLevel, int addingLevel, double buildValuePerLevel);

        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            var rockTierBuildStats = BuildStats.GetRockBuildStats(rockTier);
            Apply(rockTierBuildStats, currentLevel, addingLevel, buildValuePerLevel);
        }
    }

}