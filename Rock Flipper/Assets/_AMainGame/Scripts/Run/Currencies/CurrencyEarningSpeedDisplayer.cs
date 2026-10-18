using Agame.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Agame.Run.Combat
{
    public class CurrencyEarningSpeedDisplayer : ValueDisplayerUnified<double>
    {
        [SerializeField]
        private Currency currency;

        private string currencyName;
        private string perSecondSuffix;

        protected override void OnEnable()
        {
            perSecondSuffix = LocalizedStrings.Instance.perSecondSuffix.GetLocalizedString();
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
            base.OnEnable();
        }

        private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

        private void OnLocaleChanged(Locale _)
        {
            perSecondSuffix = LocalizedStrings.Instance.perSecondSuffix.GetLocalizedString();
            ForceDisplay();
        }

        protected override double GetCurrentValue()
        {
            var speed = RunEntry.Instance.cashEarningTracker.EarningSpeed;
            return speed < 10 ? speed : System.Math.Round(speed);
        }

        protected override string GetString(double value)
        {
            if (currencyName == null)
            {
                currencyName = Entry.Instance.currencyConfigManager.GetConfig(currency).CurrencyName;
            }
            return $"{currencyName}{value.ToLargeNumberString()}{perSecondSuffix}";
        }
    }

}
