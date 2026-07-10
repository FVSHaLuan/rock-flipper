using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BT.FeatureBranching
{
    [RequireComponent(typeof(CanvasScaler))]
    public class MobileCanvasScalerModifier : MonoBehaviour
    {
        [SerializeField]
        private Vector2 mobileReferenceResolution;

        protected void Awake()
        {
            if (!VersionBranchInfo.IsTargetedOrOnMobile)
            {
                return;
            }

            ///
            GetComponent<CanvasScaler>().referenceResolution = mobileReferenceResolution;
        }

        private void Reset()
        {
            mobileReferenceResolution = GetComponent<CanvasScaler>().referenceResolution;
        }
    }

}