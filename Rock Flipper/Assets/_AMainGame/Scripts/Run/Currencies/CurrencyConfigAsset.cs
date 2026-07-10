using Agame.GamePlatform;
using UnityEngine;

namespace Agame.Run
{
    public class CurrencyConfigAsset : ScriptableObjectWithInit, IUnityCustomArrayElementHeader
    {
        [SerializeField, HideInInspector]
        private string header;

        [SerializeField, ReadOnly]
        private Currency currency;
        [SerializeField]
        private bool isInCombatOnly;

        [Header("Cash")]
        [SerializeField, Range(0, 100)]
        private int defaultAutoSellPercentage;
        [SerializeField, LargeNumberField]
        private double baseCashValue = 1;
        [SerializeField]
        private double cashValueMultiplierPerLevel = 1.5f;
        [SerializeField]
        private double baseLevelUpCost = 10;
        [SerializeField]
        private double levelUpCostGrowthFactor = 1.2f;

        [Header("Size")]
        [SerializeField]
        private int mediumValueScale = 3;
        [SerializeField]
        private int bigValueScale = 5;

        [Header("Visual")]
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private Color color;
        [SerializeField]
        private string textSprite;

        [Space]
        [SerializeField]
        private AchievementConfig achievementConfig;

        [System.NonSerialized]
        private string currencyName;
        [System.NonSerialized]
        private string currencyNameNoColor;

        public Currency Currency => currency;
        public double BaseCashValue => baseCashValue;
        public Sprite Icon => icon;
        public Color Color => color;
        public string CurrencyName
        {
            get
            {
                ///
                TryInit();

                return currencyName;
            }
        }
        public string CurrencyNameNoColor
        {
            get
            {
                ///
                TryInit();

                return currencyNameNoColor;
            }
        }

        public AchievementConfig AchievementConfig => achievementConfig;

        public string GetHeader(int index) => header;

        public bool IsValid => true;
        public bool IsInCombatOnly => isInCombatOnly;

        protected override void Init()
        {
            ///
            base.Init();

            ///
            currencyName = string.Format("<sprite name=\"{0}\" color=\"#{1}\"/>", textSprite, ColorUtility.ToHtmlStringRGB(color));
            currencyNameNoColor = string.Format("<sprite name=\"{0}\"/>", textSprite);
        }

        public CurrencyState GetDefaultCurrencyState()
        {
            return new CurrencyState()
            {
                autoSellPercentage = defaultAutoSellPercentage,
                level = 0,
            };
        }

        public double GetCashValue(int level)
        {
            return level <= 0 ? baseCashValue : baseCashValue * cashValueMultiplierPerLevel * level;
        }

        public double GetLevelUpCost(int level)
        {
            return System.Math.Round(baseLevelUpCost * System.Math.Pow(levelUpCostGrowthFactor, level), 0);
        }

#if UNITY_EDITOR
        public void Editor_SetCurrency(Currency currency)
        {
            ///
            this.currency = currency;

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void Editor_UpdateHeader(bool isDuplicated)
        {
            ///
            if (isDuplicated)
            {
                ///
                header = "!!! Duplicated - " + currency.ToString();

                ///
                return;
            }

            ///
            if (!IsValid)
            {
                ///
                header = "!!! Invalid - " + currency.ToString();

                ///
                return;
            }

            ///
            header = currency.ToString();

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}