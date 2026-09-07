using Agame.Run;
using Agame.Run.Combat;
using Agame.Run.Dev;
using GD;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    [Serializable]
    public partial class RunData
    {
        private const float DefaultAuraWaveSpendingFactor = 0.5f;

        [field: System.NonSerialized]
        public event Action<Currency> OnCurrencyValueModified;
        [field: System.NonSerialized]
        public event Action<int> OnActiveBackgroundIdChanged;
        [field: System.NonSerialized]
        public event Action<int> OnPreferredBackgroundIdChanged;

        [SerializeField]
        private int slotId;
        [SerializeField]
        private int compatVersion;
        [SerializeField]
        private bool initedRun;
        [SerializeField]
        private bool finishedTutorial;
        [SerializeField]
        private bool beatFinale;
        [SerializeField]
        private bool isInPrestige;
        [SerializeField]
        private int prestigeCount;
        [SerializeField]
        private int exponentUnlockedPrestigeCount;

        [Header("Time")]
        [SerializeField]
        private float playTime;
        [SerializeField]
        private float prestigePlayTime;
        [SerializeField]
        private float previousPrestigePlayTime;

        [Header("Currency")]
        [SerializeField]
        private CurrencyValueDictionary currencyValues;
        [SerializeField]
        private List<Currency> discoveredCurrencies;

        [Header("Stat builder states")]
        [SerializeField]
        private SkillNodeStateDictionary skillNodeStateDictionary;
        [SerializeField]
        private StringIntSerializableDictionary builderButtonLevels;

        [Header("Backgrounds")]
        [SerializeField]
        private int activeBackgroundId;
        [SerializeField]
        private int preferredBackgroundId;
        [SerializeField]
        private float preferredBackgroundTimeElapsed;
        public bool visitedBackgroundShop;
        [SerializeField]
        private List<int> unlockedBackgroundIds = new List<int>() { 0 };

        [Header("Other gameplay data (RESET)")]
        [SerializeField]
        private List<ChestState> chestStates;
        [SerializeField]
        private int level;
        [SerializeField]
        private double levelProgress;
        [SerializeField]
        private int chestLevel;
        [SerializeField]
        private double chestLevelProgress;

        [Header("Other gameplay data (NO RESET)")]
        public bool showedDemoEnding = false;

        [Space]
        [NonSerialized]
        private float lastSaveTime;

        [field: NonSerialized]
        public static int UpStagedFrame { get; private set; } = -1;

        public float LastSaveTime => lastSaveTime;

        public int CompatVersion => compatVersion;
        public bool InitedRun { get => initedRun; private set => initedRun = value; }
        public bool FinishedTutorial { get => finishedTutorial; set => finishedTutorial = value; }
        public bool IsInPrestige { get => isInPrestige; }
        public int PrestigeCount { get => prestigeCount; }
        public float PlayTime => playTime;
        [field: NonSerialized]
        public float PlayTimeAdjustment { get; set; }
        public float PlayTimeNow
        {
            get
            {
                ///
                if (RunEntry.Instance == null)
                {
                    throw new System.InvalidOperationException("RunEntry.Instance is null when getting PlayTimeNow");
                }

                ///
                var delta = Time.realtimeSinceStartup - LastSaveTime;
                if (delta < 0)
                {
                    delta = 0;
                }
                return PlayTime + delta + PlayTimeAdjustment;
            }
        }
        public float PrestigePlayTimeNow
        {
            get
            {
                ///
                if (RunEntry.Instance == null)
                {
                    throw new System.InvalidOperationException("RunEntry.Instance is null when getting PlayTimeNow");
                }

                ///
                var delta = Time.realtimeSinceStartup - LastSaveTime;
                if (delta < 0)
                {
                    delta = 0;
                }
                return prestigePlayTime + delta + PlayTimeAdjustment;
            }
        }
        public float PreviousPrestigePlayTime => previousPrestigePlayTime;

        public int SlotId
        {
            get => slotId;
            private set => slotId = value;
        }

        public int ExponentUnlockedPrestigeCount { get => exponentUnlockedPrestigeCount; set => exponentUnlockedPrestigeCount = value; }

        #region Background - Properties
        public int ActiveBackgroundId
        {
            get => activeBackgroundId;
            set
            {
                if (activeBackgroundId == value) return;

                ///
                activeBackgroundId = value;

                ///
                OnActiveBackgroundIdChanged?.Invoke(activeBackgroundId);
            }
        }
        public int PreferredBackgroundId
        {
            get => preferredBackgroundId;
            set
            {
                if (preferredBackgroundId == value) return;

                ///
                preferredBackgroundId = value;
                preferredBackgroundTimeElapsed = 0;

                ///
                OnPreferredBackgroundIdChanged?.Invoke(preferredBackgroundId);
            }
        }
        public float PreferredBackgroundTimeElapsed { get => preferredBackgroundTimeElapsed; set => preferredBackgroundTimeElapsed = value; }
        public int UnlockedBackgroundCount => unlockedBackgroundIds == null ? 1 : unlockedBackgroundIds.Count;
        #endregion Background - Properties


        #region Initialization
        public void InitRun(int slotId)
        {
            this.slotId = slotId;
            this.compatVersion = Entry.Instance.compatManager.CurrentCompatVersion;

            ///
            InitedRun = true;

            ///
            RunLogger.Clear();
        }

        public void StartRun()
        {
            lastSaveTime = Time.realtimeSinceStartup;
        }

        public void CopyFrom(RunData source, int thisSlotId, bool copySkill)
        {
            if (source == this)
            {
                throw new ArgumentException("Cannot copy from itself.");
            }
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Cannot copy from a null RunData.");
            }
            if (initedRun)
            {
                throw new InvalidOperationException("Cannot copy data into an already initialized RunData.");
            }

            ///
            initedRun = true;
            slotId = thisSlotId;
            compatVersion = Entry.Instance.compatManager.CurrentCompatVersion;
            finishedTutorial = source.finishedTutorial;

            // Currency values
            if (currencyValues == null)
            {
                currencyValues = new CurrencyValueDictionary();
            }
            currencyValues[Currency.RAW_PRESTIGE] = source.GetCurrencyValue(Currency.RAW_PRESTIGE) + source.GetCurrencyValue(Currency.PRESTIGE);

            // Currency discoveries
            if (source.discoveredCurrencies != null)
            {
                discoveredCurrencies = new List<Currency>(source.discoveredCurrencies);
            }
        }
        #endregion Initialization

        #region Currencies      
        public double GetCurrencyValue(Currency currency)
        {
            if (currencyValues == null
                || !currencyValues.ContainsKey(currency))
            {
                return 0;
            }

            ///
            return currencyValues[currency];
        }

        public bool IsEnough(CurrencyAmount currencyAmount)
        {
            return IsEnough(currencyAmount.currency, currencyAmount.amount);
        }

        public bool IsEnough(Currency currency, double amount)
        {
            return GetCurrencyValue(currency) >= amount;
        }

        public bool SpendCurrency(CurrencyAmount currencyAmount)
        {
            return SpendCurrency(currencyAmount.currency, currencyAmount.amount);
        }

        public bool SpendCurrency(Currency currency, double amount)
        {
            var currentValue = GetCurrencyValue(currency);
            if (currentValue < amount)
            {
                return false;
            }

            ///
            var modifiedAmount = currentValue - amount;

            ///
            if (currencyValues == null)
            {
                currencyValues = new CurrencyValueDictionary();
            }

            ///
            currencyValues[currency] = modifiedAmount;

            ///
            ModifiedCurrenciesThisFrame.Add(currency);

            ///
            OnCurrencyValueModified?.Invoke(currency);

            ///
            return true;
        }

        public void AddCurrency(Currency currency, double amount, string logMessage)
        {
            AddCurrency(currency, amount);

#if UNITY_EDITOR
            Debug.Log($"Added Currency {currency} {amount.ToLargeNumberString()}, message: {logMessage}");
#endif
        }

        public void AddCurrency(Currency currency, double amount)
        {
            ///
            if (Mathg.Approximately(0, amount))
            {
                return;
            }

            ///
            var newValue = amount + GetCurrencyValue(currency);

            ///
            if (double.IsNaN(newValue) || double.IsInfinity(newValue))
            {
                newValue = double.MaxValue;
            }

            ///
            if (currencyValues == null)
            {
                currencyValues = new CurrencyValueDictionary();
            }

            ///
            currencyValues[currency] = newValue;

            ///
            DiscoverCurrency(currency);

            ///
            ModifiedCurrenciesThisFrame.Add(currency);

            ///
            OnCurrencyValueModified?.Invoke(currency);
        }

        public void InvokeCurrencyValueModified(Currency currency)
        {
            ModifiedCurrenciesThisFrame.Add(currency);
            OnCurrencyValueModified?.Invoke(currency);
        }

        public void ApplyCurrencyValues(CurrencyValueDictionary combatCurrencyValues)
        {
            foreach (var item in combatCurrencyValues)
            {
                AddCurrency(item.Key, item.Value);
            }
        }

        public bool HasDiscoveredCurrency(Currency currency)
        {
            if (discoveredCurrencies == null)
            {
                return false;
            }

            ///
            return discoveredCurrencies.Contains(currency);
        }

        private void DiscoverCurrency(Currency currency)
        {
            if (discoveredCurrencies == null)
            {
                discoveredCurrencies = new List<Currency>();
            }

            ///
            if (!discoveredCurrencies.Contains(currency))
            {
                discoveredCurrencies.Add(currency);
            }
        }
        #endregion Currencies

        #region Background
        public bool IsBackgroundUnlocked(int backgroundId)
        {
            if (backgroundId == 0) return true;

            ///
            if (unlockedBackgroundIds == null) return false;

            ///
            return unlockedBackgroundIds.Contains(backgroundId);
        }

        public void UnlockBackground(int backgroundId)
        {
            if (unlockedBackgroundIds == null)
            {
                unlockedBackgroundIds = new List<int>() { 0 };
            }

            ///
            if (!unlockedBackgroundIds.Contains(backgroundId))
            {
                unlockedBackgroundIds.Add(backgroundId);
            }
        }

        public void ChangeRandomPreferredBackground()
        {
            int backgroundId;
            do
            {
                backgroundId = unlockedBackgroundIds.GetRandomItem();
            }
            while (backgroundId == preferredBackgroundId);

            ///
            PreferredBackgroundId = backgroundId;
        }
        #endregion Background

        #region Stat builder states
        public SkillNodeState GetSkillNodeState(string skillNodeId)
        {
            if (skillNodeStateDictionary == null
                || !skillNodeStateDictionary.ContainsKey(skillNodeId))
            {
                return new SkillNodeState();
            }

            ///
            return skillNodeStateDictionary[skillNodeId];
        }

        public void UpdateSkillNodeState(string skillNodeId, SkillNodeState skillNodeState)
        {
            if (skillNodeStateDictionary == null)
            {
                skillNodeStateDictionary = new SkillNodeStateDictionary();
            }

            ///
            skillNodeStateDictionary[skillNodeId] = skillNodeState;
        }

        public int GetBuilderButtonLevel(string buttonId)
        {
            ///
            if (string.IsNullOrWhiteSpace(buttonId))
            {
                throw new ArgumentException("Button ID cannot be null or whitespace.", nameof(buttonId));
            }

            ///
            if (!builderButtonLevels.ContainsKey(buttonId))
            {
                return 0;
            }

            ///
            return builderButtonLevels[buttonId];
        }

        public void SetBuilderButtonLevel(string buttonId, int level)
        {
            ///
            if (string.IsNullOrWhiteSpace(buttonId))
            {
                throw new ArgumentException("Button ID cannot be null or whitespace.", nameof(buttonId));
            }

            ///
            if (level < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Level cannot be negative.");
            }

            ///
            if (builderButtonLevels == null)
            {
                builderButtonLevels = new StringIntSerializableDictionary();
            }

            ///
            builderButtonLevels[buttonId] = level;
        }
        #endregion Stat builder states

        #region Other Gameplay Data (RESET)
        public int GetChestStateCount()
        {
            if (chestStates == null)
            {
                return 0;
            }
            ///
            return chestStates.Count;
        }

        public ChestState GetChestState(int index)
        {
            if (chestStates == null || index < 0 || index >= chestStates.Count)
            {
                return new ChestState();
            }

            ///
            return chestStates[index];
        }

        public void SetChestStates(List<ChestState> states)
        {
            if (chestStates == null)
            {
                chestStates = new List<ChestState>();
            }

            ///
            chestStates.Clear();

            ///
            if (states == null)
            {
                return;
            }

            ///
            foreach (var state in states)
            {
                chestStates.Add(state);
            }
        }
        #endregion Other Gameplay Data (RESET)

        #region Other Gameplay Data (NO RESET)

        #endregion Other Gameplay Data (NO RESET)

        #region Other

        #endregion Other
    }
}