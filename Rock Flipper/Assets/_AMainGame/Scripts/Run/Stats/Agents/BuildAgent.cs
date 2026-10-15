using Agame.GamePlatform;
using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    [DisallowMultipleComponent]
    public abstract class BuildAgent : ExtendedMonoBehaviourRun
    {
        public virtual bool RefundCost => false;

        public abstract void Apply(int currentLevel, int addingLevel, double buildValuePerLevel);

        public virtual bool Verify(double buildValue, int maxLevel)
        {
            ///
            return true;
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