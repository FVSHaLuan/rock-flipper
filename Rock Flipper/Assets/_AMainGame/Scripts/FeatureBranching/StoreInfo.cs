#if UNITY_STANDALONE && !UNITY_STANDALONE_LINUX
// #define ENABLE_GOG
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.FeatureBranching
{
    public enum Store
    {
        Unknown = 0,
        Steam = 1,
        GOG = 2,
        MicrosoftStore = 3,
        AppleAppStore = 4,
        GooglePlay = 5,
    }

    public static class StoreInfo
    {
        public static Store CurrentStore
        {
            get
            {
#if !DISABLESTEAMWORKS
                if (SteamManager.Initialized)
                {
                    return Store.Steam;
                }
#endif
#if ENABLE_GOG
                if (GogGalaxyManager.IsInitialized())
                {
                    return Store.GOG;
                }
#endif

                ///
                return Store.Unknown;
            }
        }
    }

}