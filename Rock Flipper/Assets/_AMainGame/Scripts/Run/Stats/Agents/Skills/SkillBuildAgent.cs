using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    [System.Obsolete]
    public abstract class SkillBuildAgent : BuildAgent
    {
        public abstract string GetTooltipText(int currentLevel, double buildValuePerLevel);
    }

}