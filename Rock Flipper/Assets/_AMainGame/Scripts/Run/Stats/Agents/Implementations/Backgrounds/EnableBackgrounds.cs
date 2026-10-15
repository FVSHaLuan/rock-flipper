using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class EnableBackgrounds : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            if (currentLevel + addingLevel > 0)
            {
                BuildStats.BackgroundsEnabled = true;
            }
        }
    }
}
