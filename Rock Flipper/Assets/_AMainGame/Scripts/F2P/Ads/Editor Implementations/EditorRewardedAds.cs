using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Agame.F2P
{
    public class EditorRewardedAds : IRewardedAdsImplementation
    {
        public bool IsAvailable => true;

        public bool Show(RewardedAds.RewardedAdsCallback callback)
        {
#if UNITY_EDITOR
            var rs = EditorUtility.DisplayDialog("Rewarded ads", "Ads!", "Complete", "Cancel");
            callback?.Invoke(true, rs);

            ///
            return true;
#else
            TellADev.That("Do NOT use editor rewarded ads in builds");
            throw new System.NotImplementedException();
#endif
        }
    }

}