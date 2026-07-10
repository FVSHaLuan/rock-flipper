using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Agame.F2P.RewardedAds;

namespace Agame.F2P
{
    public interface IRewardedAdsImplementation
    {
        public bool IsAvailable { get; }
        public bool Show(RewardedAdsCallback callback);
    }

}