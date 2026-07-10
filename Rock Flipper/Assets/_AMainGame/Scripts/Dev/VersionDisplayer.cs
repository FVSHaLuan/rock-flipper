using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;

public class VersionDisplayer : MonoBehaviour
{
    private static Color FullVersionColor => new Color(141 / 255.0f, 255 / 255.0f, 221 / 255.0f);
    private static Color BetaVersionColor => new Color(85 / 255.0f, 108 / 255.0f, 212 / 255.0f);
    private static Color DemoVersionColor => new Color(176 / 255.0f, 0 / 255.0f, 255 / 255.0f);
    private static Color PlaytestVersionColor => new Color(226 / 255.0f, 158 / 255.0f, 0 / 255.0f);
    private static Color PrologueVersionColor => new Color(164 / 255.0f, 255 / 255.0f, 0 / 255.0f);

    [SerializeField]
    private UnifiedText text;

    protected void Start()
    {
        ///
        if (Entry.Instance.GameSetting.hideVersionInfo)
        {
            text.EnabledRenderer = false;
            return;
        }

        ///
        text.EnabledRenderer = true;

        ///
        switch (VersionBranchInfo.Current)
        {
            case VersionBranch.Demo:
                if (!VersionBranchInfo.IsPrologue)
                {
                    text.Text = "Demo - " + Application.version;
                    text.Color = DemoVersionColor;
                }
                else
                {
                    text.Text = "Prologue - " + Application.version;
                    text.Color = PrologueVersionColor;
                }
                break;
            case VersionBranch.Playtest:
                text.Text = "Playtest - " + Application.version;
                text.Color = PlaytestVersionColor;
                break;
            case VersionBranch.Full:
                if (VersionBranchInfo.IsBetaVersion)
                {
                    text.Text = Application.version;
                    text.Color = BetaVersionColor;
                }
                else
                {
                    text.Text = (VersionBranchInfo.IsTargetedOrOnMobile ? "PR - " : "Pre-release - ") + Application.version;
                    text.Color = FullVersionColor;
                }
                ///
                break;
            default:
                text.Text = "<Unknown> - " + Application.version;
                text.Color = PlaytestVersionColor;
                break;
        }
    }
}
