using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseGlobalRockPurityCashMultiplier : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalRockPurityCashMultiplier += addingLevel * (float)buildValuePerLevel;
        }
    }
}
