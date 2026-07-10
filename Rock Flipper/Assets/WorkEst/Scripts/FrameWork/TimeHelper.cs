using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorkEst
{
    public static class TimeHelper
    {
        public static string ToWorkTimeString(this float time)
        {
            const float MonthDayCount = 30;

            ///
            if (time < MonthDayCount)
            {
                return string.Format("{0:0.00} Day", time);
            }
            else
            {
                return string.Format("{0:0.00} Month", time / MonthDayCount);
            }
        }
    }
}