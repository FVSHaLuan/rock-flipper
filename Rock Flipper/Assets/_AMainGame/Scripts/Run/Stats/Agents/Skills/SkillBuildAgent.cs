using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public abstract class SkillBuildAgent : BuildAgent
    {
        public abstract string GetTooltipText(int currentLevel, double buildValuePerLevel);
    }

}