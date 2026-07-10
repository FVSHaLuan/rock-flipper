using UnityEngine;

namespace BT.Run
{
    [System.Serializable]
    public struct CurrencyAmount
    {
        public Currency currency;
        public double amount;

        public CurrencyAmount(Currency currency, double amount)
        {
            this.currency = currency;
            this.amount = amount;
        }

        public string GetFormattedString()
        {
            return Entry.Instance.currencyConfigManager.GetConfig(currency).CurrencyName + amount.ToLargeNumberString();
        }
    }

}