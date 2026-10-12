using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseGlobalRockPurity : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalRockPurity += addingLevel * (float)buildValuePerLevel;
        }
    }
}
