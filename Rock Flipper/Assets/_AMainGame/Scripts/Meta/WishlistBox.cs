using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;

namespace BT.Meta
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