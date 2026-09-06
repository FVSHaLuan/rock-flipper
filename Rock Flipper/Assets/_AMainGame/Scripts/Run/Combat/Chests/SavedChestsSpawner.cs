using UnityEngine;

namespace Agame.Run.Combat
{
    public class SavedChestsSpawner : ExtendedMonoBehaviourRun
    {
        protected void Start()
        {
            if (CurrentRunState == RunState.Combat)
            {
                SpawnSavedChests();
            }

            ///
            RunEntry.runStateManager.OnCombatStarted += RunStateManager_OnCombatStarted;
        }

        private void RunStateManager_OnCombatStarted()
        {
            SpawnSavedChests();
        }

        private void SpawnSavedChests()
        {
            ///
            var chestStateCount = RunData.GetChestStateCount();
            for (int i = 0; i < chestStateCount; i++)
            {
                var chestState = RunData.GetChestState(i);
                RunEntry.chestInstanceManager.Spawn(chestState);
            }

            ///
            RunEntry.chestInstanceManager.SetSpawnedSavedChest();
        }
    }

}