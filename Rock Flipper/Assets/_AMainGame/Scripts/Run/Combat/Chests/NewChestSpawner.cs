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
            RunEntry.chestInstanceManager.Spawn(rarity);
        }
    }

}