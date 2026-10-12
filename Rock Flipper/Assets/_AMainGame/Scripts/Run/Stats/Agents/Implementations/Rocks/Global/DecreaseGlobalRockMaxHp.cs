using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class DecreaseGlobalRockMaxHp : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.globalRockMaxHp -= addingLevel * (int)buildValuePerLevel;
        }
    }
}
