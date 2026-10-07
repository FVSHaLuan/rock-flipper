using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class UnlockPrism : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            if (currentLevel + addingLevel > 0)
            {
                BuildStats.GetMonolithBuildStats(MonolithType.LVIII).unlocked = true;
            }
        }
    }
}
