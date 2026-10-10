using UnityEngine;

namespace Agame.Run.Combat
{
    public class CurrencyEarningSpeedDisplayer : ValueDisplayerUnified<double>
    {
        [SerializeField]
        private Currency currency;

        private string currencyName;

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
            return $"{currencyName}{value.ToLargeNumberString()}/s";
        }
    }

}