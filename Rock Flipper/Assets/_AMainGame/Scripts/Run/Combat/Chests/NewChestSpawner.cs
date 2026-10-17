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

        protected void Start()
        {
            RunData.OnChestLeveledUp += RunData_OnChestLeveledUp;
        }

        private void RunData_OnChestLeveledUp(int levelCount)
        {
            for (int i = 0; i < levelCount; i++)
            {
                SpawnChest();
            }
        }

        [ContextMenu("SpawnChest"), PlayModeOnly]
        private void SpawnChest()
        {
            if (!ChestUtilities.TryPickRarity(out var rarity))
            {
                Debug.LogWarning("No chest rarity has an item left to drop. No chest dropped.", this);
                return;
            }
            SpawnChest(rarity);
        }

        /// <summary>
        /// Drops a chest of <paramref name="rarity"/> the way a filled chest bar does.
        /// With <paramref name="force"/>, a Unique chest still drops when no Unique item is left for it to hold;
        /// it then holds no item.
        /// </summary>
        public void SpawnChest(ChestRarity rarity, bool force = false)
        {
            if (rarity != ChestRarity.Unique)
            {
                RunEntry.chestInstanceManager.SpawnAsNewChest(rarity, newChestStartPosition.position);
                return;
            }

            // A Unique chest picks the item it holds as it drops.
            if (RunEntry.uniqueChestDropRules.TryCreateUniqueChestState(out var state))
            {
                var chest = RunEntry.chestInstanceManager.Spawn(state);
                SpawnUniqueChestSpawnEffect(chest.transform.position);
            }
            else if (force)
            {
                var chest = RunEntry.chestInstanceManager.Spawn(rarity);
                SpawnUniqueChestSpawnEffect(chest.transform.position);
            }
            else
            {
                Debug.LogError("Picked a Unique chest, but no Unique item is left for it to hold. No chest dropped.", this);
            }
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