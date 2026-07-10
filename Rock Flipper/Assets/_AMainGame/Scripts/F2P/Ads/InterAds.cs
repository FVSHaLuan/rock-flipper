using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;

namespace BT.F2P
{
    public static class InterAds
    {
        private const float MinAdsInterval = 60 * 5;

        private static IInterAdsImplementation interAdsImplementation;

        public delegate void InterAdsCallback(bool showed);

        private static float lastTimeClosedAnAds = -999;

        private static bool Enabled => !Entry.Instance.PlayerData.UnlockedPremium;

        static InterAds()
        {
            ///
            if (!VersionBranchInfo.IsF2P)
            {
                return;
            }

            ///
#if UNITY_EDITOR
            interAdsImplementation = new EditorInterAds();
#elif UNITY_ANDROID || UNITY_IOS
            interAdsImplementation = AppodealInterAds.Instance;
#else
            interAdsImplementation = new NoInterAds();
#endif
        }

        public static bool Show(InterAdsCallback callback)
        {
            ///
#if !BSB_F2P
            callback?.Invoke(false);

            ///
            return false;
#endif
            ///
            if (!Enabled || Time.realtimeSinceStartup - lastTimeClosedAnAds < MinAdsInterval)
            {
                ///
                callback?.Invoke(false);

                ///
                return false;
            }

            ///
            InterAdsCallback effCallback = (bool showed) =>
            {
                ///
                if (showed)
                {
                    lastTimeClosedAnAds = Time.realtimeSinceStartup;
                }

                ///
                callback?.Invoke(showed);
            };

            ///
            return interAdsImplementation.Show(effCallback);
        }
    }

}