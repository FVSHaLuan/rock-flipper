using System;
using UnityEngine;

namespace BT.Marketing
{
    public class CountdownBox : MonoBehaviour
    {
        [SerializeField]
        private int targetYear = 0;
        [SerializeField]
        private int targetMonth = 0;
        [SerializeField]
        private int targetDay = 0;
        [SerializeField]
        private int targetHour = 0;
        [SerializeField]
        private int targetMin = 0;

        [Space]
        [SerializeField]
        private UnifiedText dayText;
        [SerializeField]
        private UnifiedText hourText;
        [SerializeField]
        private UnifiedText minText;
        [SerializeField]
        private UnifiedText secText;

        [Space]
        [SerializeField, Multiline(2)]
        private string dayStringFormat;
        [SerializeField, Multiline(2)]
        private string hourStringFormat;
        [SerializeField, Multiline(2)]
        private string minStringFormat;
        [SerializeField, Multiline(2)]
        private string secStringFormat;

        private TimeSpan lastDisplayedTimeSpan;
        private DateTime targetDateTime;

        protected void Awake()
        {
            targetDateTime = new DateTime(targetYear, targetMonth, targetDay, targetHour, targetMin, 0, DateTimeKind.Utc);
            targetDateTime = TimeZone.CurrentTimeZone.ToLocalTime(targetDateTime);
        }

        protected void OnEnable()
        {
            UpdateView(true);
        }

        protected void Update()
        {
            UpdateView(false);
        }

        private void UpdateView(bool forced)
        {
            var timeRemained = targetDateTime - System.DateTime.Now;
            if (timeRemained.TotalMilliseconds <= 0)
            {
                timeRemained = new TimeSpan(0);
            }

            ///
            if (forced || timeRemained.Days != lastDisplayedTimeSpan.Days)
            {
                dayText.Text = string.Format(dayStringFormat, timeRemained.Days);
            }
            if (forced || timeRemained.Hours != lastDisplayedTimeSpan.Hours)
            {
                hourText.Text = string.Format(hourStringFormat, timeRemained.Hours);
            }
            if (forced || timeRemained.Minutes != lastDisplayedTimeSpan.Minutes)
            {
                minText.Text = string.Format(minStringFormat, timeRemained.Minutes);
            }
            if (forced || timeRemained.Seconds != lastDisplayedTimeSpan.Seconds)
            {
                secText.Text = string.Format(secStringFormat, timeRemained.Seconds);
            }

            ///
            lastDisplayedTimeSpan = timeRemained;
        }
    }

}