using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseGlobalRockCriticalLandingChance : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalRockCriticalLandingChance += addingLevel * (float)buildValuePerLevel;
        }
    }
}
