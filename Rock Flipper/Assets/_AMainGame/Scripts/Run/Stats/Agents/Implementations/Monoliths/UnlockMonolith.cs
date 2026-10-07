using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class UnlockMonolith : BuildAgent
    {
        [SerializeField]
        private MonolithType monolithType;

        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            if (currentLevel + addingLevel > 0)
            {
                BuildStats.GetMonolithBuildStats(monolithType).unlocked = true;
            }
        }
    }
}
