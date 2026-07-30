using OneLine;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class DumbCostConfig : MonoBehaviour, ICostConfig
    {
        [SerializeField, OneLine]
        private CurrencyAmount cost;

        public CurrencyAmount GetCost(int level)
        {
            return cost;
        }
    }

}