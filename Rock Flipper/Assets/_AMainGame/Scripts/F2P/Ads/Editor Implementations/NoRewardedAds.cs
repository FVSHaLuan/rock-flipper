using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.F2P
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
