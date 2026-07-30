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
            bool isPure = Random.value <= tierBuildStats.purity;
            var rockPrototype = RunEntry.prototypeManager.GetRockPrototype(rockTier, isPure);
            RunEntry.rockInstanceManager.SpawnAsOldRock(rockPrototype.PoolHandler);
        }
    }

}