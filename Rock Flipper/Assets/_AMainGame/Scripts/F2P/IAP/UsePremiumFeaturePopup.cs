using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BT.F2P
{
    [RequireComponent(typeof(UIScreen))]
    public class UsePremiumFeaturePopup : MonoBehaviour
    {
        public delegate void ResultCallback(Result result);

        [SerializeField]
        private Button watchAdsButton;
        [SerializeField]
        private Button purchasePremiumButton;

        private ResultCallback resultCallback;

        public enum Result
        {
            Canceled,
            InitiatedPremiumPurchase,
            WatchedAds,
        }

        public void Show(ResultCallback callback)
        {
            resultCallback = callback;

            ///
            watchAdsButton.gameObject.SetActive(true);
            purchasePremiumButton.interactable = true;

            ///
            UpdateWatchAdsButton();
            gameObject.SetActive(true);
        }

        public void Cancel()
        {
            gameObject.SetActive(false);
            resultCallback?.Invoke(Result.Canceled);
        }

        public void HandleClickedWatchAdsButton()
        {
            ///
            watchAdsButton.gameObject.SetActive(false);

            ///
            RewardedAds.Show(RewardedAdsCallback);
        }

        private void RewardedAdsCallback(bool showed, bool completed)
        {
            ///
            watchAdsButton.gameObject.SetActive(true);

            ///
            if (completed)
            {
                ///
                gameObject.SetActive(false);

                ///
                resultCallback?.Invoke(Result.WatchedAds);
            }
        }

        public void HandleInitiatedPremiumPurchase()
        {
            gameObject.SetActive(false);
            resultCallback?.Invoke(Result.InitiatedPremiumPurchase);
        }

        private void UpdateWatchAdsButton()
        {
            watchAdsButton.interactable = RewardedAds.IsAvailable;
        }

        protected void Update()
        {
            UpdateWatchAdsButton();
        }
    }

}