using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotMovementSpeedFactor : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotMovementSpeedFactor += addingLevel * (float)buildValuePerLevel;
        }
    }
}
