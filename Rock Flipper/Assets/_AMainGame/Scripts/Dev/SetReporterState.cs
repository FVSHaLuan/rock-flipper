using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;

public class SetReporterState : MonoBehaviour
{
    protected void OnEnable()
    {
        Set();
    }

    public static void Set()
    {
        if (VersionBranchInfo.IsTargetedOrOnMobile)
        {
            if (Reporter.Instance != null)
            {
                Reporter.Instance.enabled = Entry.Instance.GameSetting.enableReporterOnMobile;
            }
        }
    }
}
