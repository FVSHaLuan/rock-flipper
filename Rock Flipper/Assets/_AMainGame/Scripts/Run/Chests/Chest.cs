using UnityEngine;

namespace Agame.Run.Combat
{
    public class Chest : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private ChestRarity rarity;

        [Space]
        [SerializeField]
        private ChestPoolHandler chestPoolHandler;

        public ChestPoolHandler PoolHandler => chestPoolHandler;

        public ChestRarity Rarity => rarity;
    }

}