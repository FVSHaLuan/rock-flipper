using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseGlobalRockFlippingSpeedFactor : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalRockFlippingSpeedFactor += addingLevel * (float)buildValuePerLevel;
        }
    }
}
