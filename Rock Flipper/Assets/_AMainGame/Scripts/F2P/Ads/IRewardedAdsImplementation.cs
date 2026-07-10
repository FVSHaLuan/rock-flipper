using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BT.F2P.RewardedAds;

namespace BT.F2P
{
    public interface IRewardedAdsImplementation
    {
        public bool IsAvailable { get; }
        public bool Show(RewardedAdsCallback callback);
    }

}