using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotActiveTime : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotActiveTime += addingLevel * (float)buildValuePerLevel;
        }
    }
}
