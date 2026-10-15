using UnityEngine;

namespace Agame.Run.Dev
{
    public class SkillTreeReloader : ExtendedMonoBehaviourRun
    {
        [ContextMenu("ReloadSkillTree"), PlayModeOnly]
        public void ReloadSkillTree()
        {
            RunEntry.skillTree.DrawTreeWithoutApplyingToBuildStats();
            RunEntry.RebuildBuildStats();
        }
    }
}
