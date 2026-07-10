using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BT.FeatureBranching
{
    public class VersionBranchUtilities : IPreprocessBuildWithReport
    {
        public int callbackOrder => int.MinValue;

        public void OnPreprocessBuild(BuildReport report)
        {
            // Check output path
            if (VersionBranchInfo.IsPlaytestOrDemo || VersionBranchInfo.IsPlaytest)
            {
                if (report.summary.outputPath.ToLower().Contains("release"))
                {
                    EditorUtility.DisplayDialog("Wrong output path", "You are trying to build a playtest or demo version to a release folder. Please change the output path to a playtest or demo folder.", "OK");
                    throw new BuildFailedException("Wrong output path for playtest or demo build");
                }
            }
            else
            {
                if (!report.summary.outputPath.ToLower().Contains("release"))
                {
                    EditorUtility.DisplayDialog("Wrong output path", "You are trying to build a full version to a non-release folder. Please change the output path to a release folder.", "OK");
                    throw new BuildFailedException("Wrong output path for full build");
                }
            }

            ///
            string msg = string.Format("Version: {0}\r\nF2P: {1}\r\nOutput path: {2}", VersionBranchInfo.GetCurrentVersionDisplayName(), VersionBranchInfo.IsF2P, report.summary.outputPath);

            ///
            if (!EditorUtility.DisplayDialog("Build for version branch:", msg, "OK", "Cancel"))
            {
                throw new BuildFailedException("Cancelled! Wrong version branch or folder");
            }
        }

        [MenuItem("FH/Version Branching/Switch to: FULL", validate = true)]
        private static bool SwitchToFullValidator()
        {
            return VersionBranchInfo.Current != VersionBranch.Full;
        }

        [MenuItem("FH/Version Branching/Switch to: FULL")]
        private static void SwitchToFull()
        {
            ///
            SwitchToWithPrompt("Full", null);
        }

        [MenuItem("FH/Version Branching/Switch to: PLAYTEST", validate = true)]
        private static bool SwitchToPlaytestValidator()
        {
            return VersionBranchInfo.Current != VersionBranch.Playtest;
        }

        [MenuItem("FH/Version Branching/Switch to: PLAYTEST")]
        private static void SwitchToPlaytest()
        {
            SwitchTo(VersionBranchInfo.PlaytestDirective);
        }

        [MenuItem("FH/Version Branching/Switch to: DEMO", validate = true)]
        private static bool SwitchToDemoValidator()
        {
            return VersionBranchInfo.Current != VersionBranch.Demo || VersionBranchInfo.IsPrologue;
        }

        [MenuItem("FH/Version Branching/Switch to: DEMO")]
        private static void SwitchToDemo()
        {
            SwitchTo(VersionBranchInfo.DemoDirective);
        }

        [MenuItem("FH/Version Branching/Switch to: PROLOGUE", validate = true)]
        private static bool SwitchToPrologueValidator()
        {
            return VersionBranchInfo.Current != VersionBranch.Demo || !VersionBranchInfo.IsPrologue;
        }

        [MenuItem("FH/Version Branching/Switch to: PROLOGUE")]
        private static void SwitchToPrologue()
        {
            SwitchTo(VersionBranchInfo.DemoDirective, VersionBranchInfo.PrologueDirective);
        }

#if BSB_F2P

        [MenuItem("FH/Version Branching/Disable F2P")]
        private static void DisableF2P()
        {
            ///
            if (VersionBranchInfo.IsTargetedOrOnMobile)
            {
                if (EditorUtility.DisplayDialog("WARNING!!!!!!!", "You're disabling F2P for mobile target!", "Cancel", "Disable anyway"))
                {
                    Debug.Log("Cancelled");

                    ///
                    return;
                }
            }

            ///
            ToggleSymbol(VersionBranchInfo.F2PDirective, false);
        }
#else
        [MenuItem("FH/Version Branching/Enable F2P")]
        private static void EnableF2P()
        {
            ///
            if (!VersionBranchInfo.IsTargetedOrOnMobile)
            {
                if (EditorUtility.DisplayDialog("WARNING!!!!!!!", "You're enabling F2P for non-mobile target!", "Cancel", "Enable anyway"))
                {
                    Debug.Log("Cancelled");

                    ///
                    return;
                }
            }

            ///
            ToggleSymbol(VersionBranchInfo.F2PDirective, true);
        }
#endif

        private static void ToggleSymbol(string symbol, bool enabled)
        {
            ///
            if (!UnityEditor.EditorUtility.DisplayDialog(enabled ? "Enable" : "Disable", symbol, "OK", "Cancel"))
            {
                return;
            }

            ///
            var target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            ///
            PlayerSettings.GetScriptingDefineSymbols(target, out var defines);

            ///
            var defineList = new HashSet<string>(defines);

            ///
            if (enabled)
            {
                defineList.Add(symbol);
            }
            else
            {
                defineList.Remove(symbol);
            }

            ///
            PlayerSettings.SetScriptingDefineSymbols(target, defineList.ToArray());
        }

        private static void SwitchToWithPrompt(string branchName, params string[] symbols)
        {
            ///
            if (!UnityEditor.EditorUtility.DisplayDialog("Switch to:", branchName, "OK", "Cancel"))
            {
                return;
            }

            ///
            var target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            ///
            PlayerSettings.GetScriptingDefineSymbols(target, out var defines);

            ///
            var defineList = new HashSet<string>(defines);
            defineList.Remove(VersionBranchInfo.PlaytestDirective);
            defineList.Remove(VersionBranchInfo.DemoDirective);
            defineList.Remove(VersionBranchInfo.PrologueDirective);

            ///
            if (symbols != null)
            {
                foreach (var item in symbols)
                {
                    defineList.Add(item);
                }
            }

            ///
            PlayerSettings.SetScriptingDefineSymbols(target, defineList.ToArray());
        }

        private static void SwitchTo(params string[] symbols)
        {
            ///
            string prompt = "";
            foreach (string symbol in symbols)
            {
                if (prompt == "")
                {
                    prompt = symbol;
                }
                else
                {
                    prompt += " + " + symbol;
                }
            }

            ///
            SwitchToWithPrompt(prompt, symbols);
        }
    }

}