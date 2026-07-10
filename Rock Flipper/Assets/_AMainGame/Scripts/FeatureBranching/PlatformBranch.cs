using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.FeatureBranching
{
    public enum PlatformBranch
    {
        None = -1,
        All = 0,
        PC = 1,
        Mac = 2,
        Linux = 3,
        Web = 4,
        XBox = 5,
        MicrosoftStore = 6,
        Mobile = 7,
    }

    public static class PlatformBranchInfo
    {        
        public static PlatformBranch Current { get; private set; } =
#if UNITY_STANDALONE_WIN
            PlatformBranch.PC
#elif UNITY_STANDALONE_OSX
            PlatformBranch.Mac
#elif UNITY_STANDALONE_LINUX
            PlatformBranch.Linux
#elif UNITY_WEBGL
            PlatformBranch.Web
#elif UNITY_WSA
            PlatformBranch.MicrosoftStore
#elif UNITY_XBOXONE
            PlatformBranch.XBox
#elif UNITY_IOS || UNITY_ANDROID
            PlatformBranch.Mobile
#else
            PlatformBranch.None
#endif
            ;
    }
}