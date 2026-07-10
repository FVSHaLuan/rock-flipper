using UnityEngine;

namespace BT.Run.Stats.Agents
{
    public abstract class SkillBuildAgentWithSimpleTooltip : SkillBuildAgent
    {
        [SerializeField, TextArea]
        private string tooltipFormat = "<Mockup tooltip, buildValue: {0}>";

        protected virtual string GetBuildValuePerLevelText(double buildValuePerLevel)
        {
            return buildValuePerLevel.ToLargeNumberString();
        }

        public override string GetTooltipText(int currentLevel, double buildValuePerLevel)
        {
            return string.Format(tooltipFormat, GetBuildValuePerLevelText(buildValuePerLevel));
        }
    }

}