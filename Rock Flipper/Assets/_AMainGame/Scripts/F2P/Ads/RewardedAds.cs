using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;

namespace BT.F2P
{
    public static class RewardedAds
    {
        public delegate void RewardedAdsCallback(bool showed, bool completed);

        private static IRewardedAdsImplementation rewardedAdsImplementation;

        public static bool Enabled
        {
            get
            {
                return (VersionBranchInfo.IsF2P
                        && (!Entry.Instance.PlayerData.UnlockedPremium || Entry.Instance.GameSetting.enabledRewardedAds)
                        );
            }
        }

        public static bool IsAvailable
        {
            get
            {
                ///
                if (!Enabled)
                {
                    return false;
                }

                ///
                return Entry.Instance.PlayerData.UnlockedPremium || rewardedAdsImplementation.IsAvailable;
            }
        }

        static RewardedAds()
        {
            ///
            if (!VersionBranchInfo.IsF2P)
            {
                return;
            }

            ///
#if UNITY_EDITOR
            rewardedAdsImplementation = new EditorRewardedAds();
#elif UNITY_ANDROID || UNITY_IOS
            rewardedAdsImplementation = AppodealRewardedAds.Instance;
#else
            rewardedAdsImplementation = new NoRewardedAds();
#endif
        }

        public static bool Show(RewardedAdsCallback callback)
        {
            if (!VersionBranchInfo.IsF2P)
            {
                callback?.Invoke(false, false);

                ///
                return false;
            }

            ///
            if (!IsAvailable)
            {
                callback?.Invoke(false, false);
                return false;
            }

            ///
            if (Entry.Instance.PlayerData.UnlockedPremium)
            {
                callback?.Invoke(true, true);

                ///
                return true;
            }

            ///
            return rewardedAdsImplementation.Show(callback);
        }
    }

}