using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreasePrismMinBeamCount : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.prismMinBeamCount += addingLevel * (int)buildValuePerLevel;
        }
    }
}
