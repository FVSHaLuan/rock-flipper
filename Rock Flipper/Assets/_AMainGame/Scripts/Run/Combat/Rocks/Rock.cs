using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public class Rock : ExtendedMonoBehaviourRun
    {
        public event System.Action OnStartedNewLife;
        public event System.Action OnHPChanged;

        [SerializeField]
        private RockTier rockTier = RockTier.P0;
        [SerializeField]
        private bool isPure = false;

        [Space]
        [SerializeField]
        private int baseHP = 5;

        [Header("New rock flipping")]
        [SerializeField]
        private float newRockFlippingDuration = 1f;
        [SerializeField]
        private float newRockFlippingHeight = 2f;

        [Header("Components")]
        [SerializeField]
        private RockPoolHandler rockPoolHandler;
        [SerializeField]
        private Flippable flippable;
        [SerializeField]
        private FlippableByPlayerCursor flippableByPlayerCursor;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onStartedFlipping;
        [SerializeField]
        private UnityEvent onFinishedFlipping;

        [Header("Delegations")]
        [SerializeField]
        private UnityEvent newLifeEffectDelegation;

        public RockTier Tier => rockTier;
        public bool IsPure => isPure;
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        public RockPoolHandler PoolHandler => rockPoolHandler;

        protected void Start()
        {
            flippable.OnStartedFlipping += Flippable_OnStartedFlipping;
            flippable.OnFinishedFlipping += Flippable_OnFinishedFlipping;
        }

        private void Flippable_OnFinishedFlipping()
        {
            CurrentHP--;
            if (CurrentHP <= 0)
            {
                ///
                EarnBreakingCash();

                ///
                BreakCurrentRockAndSpawnNewOne();
            }
            else
            {
                ///
                EarnLandingCash();

                ///
                UpdateLandingCooldown();

                ///
                onFinishedFlipping?.Invoke();
                OnHPChanged?.Invoke();
            }
        }

        private void UpdateLandingCooldown()
        {
            flippableByPlayerCursor.LastTimeLanded = Time.time;
            flippableByPlayerCursor.LandingCooldownTime = BuildStats.GetRockTierBuildStats(Tier).landingCooldown;
        }

        private void BreakCurrentRockAndSpawnNewOne()
        {
            var isNewRockPure = Random.value <= Tier.GetPurityChance();
            if (isNewRockPure && isPure)
            {
                StartNewLife(true);
            }
            else
            {
                PoolHandler.TryReturnToPoolAndDeactivate();
                var newRockPrototype = RunEntry.prototypeManager.GetRockPrototype(rockTier, isNewRockPure);
                RunEntry.rockInstanceManager.SpawnAsReplacement(newRockPrototype.rockPoolHandler, transform.position);
            }
        }

        private void EarnLandingCash()
        {
            var stats = BuildStats.GetRockTierBuildStats(rockTier);
            var amount = stats.landingCash * (isPure ? stats.purityCashMultiplier : 1);
            RunData.AddCurrency(Currency.CASH, amount);
        }

        private void EarnBreakingCash()
        {
            var stats = BuildStats.GetRockTierBuildStats(rockTier);
            var amount = stats.breakingCash * (isPure ? stats.purityCashMultiplier : 1);
            RunData.AddCurrency(Currency.CASH, amount);
        }

        private void Flippable_OnStartedFlipping()
        {
            onStartedFlipping?.Invoke();
        }

        [ContextMenu("Start New Life"), PlayModeOnly]
        public void StartNewLife(bool playNewLifeEffect)
        {
            MaxHP = baseHP;
            CurrentHP = MaxHP;

            ///
            UpdateLandingCooldown();

            ///
            gameObject.SetActive(true);

            ///
            OnStartedNewLife?.Invoke();

            ///
            if (playNewLifeEffect)
            {
                PlayNewLifeEffect();
            }
        }

        public void DoNewRockFlipping(Vector2 landingPosition)
        {
            flippable.ForceFlipping(newRockFlippingDuration, landingPosition, newRockFlippingHeight);
        }

        private void PlayNewLifeEffect()
        {
            newLifeEffectDelegation?.Invoke();
        }
    }

}