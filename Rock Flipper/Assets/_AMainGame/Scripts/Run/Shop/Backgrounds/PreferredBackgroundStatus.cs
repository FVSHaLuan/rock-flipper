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
            var preferredMultiplier = subscribedPreferredMultiplier;
            if (!preferredMultiplier.HasPreferredBackground)
            {
                statusText.SetText("Unlock more backgrounds to unlock the Preferred background.");
                return;
            }

            ///
            var multiplier = FormatMultiplier(preferredMultiplier.PreferredMultiplier);
            if (!preferredMultiplier.IsUsingPreferredBackground)
            {
                statusText.SetText($"Select the Preferred background to get <b>{multiplier}</b> cash income.");
                return;
            }

            ///
            var text = $"Preferred background: <b>{multiplier}</b> cash income.";
            if (RunData.UnlockedBackgroundCount < GameConst.BackgroundCount)
            {
                text += "\nUnlock more backgrounds to increase it.";
            }
            statusText.SetText(text);
        }
    }

}
