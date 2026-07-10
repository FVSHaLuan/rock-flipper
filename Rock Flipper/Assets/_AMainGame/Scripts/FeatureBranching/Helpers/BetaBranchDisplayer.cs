using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.FeatureBranching
{
    public class BetaBranchDisplayer : MonoBehaviour
    {
        protected void OnEnable()
        {
            if (!VersionBranchInfo.IsBetaVersion)
            {
                gameObject.SetActive(false);
            }
        }
    }

}