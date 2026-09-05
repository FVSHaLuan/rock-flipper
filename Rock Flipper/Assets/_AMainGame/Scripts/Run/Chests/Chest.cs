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

        public void ApplyState(ChestState state)
        {
            if (state.rarity != rarity)
            {
                Debug.LogError($"Chest state rarity {state.rarity} does not match chest rarity {rarity}");
            }
        }

        public ChestState GetState()
        {
            return new ChestState
            {
                rarity = rarity
            };
        }

#if UNITY_EDITOR
        [ContextMenu("Spawn This Rarity"), PlayModeOnly]
        private void Editor_SpawnThisRarity()
        {
            RunEntry.Instance.chestInstanceManager.Spawn(rarity);
        }
#endif
    }

}