using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class EnableMouseHover : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            if (currentLevel + addingLevel > 0)
            {
                BuildStats.enabledMouseHover = true;
            }
        }
    }
}
