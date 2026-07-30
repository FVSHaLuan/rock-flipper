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
                Spawn();
            }

            ///
            StateManager.OnCombatStarted += StateManager_OnCombatStarted;
        }

        private void StateManager_OnCombatStarted()
        {
            Spawn();
        }

        private void Spawn()
        {

        }
    }

}