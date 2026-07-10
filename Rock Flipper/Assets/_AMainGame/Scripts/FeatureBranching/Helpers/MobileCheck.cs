using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.FeatureBranching
{
    public class MobileCheck : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onMobile;
        [SerializeField]
        private UnityEvent onNotMobile;

        protected void Awake()
        {
            if (VersionBranchInfo.IsTargetedOrOnMobile)
            {
                onMobile?.Invoke();
            }
            else
            {
                onNotMobile?.Invoke();
            }
        }
    }
}