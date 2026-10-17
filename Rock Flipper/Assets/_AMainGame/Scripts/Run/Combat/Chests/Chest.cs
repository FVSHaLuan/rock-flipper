using Agame.Balancing;
using Agame.Run.Shop;
using System.Collections;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class Chest : ExtendedMonoBehaviourRun
    {
        public event System.Action OnStartedNewLife;
        public event System.Action OnHPChanged;
        /// <summary>
        /// When it lands with HP left. Its last landing opens it instead (<see cref="OnOpened"/>).
        /// </summary>
        public event System.Action OnLanded;
        public event System.Action OnOpened;

        [SerializeField]
        private ChestRarity rarity;

        [Header("New chest flipping")]
        [SerializeField]
        private float newChestFlippingDuration = 1f;
        [SerializeField]
        private float newChestFlippingHeight = 2f;

        [Space]
        [SerializeField]
        private Flippable flippable;
        [SerializeField]
        private ChestPoolHandler chestPoolHandler;
        [SerializeField]
        private ChestOpeningAnimation openingAnimation;

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
        /// <summary>
        /// Between <see cref="Open"/> and the end of the opening animation, after which the chest returns to the pool.
        /// </summary>
        public bool IsOpening { get; private set; }
        /// <summary>See <see cref="ChestState.uniqueItemId"/>.</summary>
        public string UniqueItemId => uniqueItemId;

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
            // Dropping from the chest bar is the chest's arrival, not a landing that costs HP.
            if (flippable.LastFlipSource == FlipSource.NewChest)
            {
                OnLanded?.Invoke();
                return;
            }

            ///
            CurrentHP--;

            ///
            if (CurrentHP <= 0)
            {
                Open();
            }
            else
            {
                OnHPChanged?.Invoke();
                OnLanded?.Invoke();
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
            if (IsOpening)
            {
                IsOpening = false;
                flippable.RemoveFlippingLock(this);
            }
            openingAnimation.ResetView();

            ///
            OnStartedNewLife?.Invoke();
        }

        public void DoNewChestFlipping(Vector2 landingPosition)
        {
            flippable.ForceFlipping(newChestFlippingDuration, landingPosition, newChestFlippingHeight, FlipSource.NewChest);
        }

        /// <summary>
        /// The item is found right away, so quitting mid-animation loses nothing; the animation only shows it,
        /// and the items UI shows the find once it lands (<see cref="FoundItemAnimation.FindItem"/>).
        /// </summary>
        private void Open()
        {
            ///
            IsOpening = true;
            flippable.AddFlippingLock(this);

            ///
            var find = DropItem();

            ///
            // Removed here rather than in OnDisable: unloading the scene also disables chests,
            // and the save made on unload must still include them.
            RunEntry.chestInstanceManager.RemoveActiveChest(this);

            ///
            OnOpened?.Invoke();
            StartCoroutine(PlayOpening(find));
        }

        private IEnumerator PlayOpening(FoundItemAnimation.UnrevealedFind find)
        {
            ///
            yield return openingAnimation.Play(() =>
            {
                if (find != null)
                {
                    RunEntry.foundItemAnimation.Play(find, openingAnimation.ItemOrigin);
                }
            });

            ///
            PoolHandler.TryReturnToPoolAndDeactivate();
        }

        /// <summary>
        /// Finds the item picked when this Unique chest dropped (<see cref="UniqueItemId"/>),
        /// or else a random item of this chest's rarity that can drop right now, and returns its find.
        /// Returns null if no item can drop.
        /// </summary>
        private FoundItemAnimation.UnrevealedFind DropItem()
        {
            var item = string.IsNullOrEmpty(uniqueItemId)
                ? RunEntry.itemManager.GetRandomItem(rarity, RunData)
                : RunEntry.itemManager.GetItemById(uniqueItemId);
            if (item == null)
            {
                Debug.LogWarning($"Chest {name} found no item of rarity {rarity} to drop.", this);
                return null;
            }
            var exp = GameBalance.GetItemExpPerFind(rarity);
            return RunEntry.foundItemAnimation.FindItem(item, exp);
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
        protected void Reset()
        {
            openingAnimation = GetComponent<ChestOpeningAnimation>();
        }

        [ContextMenu("Spawn This Rarity"), PlayModeOnly]
        private void Editor_SpawnThisRarity()
        {
            FindAnyObjectByType<NewChestSpawner>().SpawnChest(rarity, force: true);
        }
#endif
    }

}