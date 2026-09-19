using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class IncreaseMouseRadius : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            BuildStats.mouseRadius += addingLevel * (float)buildValuePerLevel;
        }
    }
}
