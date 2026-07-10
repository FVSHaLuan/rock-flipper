using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Agame
{
    public class SettingPopupHelper : ExtendedMonoBehaviour
    {
        public void ShowSettingPopup()
        {
            entry.SettingPopup.ShowPopup();
        }

        public void ShowCustomDifficulty()
        {
            if (VersionBranchInfo.IsF2P && !playerData.UnlockedPremium)
            {
                ///
                entry.SettingPopup.PredefinedLastSelectableFlag = "Purchase Premium Button - CD";
            }
            else
            {
                ///
                entry.SettingPopup.PredefinedLastSelectableFlag = "Enemy HP Slider";
            }            

            ///
            entry.SettingPopup.ShowPopup();
        }
    }

}