using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseRestlessRockSelfFlipChance : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.restlessRockSelfFlipChance += addingLevel * (float)buildValuePerLevel;
        }
    }
}
