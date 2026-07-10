using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    /// <summary>
    /// Input is still multiplicative factor, but tooltip shows percentage.
    /// </summary>
    public abstract class SkillBuildAgentWithSimpleTooltipPercentage : SkillBuildAgentWithSimpleTooltip
    {
        protected sealed override string GetBuildValuePerLevelText(double buildValuePerLevel)
        {
            return $"{buildValuePerLevel * 100:0.###}%";
        }        

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {

        }
#endif
    }

}