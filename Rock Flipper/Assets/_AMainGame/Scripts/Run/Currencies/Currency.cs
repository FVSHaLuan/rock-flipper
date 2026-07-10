using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BT.Run
{
    public enum Currency
    {
        ///
        INVALID = -1,

        ///
        CASH = 0,
        BLANK_BALL = 1,
        RAW_PRESTIGE = 2,
        PRESTIGE = 3,
        BUCKET = 4,
        FREE = 5,
        AURA = 6,
    }

    public static class CurrencyExtensions
    {
        public static int ValidCount { get; private set; }

        private static List<Currency> validCurrencies = new List<Currency>();

        static CurrencyExtensions()
        {
            validCurrencies = new List<Currency>(System.Enum.GetValues(typeof(Currency)).Cast<Currency>());
            validCurrencies.Remove(Currency.INVALID);
            ValidCount = validCurrencies.Count;
        }

        public static Currency GetValidCurrency(int index)
        {
            return validCurrencies[index];
        }

        public static string CurrencyName(this Currency currency)
        {
            return Entry.Instance.currencyConfigManager.GetConfig(currency).CurrencyName;
        }
    }

}