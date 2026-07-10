#define GOOGLE_REVIEW

#if GOOGLE_REVIEW && UNITY_ANDROID
using Google.Play.Review; 
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.F2P
{
    public class MobileRatingLauncher : ExtendedMonoBehaviour
    {
        public void Show()
        {
#if UNITY_IOS
            UnityEngine.iOS.Device.RequestStoreReview();
#elif UNITY_ANDROID
#if GOOGLE_REVIEW
            entry.StartCoroutine(ShowAndroidReview()); 
#endif
#endif
        }

#if UNITY_ANDROID && GOOGLE_REVIEW
        private IEnumerator ShowAndroidReview()
        {
            var reviewManager = new ReviewManager();

            ///
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            var _playReviewInfo = requestFlowOperation.GetResult();

            ///
            var launchFlowOperation = reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            // The flow has finished. The API does not indicate whether the user
            // reviewed or not, or even whether the review dialog was shown. Thus, no
            // matter the result, we continue our app flow.
        }
#endif
    }
}