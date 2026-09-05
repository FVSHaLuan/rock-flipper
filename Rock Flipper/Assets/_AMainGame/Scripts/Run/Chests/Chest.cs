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

#if UNITY_EDITOR
        [ContextMenu("Spawn This Rarity"), PlayModeOnly]
        private void Editor_SpawnThisRarity()
        {
            RunEntry.Instance.chestInstanceManager.Spawn(rarity);
        }
#endif
    }

}