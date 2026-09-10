using Agame.Balancing;
using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class RockCountCostConfig : ExtendedMonoBehaviourRun, ICostConfig
    {
        [SerializeField]
        private RockTier rockTier;

        [Space]
        [SerializeField]
        private Currency currency;
        [SerializeField]
        private List<TieredExponentialPrice> prices;

        public Currency Currency => throw new System.NotImplementedException();

        public CurrencyAmount GetNextLevelCost(int currentLevel)
        {
            throw new System.NotImplementedException();
        }
    }
}