#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using UnityEngine;

namespace Agame.UI
{
    public class FeedbackPopupLauncher : ExtendedMonoBehaviour
    {
        private const string GoogleManualForm = "https://docs.google.com/forms/d/e/1FAIpQLSfvwRMMWQyRXHjPtV-Pkk05XihsCyx4n_8us1ImnsXVqwkb7g/viewform?usp=sf_link";

        public void Launch()
        {
            if (entry.inputManager.ActiveSimplifiedInputDevice.deviceType == SimplifiedInputDeviceType.MouseAndKeyboard
                || VersionBranchInfo.IsTargetedOrOnMobile)
            {
                CommonEntry.feedbackPopup.Show();
            }
            else
            {
                if (SteamManager.Initialized)
                {
#if !DISABLESTEAMWORKS
                    SteamFriends.ActivateGameOverlayToWebPage(GoogleManualForm, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Modal);
#endif
                }
                else
                {
                    Application.OpenURL(GoogleManualForm);
                }
            }
        }


    }
}
