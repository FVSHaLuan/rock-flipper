using System;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class Chest : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private ChestRarity rarity;

        [Space]
        [SerializeField]
        private Flippable flippable;
        [SerializeField]
        private ChestPoolHandler chestPoolHandler;

        public ChestPoolHandler PoolHandler => chestPoolHandler;

        public ChestRarity Rarity => rarity;
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }

        protected void OnDisable()
        {
            RunEntry.chestInstanceManager.RemoveActiveChest(this);
        }

        protected void Start()
        {
            flippable.OnFinishedFlipping += Flippable_OnFinishedFlipping;
            flippable.OnStartedFlipping += Flippable_OnStartedFlipping;
        }

        private void Flippable_OnStartedFlipping()
        {

        }

        private void Flippable_OnFinishedFlipping()
        {
            CurrentHP--;
            if (CurrentHP <= 0)
            {
                Open();
            }
        }

        public void ApplyState(ChestState state)
        {
            if (state.rarity != rarity)
            {
                Debug.LogError($"Chest state rarity {state.rarity} does not match chest rarity {rarity}");
            }
        }

        public void StartNewLife()
        {
            var chestRarityBuildStats = BuildStats.GetChestRarityBuildStats(rarity);
            MaxHP = chestRarityBuildStats.maxHP;
            CurrentHP = MaxHP;
        }

        private void Open()
        {
            PoolHandler.TryReturnToPoolAndDeactivate();
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