using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using UnityEngine;

namespace Agame.UI.GameSettings
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