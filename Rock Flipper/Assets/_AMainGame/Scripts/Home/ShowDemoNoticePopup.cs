using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using UnityEngine;

namespace Agame.Home
{
    public class ShowDemoNoticePopup : ExtendedMonoBehaviourHome
    {
        private static bool showed;

        [SerializeField]
        private UIScreen noticePopup;

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            if (!VersionBranchInfo.IsDemo && !VersionBranchInfo.IsPlaytest)
            {
                return;
            }

            ///
            if (showed)
            {
                return;
            }

            ///
            if (HomeEntry.mainUIScreen.IsScreenActive && !HomeEntry.mainUIScreen.IsBecomingActive)
            {
                showed = true;
                noticePopup.ShowPopup();
            }
            else
            {
                HomeEntry.mainUIScreen.OnBecomeActive += MainUIScreen_OnBecomeActive;
            }
        }

        private void MainUIScreen_OnBecomeActive()
        {
            showed = true;
            noticePopup.ShowPopup();

            ///
            HomeEntry.mainUIScreen.OnBecomeActive -= MainUIScreen_OnBecomeActive;
        }
    }

}