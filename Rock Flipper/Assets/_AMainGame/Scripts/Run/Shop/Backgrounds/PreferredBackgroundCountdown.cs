using TMPro;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows the play time left until <see cref="PreferredBackgroundUpdater"/> changes the Preferred background.
    /// Shows <see cref="NoCountdownText"/> while there is no Preferred background (fewer than 2 backgrounds unlocked).
    /// </summary>
    public class PreferredBackgroundCountdown : ExtendedMonoBehaviourRun
    {
        private const string NoCountdownText = "--:--";

        [SerializeField]
        private PreferredBackgroundUpdater preferredBackgroundUpdater;
        [SerializeField]
        private TMP_Text countdownText;

        // Remaining whole seconds last displayed, so the text is only rebuilt when it changes.
        // -1 = no countdown shown, int.MinValue = nothing displayed yet.
        private int displayedSeconds = int.MinValue;

        protected void OnEnable()
        {
            displayedSeconds = int.MinValue;
            UpdateView();
        }

        protected void Update()
        {
            UpdateView();
        }

        private static string FormatSeconds(int totalSeconds)
        {
            ///
            var hours = totalSeconds / 3600;
            var minutes = totalSeconds / 60 % 60;
            var seconds = totalSeconds % 60;

            ///
            if (hours > 0)
            {
                return $"{hours}:{minutes:00}:{seconds:00}";
            }
            return $"{minutes}:{seconds:00}";
        }

        private void UpdateView()
        {
            ///
            var runData = RunData;
            var remainingSeconds = -1;
            if (runData.UnlockedBackgroundCount >= 2)
            {
                // Rounded up, so the countdown shows 0:00 only at the moment the Preferred background changes.
                var remaining = preferredBackgroundUpdater.PreferredInterval - runData.PreferredBackgroundTimeElapsed;
                remainingSeconds = Mathf.Max(0, Mathf.CeilToInt(remaining));
            }

            ///
            if (remainingSeconds == displayedSeconds)
            {
                return;
            }
            displayedSeconds = remainingSeconds;
            countdownText.SetText(remainingSeconds < 0 ? NoCountdownText : FormatSeconds(remainingSeconds));
        }
    }

}
