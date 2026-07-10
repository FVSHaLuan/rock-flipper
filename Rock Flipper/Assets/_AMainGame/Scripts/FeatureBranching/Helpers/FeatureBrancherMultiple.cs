using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.FeatureBranching
{
    public class FeatureBrancherMultiple : MonoBehaviour
    {
        [SerializeField]
        private List<VersionBranch> versions = new List<VersionBranch>();
        [SerializeField]
        private List<PlatformBranch> platforms = new List<PlatformBranch>();

        [Space]
        [SerializeField]
        private UnityEvent onMetConditions;
        [SerializeField]
        private UnityEvent onNotMetConditions;

        protected void OnEnable()
        {
            if (CheckVersions() && CheckPlatforms())
            {
                onMetConditions?.Invoke();
            }
            else
            {
                onNotMetConditions?.Invoke();
            }
        }

        private bool CheckVersions()
        {
            if (versions == null
                || versions.Count == 0
                || versions.Contains(VersionBranch.All))
            {
                return true;
            }

            ///
            return versions.Contains(VersionBranchInfo.Current);
        }

        private bool CheckPlatforms()
        {
            if (platforms == null
                || platforms.Count == 0
                || platforms.Contains(PlatformBranch.All))
            {
                return true;
            }

            ///
            return platforms.Contains(PlatformBranchInfo.Current);
        }
    }

}