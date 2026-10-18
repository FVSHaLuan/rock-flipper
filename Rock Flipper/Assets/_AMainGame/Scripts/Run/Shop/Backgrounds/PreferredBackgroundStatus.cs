using Agame.Localization;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows the status of the Preferred Multiplier: whether the Preferred feature is available yet, whether the
    /// player is using the Preferred background as the active background, and how to increase the multiplier.
    /// </summary>
    public class PreferredBackgroundStatus : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private UnifiedText statusText;

        private PreferredBackgroundMultiplier subscribedPreferredMultiplier;

        protected void OnEnable()
        {
            ///
            subscribedPreferredMultiplier = RunEntry.preferredBackgroundMultiplier;
            subscribedPreferredMultiplier.OnChanged += PreferredMultiplier_OnChanged;

            ///
            UpdateView();
        }

        protected void OnDisable()
        {
            if (subscribedPreferredMultiplier != null)
            {
                subscribedPreferredMultiplier.OnChanged -= PreferredMultiplier_OnChanged;
                subscribedPreferredMultiplier = null;
            }
        }

        private void PreferredMultiplier_OnChanged()
        {
            UpdateView();
        }

        private static string FormatMultiplier(double multiplier)
        {
            return $"x{multiplier:0.##}";
        }

        private void UpdateView()
        {
            ///
            var strings = SpecificLocalizedStrings.Instance;
            var preferredMultiplier = subscribedPreferredMultiplier;
            if (!preferredMultiplier.HasPreferredBackground)
            {
                statusText.SetText(strings.unlockMoreBackgrounds.GetLocalizedString());
                return;
            }

            ///
            var multiplier = FormatMultiplier(preferredMultiplier.PreferredMultiplier);
            if (!preferredMultiplier.IsUsingPreferredBackground)
            {
                statusText.SetText(strings.selectPreferredBackground.GetLocalizedString());
                return;
            }

            ///
            var text = string.Format(strings.preferredBackgroundCashIncome.GetLocalizedString(), $"<b>{multiplier}</b>");
            if (RunData.UnlockedBackgroundCount < GameConst.BackgroundCount)
            {
                text += $"\n{strings.unlockMoreBackgroundsToIncreaseIt.GetLocalizedString()}";
            }
            statusText.SetText(text);
        }
    }

}
