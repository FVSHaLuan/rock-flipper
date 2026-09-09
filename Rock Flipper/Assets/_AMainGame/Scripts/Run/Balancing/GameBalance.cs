using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Balancing
{
    public partial class GameBalance : ScriptableObjectWithInit
    {
        public int GetRequiredExpForNextItemLevel(int currentLevel)
        {
            if (currentLevel < 0)
            {
                Debug.LogError("Current level cannot be negative.");
                return 0;
            }

            ///
            return 10;
        }

        public int GetItemMaxLevel(ChestRarity chestRarity)
        {
            return 20;
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
