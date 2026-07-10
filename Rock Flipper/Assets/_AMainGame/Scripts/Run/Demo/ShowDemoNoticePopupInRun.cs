using BT.FeatureBranching;
using System.Collections;
using UnityEngine;

namespace BT.Run
{
    public class ShowDemoNoticePopupInRun : ExtendedMonoBehaviourRun
    {
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
            if (RunEntry.combatScreen.IsScreenActive && !RunEntry.combatScreen.IsBecomingActive)
            {
                StartCoroutine(ShowDelay());
            }
            else
            {
                RunEntry.combatScreen.OnBecomeActive += CombatScreen_OnBecomeActive;
            }
        }

        private void CombatScreen_OnBecomeActive()
        {
            StartCoroutine(ShowDelay());

            ///
            RunEntry.combatScreen.OnBecomeActive -= CombatScreen_OnBecomeActive;
        }

        private IEnumerator ShowDelay()
        {
            yield return new WaitForEndOfFrame();
            noticePopup.ShowPopup();
        }
    }

}