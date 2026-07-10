using BT.GamePlatform;
using UnityEngine;

namespace BT.Run.Stats.Agents
{
    [DisallowMultipleComponent]
    public abstract class BuildAgent : ExtendedMonoBehaviourRun
    {
        public abstract void Apply(int currentLevel, int addingLevel, double buildValuePerLevel);

        public virtual bool Verify(double buildValue, int maxLevel)
        {
            ///
            return true;
        }
    }
}