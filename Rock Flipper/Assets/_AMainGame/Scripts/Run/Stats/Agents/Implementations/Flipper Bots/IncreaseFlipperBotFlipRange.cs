using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseFlipperBotFlipRange : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotFlipRange += addingLevel * (float)buildValuePerLevel;
        }
    }
}
