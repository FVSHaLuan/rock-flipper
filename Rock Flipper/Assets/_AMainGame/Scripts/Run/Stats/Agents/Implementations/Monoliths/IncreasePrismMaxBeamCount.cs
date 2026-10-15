using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreasePrismMaxBeamCount : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.prismMaxBeamCount += addingLevel * (int)buildValuePerLevel;
        }
    }
}
