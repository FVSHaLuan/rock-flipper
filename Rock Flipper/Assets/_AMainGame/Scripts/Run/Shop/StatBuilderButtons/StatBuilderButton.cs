using Agame.Run.Stats;
using Agame.Run.Stats.Agents;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    public class StatBuilderButton : ExtendedMonoBehaviourRun, IStatBuilder
    {
        [SerializeField]
        private string buttonId;

        [Space]
        [SerializeField]
        private Button button;
        [SerializeField]
        private UnifiedText levelText;
        [SerializeField]
        private UnifiedText costText;

        private BuildAgent buildAgent;
        private ICostConfig costConfig;
        private IBuildValueConfig buildValueConfig;
        private IStatBuilderButtonSpecific statBuilderButtonSpecific;
        private IMaxLevelConfig maxLevelConfig;

        protected override bool Init()
        {
            ///
            buildAgent = GetComponentInParent<BuildAgent>();
            costConfig = GetComponentInParent<ICostConfig>();
            buildValueConfig = GetComponentInParent<IBuildValueConfig>();
            maxLevelConfig = GetComponentInParent<IMaxLevelConfig>();

            ///
            Assert.IsNotNull(buildAgent, $"StatBuilderButton: {name} is missing BuildAgent component in parent.");
            Assert.IsNotNull(costConfig, $"StatBuilderButton: {name} is missing ICostConfig component in parent.");
            Assert.IsNotNull(buildValueConfig, $"StatBuilderButton: {name} is missing IBuildValueConfig component in parent.");
            Assert.IsNotNull(maxLevelConfig, $"StatBuilderButton: {name} is missing IMaxLevelConfig component in parent.");

            ///
            statBuilderButtonSpecific = GetComponentInParent<IStatBuilderButtonSpecific>();
            if (statBuilderButtonSpecific == null)
            {
                statBuilderButtonSpecific = new StatBuilderButtonDefaultSpecific();
            }

            ///
            return base.Init();
        }

        protected void OnDisable()
        {
            ///
            RunData.OnCurrencyValueModifiedThisFrame -= RunData_OnCurrencyValueModifiedThisFrame;
        }

        protected void OnEnable()
        {
            ///
            UpdateViewAll();

            ///
            RunData.OnCurrencyValueModifiedThisFrame += RunData_OnCurrencyValueModifiedThisFrame;
        }

        private void RunData_OnCurrencyValueModifiedThisFrame(Currency currency)
        {
            if (currency == costConfig.Currency)
            {
                UpdateInteractability();
            }
        }

        public void ApplyToBuildStats()
        {
            ///
            TryInit();

            ///
            var level = RunData.GetBuilderButtonLevel(buttonId);
            buildAgent.Apply(0, level, buildValueConfig.BuildValue);
        }

        private void UpdateViewAll()
        {
            var level = RunData.GetBuilderButtonLevel(buttonId);
            UpdateViewContent(level);
            UpdateInteractability(level);
        }

        private void UpdateViewContent(int level = -1)
        {
            if (level < 0)
            {
                level = RunData.GetBuilderButtonLevel(buttonId);
            }
            levelText.Text = $"{level}/{maxLevelConfig.MaxLevel}";
            costText.Text = level < maxLevelConfig.MaxLevel ? $"{costConfig.GetNextLevelCost(level).GetFormattedString()}" : "MAXED";
        }

        private void UpdateInteractability(int level = -1)
        {
            if (level < 0)
            {
                level = RunData.GetBuilderButtonLevel(buttonId);
            }
            var isInteractable = level < maxLevelConfig.MaxLevel && RunData.IsEnough(costConfig.GetNextLevelCost(level));
            button.interactable = isInteractable;
        }

        public void HandleClick()
        {
            var level = RunData.GetBuilderButtonLevel(buttonId);
            if (level >= maxLevelConfig.MaxLevel)
            {
                return;
            }

            ///
            var cost = costConfig.GetNextLevelCost(level);
            if (RunData.SpendCurrency(cost))
            {
                LevelUp(level, 1);
                UpdateViewAll();
            }
        }

        private void LevelUp(int currentLevel, int levelCount)
        {
            ///
            buildAgent.Apply(currentLevel, levelCount, buildValueConfig.BuildValue);

            ///
            RunData.SetBuilderButtonLevel(buttonId, currentLevel + levelCount);

            ///
            statBuilderButtonSpecific.HandleLeveledUp(levelCount);
        }
    }

}