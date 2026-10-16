using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class ReduceRestlessRockSelfFlipDelay : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.restlessRockSelfFlipDelay -= addingLevel * (float)buildValuePerLevel;
        }
    }
}
