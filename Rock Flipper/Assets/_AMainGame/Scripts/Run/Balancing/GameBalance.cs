using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Balancing
{
    public partial class GameBalance : ScriptableObjectWithInit
    {
        public const int ChestExpRequired = 1000;

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

        #region Chests and items
        public static int GetRequiredExpForNextItemLevel(ChestRarity chestRarity, int currentLevel)
        {
            if (currentLevel < 0)
            {
                Debug.LogError("Current level cannot be negative.");
                return 0;
            }

            ///
            switch (chestRarity)
            {
                case ChestRarity.Common:
                    return 2 + currentLevel * 2;
                case ChestRarity.Uncommon:
                    return 2 + currentLevel * 2;
                case ChestRarity.Rare:
                    return 2 + currentLevel * 2;
                case ChestRarity.Epic:
                    return 2 + currentLevel * 2;
                case ChestRarity.Unique:
                    return 1;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public static int GetItemMaxLevel(ChestRarity chestRarity)
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

        public static double GetRequiredExpForNextChestLevel(int currentLevel) => ChestExpRequired;

        public static int GetItemExpPerFind(ChestRarity chestRarity) => 1;
        #endregion Chests and items
    }
}
