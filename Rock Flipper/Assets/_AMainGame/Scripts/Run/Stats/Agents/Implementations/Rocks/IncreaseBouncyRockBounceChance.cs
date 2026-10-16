using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseBouncyRockBounceChance : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.bouncyRockBounceChance += addingLevel * (float)buildValuePerLevel;
        }
    }
}
