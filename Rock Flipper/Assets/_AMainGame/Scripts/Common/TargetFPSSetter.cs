using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    public class TargetFPSSetter : ExtendedMonoBehaviour
    {
        protected void Start()
        {
            ///
            UpdateTargetFPS();

            ///
            gameSetting.OnTargetFPSChanged += GameSetting_OnTargetFPSChanged;
        }

        private void GameSetting_OnTargetFPSChanged()
        {
            UpdateTargetFPS();
        }

        private void UpdateTargetFPS()
        {
            Application.targetFrameRate = gameSetting.TargetFPS;
        }
    }

}