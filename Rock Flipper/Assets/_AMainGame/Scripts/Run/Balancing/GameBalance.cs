using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Balancing
{
    public partial class GameBalance : ScriptableObjectWithInit
    {
        public int GetRequiredExpForNextItemLevel(ChestRarity chestRarity, int currentLevel)
        {
            if (currentLevel < 0)
            {
                Debug.LogError("Current level cannot be negative.");
                return 0;
            }

            if (chestRarity == ChestRarity.Unique)
            {
                return 1;
            }

            ///
            return 10;
        }

        public int GetItemMaxLevel(ChestRarity chestRarity)
        {
            switch (chestRarity)
            {
                case ChestRarity.Common:
                    return 50;
                case ChestRarity.Uncommon:
                    return 20;
                case ChestRarity.Rare:
                    return 10;
                case ChestRarity.Epic:
                    return 5;
                case ChestRarity.Unique:
                    return 1;
                default:
                    throw new System.NotImplementedException($"Max level for chest rarity {chestRarity} is not defined.");
            }
        }

        public int GetItemExpPerFind(ChestRarity chestRarity)
        {
            return 1;
        }

        public double GetRequiredExpForNextLevel(int currentLevel)
        {
            if (currentLevel < 0)
            {
                Debug.LogError("Current level cannot be negative.");
                return 0;
            }

            ///
            return 1000;
        }

        public double GetRequiredExpForNextChestLevel(int currentLevel)
        {
            if (currentLevel < 0)
            {
                Debug.LogError("Current level cannot be negative.");
                return 0;
            }

            ///
            return 1000;
        }
    }
}
