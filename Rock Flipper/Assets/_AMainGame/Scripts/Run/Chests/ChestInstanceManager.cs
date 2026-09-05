using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class ChestInstanceManager : ExtendedMonoBehaviourRun
    {
        private List<Chest> activeChests = new List<Chest>();
        private List<ChestState> savedChestStates = new List<ChestState>();

        protected void Start()
        {
            entry.playerDataSaver.OnBeforeSave += PlayerDataSaver_OnBeforeSave;
        }

        private void PlayerDataSaver_OnBeforeSave()
        {
            if (CurrentRunState == RunState.Combat)
            {
                SaveChestStates();
            }
        }

        public Chest Spawn(ChestRarity chestRarity, ChestState? state = null)
        {
            var chestPrototype = RunEntry.prototypeManager.GetChestPrototype(chestRarity);
            var chest = CurrentSceneGeneralPool.TakeInstance(chestPrototype.PoolHandler, this).TargetObject;
            if (state.HasValue)
            {
                chest.ApplyState(state.Value);
            }
            chest.transform.position = Playfield.GetRandomPoint(-Vector2.one);
            chest.gameObject.SetActive(true);
            activeChests.Add(chest);
            return chest;
        }

        private void SaveChestStates()
        {
            savedChestStates.Clear();

            ///
            foreach (var chest in activeChests)
            {
                var state = chest.GetState();
                savedChestStates.Add(state);
            }

            ///
            RunData.SetChestStates(savedChestStates);
        }
    }
}