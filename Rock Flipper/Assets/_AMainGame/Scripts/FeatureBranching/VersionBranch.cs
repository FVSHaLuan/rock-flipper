// #define BETA_VERSION
using Agame.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.FeatureBranching
{
    public enum VersionBranch
    {
        All = 0,
        Full = 1,
        Demo = 2,
        Playtest = 3,
    }

    public static class VersionBranchInfo
    {
        public const string PrologueDirective = "BSB_VER_PROLOGUE";
        public const string DemoDirective = "BSB_VER_DEMO";
        public const string PlaytestDirective = "BSB_VER_PLAYTEST";
        public const string F2PDirective = "BSB_F2P";

        public static bool IsBetaVersion
        {
            get
            {
#if BETA_VERSION
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsF2P
        {
            get
            {
#if BSB_F2P
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsPrologue
        {
            get
            {
#if BSB_VER_PROLOGUE && BSB_VER_DEMO
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsTargetedOrOnMobile
        {
            get
            {
#if UNITY_ANDROID || UNITY_IOS
                return true;
#endif
                ///
                return Application.isMobilePlatform;
            }
        }

        public static bool IsPlaytestOrDemo
        {
            get
            {
                return Current == VersionBranch.Playtest || Current == VersionBranch.Demo;
            }
        }

        public static bool IsFullGame => Current == VersionBranch.Full;

        public static bool IsDemo => Current == VersionBranch.Demo;
        public static bool IsPlaytest => Current == VersionBranch.Playtest;

        public static VersionBranch Current
        {
            get
            {
#if BSB_VER_DEMO
                return VersionBranch.Demo;
#elif BSB_VER_PLAYTEST
                return VersionBranch.Playtest;
#else
                return VersionBranch.Full;
#endif
            }
        }

        public static string GetCurrentVersionDisplayName()
        {
            if (IsPrologue)
            {
                return "Prologue";
            }
            else
            {
                return Current.ToString();
            }
        }
    }
}