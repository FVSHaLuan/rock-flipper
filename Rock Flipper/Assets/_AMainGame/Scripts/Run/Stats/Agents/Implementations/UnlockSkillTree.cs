using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class UnlockSkillTree : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            if (currentLevel + addingLevel > 0)
            {
                BuildStats.UnlockedSkillTree = true;
            }
        }
    }
}
