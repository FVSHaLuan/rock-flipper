using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using Agame.Home;
using UnityEngine;

namespace Agame.F2P
{
    public class MobileRatingRequestPopupShower : ExtendedMonoBehaviourHome
    {
        private const string PrefKey = "MobileRatingThreshold";

        [SerializeField]
        private UIScreen requestPopup;
        [SerializeField]
        private List<int> thresholds = new List<int>();

        protected IEnumerator Start()
        {
            ///
            if (!VersionBranchInfo.IsF2P)
            {
                yield break;
            }

            ///
            while (!HomeEntry.mainUIScreen.IsScreenActive)
            {
                yield return null;
            }

            ///
            int thresholdId = PlayerPrefs.GetInt(PrefKey, 0);

            ///
            if (thresholdId >= thresholds.Count)
            {
                yield break;
            }

            ///
            var threshold = thresholds[thresholdId];
            var value = 0; // playerData.FinishedRunCount

            ///
            if (value < threshold)
            {
                yield break;
            }

            ///
            int nextThresholdId = thresholdId + 1;
            for (int i = thresholdId; i < thresholds.Count; i++)
            {
                if (value < thresholds[i])
                {
                    nextThresholdId = i;

                    ///
                    break;
                }
            }

            ///
            PlayerPrefs.SetInt(PrefKey, nextThresholdId);

            ///
            requestPopup.ShowPopup();
        }

        public void SetAsRated()
        {
            ///
            PlayerPrefs.SetInt(PrefKey, 999);
        }
    }
}
