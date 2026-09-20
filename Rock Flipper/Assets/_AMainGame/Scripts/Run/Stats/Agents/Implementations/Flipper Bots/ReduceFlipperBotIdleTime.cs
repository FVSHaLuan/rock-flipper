using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class ReduceFlipperBotIdleTime : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotIdleTime -= addingLevel * (float)buildValuePerLevel;
        }
    }
}
