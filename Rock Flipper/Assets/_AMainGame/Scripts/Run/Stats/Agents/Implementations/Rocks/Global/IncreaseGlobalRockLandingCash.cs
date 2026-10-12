using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseGlobalRockLandingCash : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalRockLandingCash += addingLevel * buildValuePerLevel;
        }
    }
}
