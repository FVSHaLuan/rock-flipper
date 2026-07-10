using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.FeatureBranching
{
    public class MobileObjectScaler : MonoBehaviour
    {
        [SerializeField]
        private Vector3 mobileLocalScale = Vector3.one;

        protected void Start()
        {
            if (VersionBranchInfo.IsTargetedOrOnMobile)
            {
                transform.localScale = mobileLocalScale;
            }
        }
    }
}
