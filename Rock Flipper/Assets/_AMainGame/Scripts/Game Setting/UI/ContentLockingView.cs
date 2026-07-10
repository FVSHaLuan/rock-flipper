using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;

namespace BT.UI.GameSettings
{
    public class ContentLockingView : ExtendedMonoBehaviour
    {
        [SerializeField]
        private List<GameObject> locks;

        protected void OnEnable()
        {
            ///
            if (locks == null)
            {
                return;
            }

            ///
            var unlockedAllContent = VersionBranchInfo.Current != VersionBranch.Demo;

            ///
            foreach (var item in locks)
            {
                item.SetActive(!unlockedAllContent);
            }
        }
    }

}