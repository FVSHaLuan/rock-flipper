using FH.Core.Architecture.Pool;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class NewChestSpawner : ExtendedMonoBehaviourRun
    {
        [SerializeField, Tooltip("Where non-Unique chests start their flip onto the playfield (on the chest bar)")]
        private Transform newChestStartPosition;
        [SerializeField, Tooltip("Effect spawned at a Unique chest's position when it drops (optional)")]
        private GeneralPoolMember uniqueChestSpawnEffectPrototype;
        [SerializeField, Min(0), Tooltip("How many of the first chests the chest bar drops in a save wait for the player to be on the Combat Screen before appearing, so the player sees them drop. Unique chests always wait.")]
        private int firstChestsWaitingForCombatScreen = 2;

        private bool IsOnCombatScreen => RunEntry.combatScreen.IsScreenActive;

        protected void Start()
        {
            RunData.OnChestLeveledUp += RunData_OnChestLeveledUp;
            RunEntry.combatScreen.OnBecomeActive += CombatScreen_OnBecomeActive;
        }

        private void RunData_OnChestLeveledUp(int levelCount)
        {
            // The chest bar drops one chest per chest level, so the chest level numbers the chests it dropped.
            var firstChestNumber = RunData.ChestLevel - levelCount + 1;
            for (int i = 0; i < levelCount; i++)
            {
                SpawnRandomChest(waitForCombatScreen: firstChestNumber + i <= firstChestsWaitingForCombatScreen);
            }
        }

        private void CombatScreen_OnBecomeActive()
        {
            while (RunEntry.chestInstanceManager.TryTakeWaitingChest(out var state))
            {
                DropChest(state);
            }
        }

        [ContextMenu("SpawnChest"), PlayModeOnly]
        private void SpawnChest()
        {
            SpawnRandomChest(waitForCombatScreen: false);
        }

        private void SpawnRandomChest(bool waitForCombatScreen)
        {
            if (!ChestUtilities.TryPickRarity(out var rarity))
            {
                Debug.LogWarning("No chest rarity has an item left to drop. No chest dropped.", this);
                return;
            }
            SpawnChest(rarity, waitForCombatScreen: waitForCombatScreen);
        }

        /// <summary>
        /// Drops a chest of <paramref name="rarity"/> the way a filled chest bar does.
        /// With <paramref name="force"/>, a Unique chest still drops when no Unique item is left for it to hold;
        /// it then holds no item.
        /// With <paramref name="waitForCombatScreen"/>, or for a Unique chest, the chest waits until the player is on the
        /// Combat Screen before appearing (see <see cref="ChestInstanceManager.WaitingChestStates"/>).
        /// </summary>
        public void SpawnChest(ChestRarity rarity, bool force = false, bool waitForCombatScreen = false)
        {
            ChestState state;
            if (rarity != ChestRarity.Unique)
            {
                state = new ChestState { rarity = rarity };
            }
            // A Unique chest picks the item it holds as it drops.
            else if (!RunEntry.uniqueChestDropRules.TryCreateUniqueChestState(out state))
            {
                if (!force)
                {
                    Debug.LogError("Picked a Unique chest, but no Unique item is left for it to hold. No chest dropped.", this);
                    return;
                }
                state = new ChestState { rarity = rarity };
            }

            ///
            if ((waitForCombatScreen || rarity == ChestRarity.Unique) && !IsOnCombatScreen)
            {
                RunEntry.chestInstanceManager.AddWaitingChest(state);
                return;
            }
            DropChest(state);
        }

        private void DropChest(ChestState state)
        {
            if (state.rarity != ChestRarity.Unique)
            {
                RunEntry.chestInstanceManager.SpawnAsNewChest(state.rarity, newChestStartPosition.position);
                return;
            }

            ///
            var chest = RunEntry.chestInstanceManager.Spawn(state);
            SpawnUniqueChestSpawnEffect(chest.transform.position);
        }

        private void SpawnUniqueChestSpawnEffect(Vector3 position)
        {
            if (uniqueChestSpawnEffectPrototype == null)
            {
                return;
            }

            ///
            var effect = CurrentGeneralPool.TakeInstance(uniqueChestSpawnEffectPrototype, this);
            effect.OnSpawned?.Invoke();
            effect.transform.position = position;
            effect.gameObject.SetActive(true);
            effect.OnAfterSetPosition?.Invoke();
        }
    }

}