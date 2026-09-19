using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotCount : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotCount += addingLevel * (int)buildValuePerLevel;
        }
    }
}
