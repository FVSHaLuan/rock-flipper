using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotMaxCount : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotMaxCount += addingLevel * (int)buildValuePerLevel;
        }
    }
}
