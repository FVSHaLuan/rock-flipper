using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class ReduceFlipperBotFlippingInterval : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.flipperBotFlippingInterval -= addingLevel * (float)buildValuePerLevel;
        }
    }
}
