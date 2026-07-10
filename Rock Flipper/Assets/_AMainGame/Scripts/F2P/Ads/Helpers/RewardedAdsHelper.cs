using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.F2P
{
    public class RewardedAdsHelper : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onComplete;
        [SerializeField]
        private UnityEvent onNotComplete;

        public void TryShow()
        {
            RewardedAds.Show(RewardedAdsCallback);
        }

        private void RewardedAdsCallback(bool showed, bool completed)
        {
            if (completed)
            {
                onComplete?.Invoke();
            }
            else
            {
                onNotComplete?.Invoke();
            }
        }
    }

}