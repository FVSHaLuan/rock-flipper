using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.F2P
{
    public class NoRewardedAds : IRewardedAdsImplementation
    {
        public bool IsAvailable => false;

        public bool Show(RewardedAds.RewardedAdsCallback callback)
        {
            callback?.Invoke(false, false);

            ///
            return false;
        }
    }
}
