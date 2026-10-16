using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Agame.Run.Stats
{
    public class BuildStatsObject : MonoBehaviourWithInit
    {
        public static event System.Action OnUnlockedSkillTree;
        public static event System.Action OnEnabledBackgrounds;

        [SerializeField]
        private bool unlockedSkillTree;

        [Header("Backgrounds")]
        [SerializeField]
        private bool backgroundsEnabled = false;
        /// <summary>
        /// Preferred Multiplier with exactly 2 backgrounds unlocked. See <see cref="GetPreferredMultiplier"/>
        /// </summary>
        public double preferredMultiplierBase = 1.5;
        /// <summary>
        /// Added to the Preferred Multiplier for every unlocked background beyond the first 2. See <see cref="GetPreferredMultiplier"/>
        /// </summary>
        public double preferredMultiplierPerBackground = 0.25;

        [Header("Mouse")]
        public bool enabledMouseHover = false;
        public bool enabledMouseRadius = false;
        public float mouseRadius = 0.5f;

        [Header("Rocks")]
        [SerializeField]
        private RockTierBuildStats rock_P0;
        [SerializeField]
        private RockTierBuildStats rock_P1;
        [SerializeField]
        private RockTierBuildStats rock_P2;
        [SerializeField]
        private RockTierBuildStats rock_P3;

        [Header("Rocks - Global")]
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.criticalLandingChance"/>
        /// </summary>
        [FormerlySerializedAs("globalCriticalLandingChance")]
        public float globalRockCriticalLandingChance = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.criticalCashMultiplier"/>
        /// </summary>
        [FormerlySerializedAs("globalCriticalCashMultiplier")]
        public float globalRockCriticalCashMultiplier = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.landingCash"/>
        /// </summary>
        public double globalRockLandingCash = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.flippingSpeedFactor"/>
        /// </summary>
        public float globalRockFlippingSpeedFactor = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.breakingCashMultiplier"/>
        /// </summary>
        public double globalRockBreakingCashMultiplier = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.purity"/>
        /// </summary>
        public float globalRockPurity = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.purityCashMultiplier"/>
        /// </summary>
        public float globalRockPurityCashMultiplier = 0;
        /// <summary>
        /// Added to every tier's <see cref="RockTierBuildStats.maxHp"/>
        /// </summary>
        public int globalRockMaxHp = 0;

        [Header("Bouncy Rocks")]
        public float bouncyRockBounceChance = 0.1f;
        public bool enabledBouncyRockRebound = false;

        [Header("Restless Rocks")]
        public float restlessRockSelfFlipDelay = 5f;
        public float restlessRockSelfFlipChance = 0.5f;

        [Header("Shockwave Rocks")]
        public float shockwaveRockShockwaveChance = 0.2f;
        public float shockwaveRockShockwaveRadius = 0.5f;
        public int shockwaveRockMaxShockwaveFlips = 3;

        [Header("Monoliths")]
        [SerializeField]
        private MonolithBuildStats monolith_LII;
        [SerializeField]
        private MonolithBuildStats monolith_LIV;
        [SerializeField]
        private MonolithBuildStats monolith_LVI;
        [SerializeField]
        private MonolithBuildStats monolith_LVIII;
        [SerializeField]
        private MonolithBuildStats monolith_LX;
        [SerializeField]
        private MonolithBuildStats monolith_LXII;

        [Header("Monoliths - L VIII (Prism)")]
        public float prismShootingInterval = 5f;
        public int prismMinBeamCount = 1;
        public int prismMaxBeamCount = 1;
        public float prismTemporaryMultiplierIncrease = 0.5f;

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
        public bool flipperBotsEnabled = false;
        public int flipperBotMaxCount = 5;
        public int flipperBotCount = 0;
        public float flipperBotFlippingInterval = 2.0f;
        [Tooltip("Radius of the circle around a Flipper Bot within which it flips rocks and chests.")]
        public float flipperBotFlipRange = 0.5f;
        [Tooltip("Max number of rocks and chests a Flipper Bot flips at once.")]
        public int flipperBotStrength = 1;
        public float flipperBotMovementSpeedFactor = 1f;
        public float flipperBotActiveTime = 10f;
        public float flipperBotIdleTime = 5f;
        public bool flipperBotChestFlippingEnabled = false;
        private MaxCountCapBreakpoints flipperBotMaxCountCapBreakpoints = new MaxCountCapBreakpoints();

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

        public bool BackgroundsEnabled
        {
            get => backgroundsEnabled;
            set
            {
                backgroundsEnabled = value;
                if (value)
                {
                    OnEnabledBackgrounds?.Invoke();
                }
            }
        }

        /// <summary>
        /// The cash income multiplier for using the Preferred background as the active background, with
        /// <paramref name="unlockedBackgroundCount"/> backgrounds unlocked. 1 with fewer than 2 unlocked, since there is
        /// no Preferred background then.
        /// </summary>
        public double GetPreferredMultiplier(int unlockedBackgroundCount)
        {
            if (unlockedBackgroundCount < 2)
            {
                return 1;
            }
            return preferredMultiplierBase + preferredMultiplierPerBackground * (unlockedBackgroundCount - 2);
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

        public float GetRockCriticalLandingChance(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).criticalLandingChance + globalRockCriticalLandingChance;
        }

        public float GetRockCriticalCashMultiplier(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).criticalCashMultiplier + globalRockCriticalCashMultiplier;
        }

        public double GetRockLandingCash(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).landingCash + globalRockLandingCash;
        }

        public float GetRockFlippingSpeedFactor(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).flippingSpeedFactor + globalRockFlippingSpeedFactor;
        }

        public double GetRockBreakingCashMultiplier(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).breakingCashMultiplier + globalRockBreakingCashMultiplier;
        }

        public float GetRockPurity(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).purity + globalRockPurity;
        }

        public float GetRockPurityCashMultiplier(RockTier rockTier)
        {
            return GetRockTierBuildStats(rockTier).purityCashMultiplier + globalRockPurityCashMultiplier;
        }

        /// <summary>
        /// Never below 1
        /// </summary>
        public int GetRockMaxHp(RockTier rockTier)
        {
            return Mathf.Max(1, GetRockTierBuildStats(rockTier).maxHp + globalRockMaxHp);
        }

        public MonolithBuildStats GetMonolithBuildStats(MonolithType monolithType)
        {
            return monolithType switch
            {
                MonolithType.LII => monolith_LII,
                MonolithType.LIV => monolith_LIV,
                MonolithType.LVI => monolith_LVI,
                MonolithType.LVIII => monolith_LVIII,
                MonolithType.LX => monolith_LX,
                MonolithType.LXII => monolith_LXII,
                _ => throw new System.NotImplementedException($"MonolithType {monolithType} is not implemented in BuildStatsObject."),
            };
        }

        /// <summary>
        /// Snapshots the current <see cref="flipperBotMaxCount"/> as a breakpoint. Call this right before
        /// raising <see cref="flipperBotMaxCount"/> (e.g. from a skill/shop upgrade) so
        /// <see cref="GetFlipperBotCountPriceLevelSinceLastCapIncrease"/> can later reset the price-scaling
        /// level relative to this cap.
        /// </summary>
        public void RecordFlipperBotMaxCountCapBreakpoint()
        {
            flipperBotMaxCountCapBreakpoints.RecordBreakpoint(flipperBotMaxCount);
        }

        /// <summary>
        /// Converts an absolute owned flipper bot count into the level to feed into the shop's price-scaling
        /// formula, resetting the count relative to the most recent cap breakpoint the owned count has passed.
        /// </summary>
        public int GetFlipperBotCountPriceLevelSinceLastCapIncrease(int ownedCount, out int priceSet)
        {
            return flipperBotMaxCountCapBreakpoints.GetPriceLevelSinceLastCapIncrease(ownedCount, out priceSet);
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