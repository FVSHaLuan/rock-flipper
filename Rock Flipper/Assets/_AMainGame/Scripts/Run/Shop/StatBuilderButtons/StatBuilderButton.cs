using Agame.Run.Stats.Agents;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    public class StatBuilderButton : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private IDAsset buttonIDAsset;

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

        protected void OnEnable()
        {
            UpdateViewAll();
        }

        public void Apply()
        {
            ///
            TryInit();

            ///
            var level = RunData.GetBuilderButtonLevel(buttonIDAsset.ID);
            buildAgent.Apply(0, level, buildValueConfig.BuildValue);
        }

        private void UpdateViewAll()
        {
            var level = RunData.GetBuilderButtonLevel(buttonIDAsset.ID);
            UpdateViewContent(level);
            UpdateInteractability(level);
        }

        private void UpdateViewContent(int level = -1)
        {
            if (level < 0)
            {
                level = RunData.GetBuilderButtonLevel(buttonIDAsset.ID);
            }
            levelText.Text = $"{level}/{maxLevelConfig.MaxLevel}";
            costText.Text = level < maxLevelConfig.MaxLevel ? $"{costConfig.GetCost(level).GetFormattedString()}" : "MAXED";
        }

        private void UpdateInteractability(int level = -1)
        {
            if (level < 0)
            {
                level = RunData.GetBuilderButtonLevel(buttonIDAsset.ID);
            }
            var isInteractable = level < maxLevelConfig.MaxLevel && RunData.IsEnough(costConfig.GetCost(level));
            button.interactable = isInteractable;
        }
    }

}