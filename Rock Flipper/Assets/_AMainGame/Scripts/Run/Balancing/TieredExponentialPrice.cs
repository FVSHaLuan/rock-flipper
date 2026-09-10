using UnityEngine;

namespace Agame.Balancing
{
    public struct TieredExponentialPrice
    {
        [Tooltip("price = a + b * cashTierBase^level")]
        public CashTier cashTier;
        public double a;
        public double b;

        public double GetPrice(int level)
        {
            var cashTierConfig = Entry.Instance.cashTiers.GetConfig(cashTier);
            var cashTierBase = cashTierConfig.cashBase;
            return a + b * System.Math.Pow(cashTierBase, level);
        }
    }

}