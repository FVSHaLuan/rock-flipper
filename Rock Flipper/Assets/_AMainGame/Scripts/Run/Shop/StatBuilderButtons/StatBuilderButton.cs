using Agame.Run.Stats.Agents;
using UnityEngine;
using UnityEngine.Assertions;

namespace Agame.Run.Shop
{
    public class StatBuilderButton : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private IDAsset buttonIDAsset;

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

        public void Apply()
        {
            ///
            TryInit();

            ///
            var level = RunData.GetBuilderButtonLevel(buttonIDAsset.ID);
            buildAgent.Apply(0, level, buildValueConfig.BuildValue);
        }
    }

}