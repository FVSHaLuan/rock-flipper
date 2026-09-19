using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotMaxCount : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            for (int i = 0; i < addingLevel; i++)
            {
                BuildStats.RecordFlipperBotMaxCountCapBreakpoint();
                BuildStats.flipperBotMaxCount += (int)buildValuePerLevel;
            }
        }
    }
}
