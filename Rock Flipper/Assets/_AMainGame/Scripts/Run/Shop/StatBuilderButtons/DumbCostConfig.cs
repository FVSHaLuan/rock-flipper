using Agame.Balancing;
using OneLine;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class DumbCostConfig : MonoBehaviour, ICostConfig
    {
        [SerializeField]
        private Currency currency;
        [SerializeField, OneLineWithHeader]
        private TieredExponentialPrice amount;

        public Currency Currency => currency;
        public CurrencyAmount GetNextLevelCost(int level)
        {
            return new CurrencyAmount(currency, amount.GetPrice(level + 1));
        }
    }

}