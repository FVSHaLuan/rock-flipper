using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RocksSpawner : ExtendedMonoBehaviourRun
    {
        protected void Start()
        {
            ///
            if (CurrentRunState == RunState.Combat)
            {
                SpawnOldRocks();
            }

            ///
            StateManager.OnCombatStarted += StateManager_OnCombatStarted;
        }

        private void StateManager_OnCombatStarted()
        {
            SpawnOldRocks();
        }

        private void SpawnOldRocks()
        {
            foreach (RockTier rockTier in RockTierExtensions.AllTiers())
            {
                SpawnOldRocks(rockTier);
            }
        }

        private void SpawnOldRocks(RockTier rockTier)
        {
            var tierBuildStats = BuildStats.GetRockTierBuildStats(rockTier);
            var count = tierBuildStats.count;
            if (count <= 0)
            {
                return;
            }

            ///
            var regularRockPrototype = RunEntry.prototypeManager.GetRockPrototype(rockTier, false);
            var pureRockPrototype = RunEntry.prototypeManager.GetRockPrototype(rockTier, true);

            ///
            for (int i = 0; i < count; i++)
            {
                bool isPure = Random.value <= tierBuildStats.purity;
                var rockPrototype = isPure ? pureRockPrototype : regularRockPrototype;
                RunEntry.rockInstanceManager.SpawnAsOldRock(rockPrototype.PoolHandler); 
            }
        }
    }

}