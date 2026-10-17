using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class ChestInstanceManager : ExtendedMonoBehaviourRun
    {
        private List<Chest> activeChests = new List<Chest>();
        private List<ChestState> savedChestStates = new List<ChestState>();

        public IReadOnlyList<Chest> ActiveChests => activeChests;

        public bool SpawnedSavedChests { get; private set; } = false;

        protected void Start()
        {
            entry.playerDataSaver.OnBeforeSave += PlayerDataSaver_OnBeforeSave;
        }

        protected void OnDestroy()
        {
            entry.playerDataSaver.OnBeforeSave -= PlayerDataSaver_OnBeforeSave;
        }

        private void PlayerDataSaver_OnBeforeSave()
        {
            if (SpawnedSavedChests && CurrentRunState == RunState.Combat)
            {
                SaveChestStates();
            }
        }

        public void RemoveActiveChest(Chest chest)
        {
            if (activeChests.Contains(chest))
            {
                activeChests.Remove(chest);
            }
        }

        public Chest Spawn(ChestState chestState)
        {
            return Spawn(chestState.rarity, chestState);
        }

        public Chest Spawn(ChestRarity chestRarity, ChestState? state = null)
        {
            return Spawn(chestRarity, state, GetSpawnPoint());
        }

        /// <summary>
        /// Spawns a chest at <paramref name="startPosition"/> that flips onto the playfield (e.g. dropping from the chest bar)
        /// </summary>
        public Chest SpawnAsNewChest(ChestRarity chestRarity, Vector2 startPosition)
        {
            var chest = Spawn(chestRarity, null, startPosition);

            ///
            chest.DoNewChestFlipping(GetSpawnPoint());

            ///
            return chest;
        }

        private Chest Spawn(ChestRarity chestRarity, ChestState? state, Vector2 position)
        {
            var chestPrototype = RunEntry.prototypeManager.GetChestPrototype(chestRarity);
            var chest = CurrentSceneGeneralPool.TakeInstance(chestPrototype.PoolHandler, this).TargetObject;
            // Pooled chests are reused, so a chest without a saved state still gets a fresh one.
            chest.ApplyState(state ?? new ChestState { rarity = chestRarity });
            chest.transform.position = position;
            chest.gameObject.SetActive(true);
            chest.StartNewLife();

            ///
            activeChests.Add(chest);

            ///
            return chest;
        }

        private Vector2 GetSpawnPoint()
        {
            return Playfield.GetRandomPoint(-Vector2.one);
        }

        public void SetSpawnedSavedChest()
        {
            SpawnedSavedChests = true;
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