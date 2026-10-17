using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class ChestInstanceManager : ExtendedMonoBehaviourRun
    {
        private List<Chest> activeChests = new List<Chest>();
        private List<ChestState> savedChestStates = new List<ChestState>();
        private List<ChestState> waitingChestStates = new List<ChestState>();

        public IReadOnlyList<Chest> ActiveChests => activeChests;

        /// <summary>
        /// Chests that dropped while the player wasn't on the Combat Screen and aren't on the playfield yet
        /// (see <see cref="NewChestSpawner"/>). Saved with the chests on the playfield, so they appear in place on the next load.
        /// </summary>
        public IReadOnlyList<ChestState> WaitingChestStates => waitingChestStates;

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

        public void AddWaitingChest(ChestState chestState)
        {
            waitingChestStates.Add(chestState);
        }

        /// <summary>
        /// Removes the oldest waiting chest and returns its state, to spawn it. Returns false if no chest is waiting.
        /// </summary>
        public bool TryTakeWaitingChest(out ChestState chestState)
        {
            if (waitingChestStates.Count == 0)
            {
                chestState = default;
                return false;
            }

            chestState = waitingChestStates[0];
            waitingChestStates.RemoveAt(0);
            return true;
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
            savedChestStates.AddRange(waitingChestStates);

            ///
            RunData.SetChestStates(savedChestStates);
        }
    }
}