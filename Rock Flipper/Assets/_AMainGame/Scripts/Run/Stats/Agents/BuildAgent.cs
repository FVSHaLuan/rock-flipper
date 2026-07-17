using Agame.GamePlatform;
using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    [DisallowMultipleComponent]
    public abstract class BuildAgent : ExtendedMonoBehaviourRun
    {
        public string ExtraDescription => "<ExtraDescription not implemented>";
        public virtual bool RefundCost => false;

        [System.Obsolete("Use the other overload instead!")]
        public void Apply(int addingLevel, double buildValuePerLevel)
        {
            Debug.Log("Obsolete function! Use the other overload instead!");
        }

        public abstract void Apply(int currentLevel, int addingLevel, double buildValuePerLevel);

        public virtual bool Verify(double buildValue, int maxLevel)
        {
            ///
            return true;
        }

        public string GetDescriptionText(double buildValue)
        {
            return "<GetDescriptionText Not Implemented>";
        }

        public bool TryToReportAchievement()
        {
            Debug.LogError("TryToReportAchievement not implemented!");
            return false;
        }

        public bool TryToReportMaxedAchievement()
        {
            Debug.LogError("TryToReportMaxedAchievement not implemented!");
            return false;
        }
    }
}