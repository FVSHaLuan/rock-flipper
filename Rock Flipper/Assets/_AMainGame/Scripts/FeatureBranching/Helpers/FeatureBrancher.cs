using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.FeatureBranching
{
    public abstract class FeatureBrancher : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private VersionBranch versionBranch;
        [SerializeField]
        private PlatformBranch platformBranch;

        protected bool ShouldBeActive()
        {
            ///
            if (versionBranch != VersionBranchInfo.Current && versionBranch != VersionBranch.All)
            {
                return false;
            }

            ///
            if (platformBranch != PlatformBranchInfo.Current && platformBranch != PlatformBranch.All)
            {
                return false;
            }

            ///
            return true;
        }
    }

}