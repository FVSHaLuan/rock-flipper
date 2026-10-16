using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreasePreferredMultiplierPerBackground : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.preferredMultiplierPerBackground += addingLevel * buildValuePerLevel;
        }
    }
}
