using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotStrength : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotStrength += addingLevel * (int)buildValuePerLevel;
        }
    }
}
