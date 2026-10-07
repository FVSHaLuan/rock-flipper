using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class ReducePrismShootingInterval : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.prismShootingInterval -= addingLevel * (float)buildValuePerLevel;
        }
    }
}
