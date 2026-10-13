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

#if UNITY_EDITOR
        [ContextMenu("Apply 1/1"), PlayModeOnly]
        private void Editor_ApplyOneOne()
        {
            Apply(0, 1, 0);
        }
#endif
    }
}
