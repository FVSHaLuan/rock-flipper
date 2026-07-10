using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Localization
{
    public static class LocalizedPeriodMark
    {
        public static char CurrentPeriodMark { get; private set; }

        static LocalizedPeriodMark()
        {
            UpdateCurrentPeriodMark();

            ///
            LocalizationManager.OnLocalizeEvent += LocalizationManager_OnLocalizeEvent;
        }

        private static void LocalizationManager_OnLocalizeEvent()
        {
            UpdateCurrentPeriodMark();
        }

        private static void UpdateCurrentPeriodMark()
        {
            CurrentPeriodMark = LocalizationManager.GetTranslation("PeriodMark")[0];
        }

        public static string AddLocalizedPeriod(this string str)
        {
            return str.AddPeriod(CurrentPeriodMark, true);
        }
    }

}