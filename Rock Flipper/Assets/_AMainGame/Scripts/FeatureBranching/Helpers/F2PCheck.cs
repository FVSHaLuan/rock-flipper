using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.FeatureBranching
{
    public class F2PCheck : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onF2P;
        [SerializeField]
        private UnityEvent onNotF2P;

        protected void OnEnable()
        {
            if (VersionBranchInfo.IsF2P)
            {
                onF2P?.Invoke();
            }
            else
            {
                onNotF2P?.Invoke();
            }
        }
    }

}