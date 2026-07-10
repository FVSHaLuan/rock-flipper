using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using UnityEngine;

namespace Agame.Meta
{
    public class WishlistBox : MonoBehaviour
    {
        protected void Start()
        {
            if (VersionBranchInfo.Current == VersionBranch.Full
                || Application.isMobilePlatform)
            {
                gameObject.SetActive(false);
            }
        }
    }

}