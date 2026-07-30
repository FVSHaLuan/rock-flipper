using UnityEngine;

namespace Agame.Run.Shop
{
    public interface ICostConfig
    {
        public CurrencyAmount GetCost(int level);
    }

}