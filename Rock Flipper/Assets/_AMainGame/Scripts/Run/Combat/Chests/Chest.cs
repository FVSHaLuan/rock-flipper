using System;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class Chest : ExtendedMonoBehaviourRun
    {
        public event System.Action OnStartedNewLife;
        public event System.Action OnHPChanged;

        [SerializeField]
        private ChestRarity rarity;

        [Space]
        [SerializeField]
        private Flippable flippable;
        [SerializeField]
        private ChestPoolHandler chestPoolHandler;

        private FlippableByPlayerCursor flippableByPlayerCursor;
        private string uniqueItemId;
        private int uniqueBackgroundId;

        public ChestPoolHandler PoolHandler => chestPoolHandler;
        public Flippable Flippable => flippable;
        public FlippableByPlayerCursor FlippableByPlayerCursor
        {
            get
            {
                if (flippableByPlayerCursor == null)
                {
                    flippableByPlayerCursor = flippable.GetComponent<FlippableByPlayerCursor>();
                }
                return flippableByPlayerCursor;
            }
        }

        public ChestRarity Rarity => rarity;
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        /// <summary>See <see cref="ChestState.uniqueItemId"/>.</summary>
        public string UniqueItemId => uniqueItemId;

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

            ///
            if (CurrentHP <= 0)
            {
                Open();
            }
            else
            {
                OnHPChanged?.Invoke();
            }
        }

        public void ApplyState(ChestState state)
        {
            if (state.rarity != rarity)
            {
                Debug.LogError($"Chest state rarity {state.rarity} does not match chest rarity {rarity}");
            }

            ///
            uniqueItemId = state.uniqueItemId;
            uniqueBackgroundId = state.uniqueBackgroundId;
        }

        public void StartNewLife()
        {
            var chestRarityBuildStats = BuildStats.GetChestRarityBuildStats(rarity);
            MaxHP = chestRarityBuildStats.maxHP;
            CurrentHP = MaxHP;

            ///
            OnStartedNewLife?.Invoke();
        }

        private void Open()
        {
            DropItem();

            ///
            PoolHandler.TryReturnToPoolAndDeactivate();
        }

        /// <summary>
        /// Finds the item picked when this Unique chest dropped (<see cref="UniqueItemId"/>),
        /// or else a random item of this chest's rarity that can drop right now.
        /// </summary>
        private void DropItem()
        {
            var item = string.IsNullOrEmpty(uniqueItemId)
                ? RunEntry.itemManager.GetRandomItem(rarity, RunData)
                : RunEntry.itemManager.GetItemById(uniqueItemId);
            if (item == null)
            {
                Debug.LogWarning($"Chest {name} found no item of rarity {rarity} to drop.", this);
                return;
            }
            var exp = gameBalance.GetItemExpPerFind(rarity);
            RunData.AddItemExp(item.ItemId, exp);
        }

        public ChestState GetState()
        {
            return new ChestState
            {
                rarity = rarity,
                uniqueItemId = uniqueItemId,
                uniqueBackgroundId = uniqueBackgroundId,
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