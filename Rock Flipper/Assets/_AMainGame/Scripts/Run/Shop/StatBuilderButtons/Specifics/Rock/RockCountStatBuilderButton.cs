using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RockCountStatBuilderButton : RockStatBuilderButton, IMaxLevelConfig
    {
        [SerializeField]
        private Transform newRockStartPosition;

        public int MaxLevel => TierBuildStats.maxCount;

        public override void HandleLeveledUp(int levelCount)
        {
            base.HandleLeveledUp(levelCount);

            ///
            var tierBuildStats = BuildStats.GetRockTierBuildStats(Tier);

            ///
            var regularRockPrototype = RunEntry.prototypeManager.GetRockPrototype(Tier, false);
            var pureRockPrototype = RunEntry.prototypeManager.GetRockPrototype(Tier, true);

            /// Update the max level text
            for (int i = 0; i < levelCount; i++)
            {
                bool isPure = Random.value <= tierBuildStats.purity;
                var rockPrototype = isPure ? pureRockPrototype : regularRockPrototype;
                RunEntry.rockInstanceManager.SpawnAsNewRock(rockPrototype.PoolHandler, newRockStartPosition.position);
            }
        }
    }

}