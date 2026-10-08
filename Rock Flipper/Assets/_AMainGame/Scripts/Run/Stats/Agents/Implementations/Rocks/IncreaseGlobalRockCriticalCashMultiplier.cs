using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseGlobalRockCriticalCashMultiplier : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalCriticalCashMultiplier += addingLevel * (float)buildValuePerLevel;
        }
    }
}
