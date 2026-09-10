using UnityEngine;

namespace Agame.Balancing
{
    [System.Serializable]
    public struct TieredExponentialPrice
    {
        [Tooltip("price = a + b * cashTierBase * c^level")]
        public CashTier cashTier;
        [Tooltip("price = a + b * cashTierBase * c^level")]
        public double a;
        [Tooltip("price = a + b * cashTierBase * c^level")]
        public double b;
        [Tooltip("price = a + b * cashTierBase * c^level")]
        public double c;

        public double GetPrice(int level)
        {
            var cashTierConfig = Entry.Instance.cashTiers.GetConfig(cashTier);
            var cashTierBase = cashTierConfig.cashBase;
            return a + b * cashTierBase * System.Math.Pow(c, level);
        }
    }

}