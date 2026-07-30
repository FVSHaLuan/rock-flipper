using UnityEngine;

namespace Agame.Run.Shop
{
    public interface ICostConfig
    {
        public Currency Currency { get; }
        public CurrencyAmount GetNextLevelCost(int currentLevel);        
    }

}