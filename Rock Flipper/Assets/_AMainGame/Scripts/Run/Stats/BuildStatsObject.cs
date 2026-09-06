using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Stats
{
    public class BuildStatsObject : MonoBehaviourWithInit
    {
        public static event System.Action OnUnlockedSkillTree;

        [SerializeField]
        private bool unlockedSkillTree;

        [Header("Player Cursor")]
        public bool enabledPlayerCursorRadius = false;
        public float playerCursorRadius = 0.5f;

        [Header("Rocks")]
        [SerializeField]
        private RockTierBuildStats rock_P0;
        [SerializeField]
        private RockTierBuildStats rock_P1;
        [SerializeField]
        private RockTierBuildStats rock_P2;
        [SerializeField]
        private RockTierBuildStats rock_P3;


        [Header("Chests")]
        [SerializeField]
        private ChestRarityBuildStats chest_Common;
        [SerializeField]
        private ChestRarityBuildStats chest_Uncommon;
        [SerializeField]
        private ChestRarityBuildStats chest_Rare;
        [SerializeField]
        private ChestRarityBuildStats chest_Epic;
        [SerializeField]
        private ChestRarityBuildStats chest_Unique;

        [Header("Flipper Bots")]
        public float flipperBotFlippingInterval = 2.0f;
        public float flipperBotMovementSpeed = 3f;

        public bool UnlockedSkillTree
        {
            get => unlockedSkillTree;
            set
            {
                unlockedSkillTree = value;
                if (value)
                {
                    OnUnlockedSkillTree?.Invoke();
                }
            }
        }

        public RockTierBuildStats GetRockTierBuildStats(RockTier rockTier)
        {
            return rockTier switch
            {
                RockTier.P0 => rock_P0,
                RockTier.P1 => rock_P1,
                RockTier.P2 => rock_P2,
                RockTier.P3 => rock_P3,
                _ => throw new System.NotImplementedException($"RockTier {rockTier} is not implemented in BuildStatsObject."),
            };
        }

        public ChestRarityBuildStats GetChestRarityBuildStats(ChestRarity chestRarity)
        {
            return chestRarity switch
            {
                ChestRarity.Common => chest_Common,
                ChestRarity.Uncommon => chest_Uncommon,
                ChestRarity.Rare => chest_Rare,
                ChestRarity.Epic => chest_Epic,
                ChestRarity.Unique => chest_Unique,
                _ => throw new System.NotImplementedException($"ChestRarity {chestRarity} is not implemented in BuildStatsObject."),
            };
        }
    }
}