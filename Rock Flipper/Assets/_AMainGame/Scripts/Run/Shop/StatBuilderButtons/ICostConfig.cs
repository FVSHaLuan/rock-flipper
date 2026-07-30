using UnityEngine;

namespace Agame.Run.Shop
{
    public interface ICostConfig
    {
        public CurrencyAmount GetNextLevelCost(int currentLevel);
    }

}