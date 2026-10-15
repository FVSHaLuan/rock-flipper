using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreasePrismTemporaryMultiplierIncrease : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.prismTemporaryMultiplierIncrease += addingLevel * (float)buildValuePerLevel;
        }
    }
}
