using UnityEngine;

namespace Agame.Run.Combat
{
    public class NewChestSpawner : ExtendedMonoBehaviourRun
    {
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
            var rarity = ChestUtilities.PickRarity();
            if (rarity != ChestRarity.Unique)
            {
                RunEntry.chestInstanceManager.Spawn(rarity);
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