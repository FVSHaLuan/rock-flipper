using Agame.Run.Stats;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows a notice badge on the Background tab button while the background feature is enabled and either the
    /// player can unlock a background, or there is a Preferred background that isn't the active background.
    /// </summary>
    public class BackgroundTabButtonNoticeBadge : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject badge;
        [SerializeField, Tooltip("Decides whether the player can unlock a background, even while it's inactive.")]
        private UnlockBackgroundButton unlockBackgroundButton;

        private RunData subscribedRunData;
        private PreferredBackgroundMultiplier subscribedPreferredMultiplier;

        protected void OnEnable()
        {
            ///
            subscribedRunData = RunData;
            subscribedRunData.OnCurrencyValueModifiedThisFrame += RunData_OnCurrencyValueModifiedThisFrame;

            // Recalculated whenever a background is unlocked, or the active or Preferred background changes.
            subscribedPreferredMultiplier = RunEntry.preferredBackgroundMultiplier;
            subscribedPreferredMultiplier.OnChanged += PreferredMultiplier_OnChanged;

            BuildStatsObject.OnEnabledBackgrounds += BuildStatsObject_OnEnabledBackgrounds;

            ///
            UpdateView();
        }

        protected void OnDisable()
        {
            // RunData outlives this scene, so drop the handlers even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnCurrencyValueModifiedThisFrame -= RunData_OnCurrencyValueModifiedThisFrame;
                subscribedRunData = null;
            }
            if (subscribedPreferredMultiplier != null)
            {
                subscribedPreferredMultiplier.OnChanged -= PreferredMultiplier_OnChanged;
                subscribedPreferredMultiplier = null;
            }
            BuildStatsObject.OnEnabledBackgrounds -= BuildStatsObject_OnEnabledBackgrounds;
        }

        private void RunData_OnCurrencyValueModifiedThisFrame(Currency currency)
        {
            if (currency == UnlockBackgroundButton.CostCurrency)
            {
                UpdateView();
            }
        }

        private void PreferredMultiplier_OnChanged()
        {
            UpdateView();
        }

        private void BuildStatsObject_OnEnabledBackgrounds()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            ///
            if (!BuildStats.BackgroundsEnabled)
            {
                badge.SetActive(false);
                return;
            }

            ///
            var preferredMultiplier = subscribedPreferredMultiplier;
            var preferredNotSelected = preferredMultiplier.HasPreferredBackground
                && !preferredMultiplier.IsUsingPreferredBackground;
            badge.SetActive(preferredNotSelected || unlockBackgroundButton.CanUnlock);
        }
    }

}
