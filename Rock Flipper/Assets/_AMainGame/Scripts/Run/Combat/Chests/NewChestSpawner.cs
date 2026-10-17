using UnityEngine;

namespace Agame.Run.Combat
{
    public class NewChestSpawner : ExtendedMonoBehaviourRun
    {
        [SerializeField, Tooltip("Where non-Unique chests start their flip onto the playfield (on the chest bar)")]
        private Transform newChestStartPosition;

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
            if (rarity != ChestRarity.Unique)
            {
                RunEntry.chestInstanceManager.SpawnAsNewChest(rarity, newChestStartPosition.position);
                return;
            }

            // A Unique chest picks the item it holds as it drops.
            if (RunEntry.uniqueChestDropRules.TryCreateUniqueChestState(out var state))
            {
                RunEntry.chestInstanceManager.Spawn(state);
            }
            else
            {
                Debug.LogError("Picked a Unique chest, but no Unique item is left for it to hold. No chest dropped.", this);
            }
        }
    }

}