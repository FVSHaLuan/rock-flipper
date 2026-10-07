using FH.Core.Architecture.Pool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Agame.Run.Combat
{
    public class Rock : ExtendedMonoBehaviourRun
    {
        public event System.Action OnStartedNewLife;
        public event System.Action OnHPChanged;
        /// <summary>
        /// Fired when the rock lands and survives (not fired when the landing breaks it)
        /// </summary>
        public event System.Action OnLanded;

        [SerializeField]
        private RockTier rockTier = RockTier.P0;
        [SerializeField]
        private bool isPure = false;

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

        [Header("Effects")]
        [SerializeField]
        private Vector2 floatingTextOffset;
        [SerializeField]
        private float floatingTextRandomRadius = 0.3f;
        [SerializeField]
        private GeneralPoolMemberSimplifiedEffect normalLandingCashFloatingTextPrototype;
        [SerializeField]
        private GeneralPoolMemberSimplifiedEffect pureLandingCashFloatingTextPrototype;
        [SerializeField]
        private GeneralPoolMemberSimplifiedEffect normalBreakingCashFloatingTextPrototype;
        [SerializeField]
        private GeneralPoolMemberSimplifiedEffect pureBreakingCashFloatingTextPrototype;
        [SerializeField]
        private Vector2 criticalFloatingTextOffset;
        [SerializeField]
        private GeneralPoolMemberSimplifiedEffect criticalLandingFloatingTextPrototype;

        [Header("Effect delegations")]
        [SerializeField]
        private UnityEvent zappedEffectDelegate;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onStartedFlipping;
        [SerializeField, FormerlySerializedAs("onFinishedFlipping")]
        private UnityEvent onLanded;

        [Header("Delegations")]
        [SerializeField]
        private UnityEvent newLifeEffectDelegation;

        public RockTier Tier => rockTier;
        public bool IsPure => isPure;
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        public RockPoolHandler PoolHandler => rockPoolHandler;
        public Flippable Flippable => flippable;

        protected void OnDisable()
        {
            RunEntry.skillTreeScreen.OnClosed -= SkillTreeScreen_OnClosed;
        }

        protected void OnEnable()
        {
            RunEntry.skillTreeScreen.OnClosed += SkillTreeScreen_OnClosed;
        }

        protected void Start()
        {
            flippable.OnStartedFlipping += Flippable_OnStartedFlipping;
            flippable.OnFinishedFlipping += Flippable_OnFinishedFlipping;
        }

        private void SkillTreeScreen_OnClosed()
        {
            ApplyFromBuildStats();
        }

        private void ApplyFromBuildStats()
        {
            var stats = BuildStats.GetRockTierBuildStats(Tier);
            flippable.FlippingSpeedFactor = stats.flippingSpeedFactor;
            flippableByPlayerCursor.LandingCooldownTime = stats.landingCooldown;
        }

        private void Flippable_OnFinishedFlipping()
        {
            CurrentHP--;
            if (CurrentHP <= 0)
            {
                ///
                AddChestLevelExp();
                EarnCash(true);

                ///
                BreakCurrentRockAndSpawnNewOne();
            }
            else
            {
                ///
                AddLevelExp();
                EarnCash(false);

                ///
                UpdateLandingCooldown();

                ///
                onLanded?.Invoke();
                OnHPChanged?.Invoke();
                OnLanded?.Invoke();
            }
        }

        private void UpdateLandingCooldown()
        {
            flippableByPlayerCursor.LastTimeLanded = Time.time;
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
                RunEntry.rockInstanceManager.Despawn(this);
                var newRockPrototype = RunEntry.prototypeManager.GetRockPrototype(rockTier, isNewRockPure);
                RunEntry.rockInstanceManager.SpawnAsReplacement(newRockPrototype.rockPoolHandler, transform.position);
            }
        }

        private void EarnCash(bool isBreaking)
        {
            var stats = BuildStats.GetRockTierBuildStats(rockTier);
            var isCritical = Random.value < BuildStats.GetCriticalLandingChance(rockTier);
            var amount = stats.landingCash
                * (isBreaking ? stats.breakingCashMultiplier : 1)
                * (isPure ? stats.purityCashMultiplier : 1)
                * (isCritical ? BuildStats.GetCriticalCashMultiplier(rockTier) : 1);
            RunData.AddCurrency(Currency.CASH, amount);

            ///
            PlayFloatingTextEffect(amount, isBreaking, isCritical);
        }

        private void PlayFloatingTextEffect(double amount, bool isBreaking, bool isCritical)
        {
            var prototype = GetFloatingTextPrototype(isBreaking);
            var text = entry.currencyConfigManager.GetConfig(Currency.CASH).CurrencyName + amount.ToLargeNumberString();
            var position = (Vector2)transform.position + floatingTextOffset + Random.insideUnitCircle * floatingTextRandomRadius;
            RunEntry.floatingTextManager.Spawn(prototype, position, text);

            ///
            if (isCritical)
            {
                var criticalPosition = (Vector2)transform.position + criticalFloatingTextOffset + Random.insideUnitCircle * floatingTextRandomRadius;
                RunEntry.floatingTextManager.Spawn(criticalLandingFloatingTextPrototype, criticalPosition);
            }
        }

        private GeneralPoolMemberSimplifiedEffect GetFloatingTextPrototype(bool isBreaking)
        {
            if (isBreaking)
            {
                return IsPure ? pureBreakingCashFloatingTextPrototype : normalBreakingCashFloatingTextPrototype;
            }
            else
            {
                return IsPure ? pureLandingCashFloatingTextPrototype : normalLandingCashFloatingTextPrototype;
            }
        }

        private void Flippable_OnStartedFlipping()
        {
            onStartedFlipping?.Invoke();
        }

        [ContextMenu("Start New Life"), PlayModeOnly]
        public void StartNewLife(bool playNewLifeEffect)
        {
            ///
            var tierStats = BuildStats.GetRockTierBuildStats(rockTier);

            ///
            MaxHP = tierStats.maxHp;
            CurrentHP = MaxHP;

            ///
            UpdateLandingCooldown();

            ///
            gameObject.SetActive(true);

            ///
            ApplyFromBuildStats();

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
            flippable.ForceFlipping(newRockFlippingDuration, landingPosition, newRockFlippingHeight, FlipSource.NewRock);
        }

        private void PlayNewLifeEffect()
        {
            newLifeEffectDelegation?.Invoke();
        }

        private void AddChestLevelExp()
        {
            var stats = BuildStats.GetRockTierBuildStats(rockTier);
            RunData.AddChestLevelExp(stats.chestLevelExp);
        }

        private void AddLevelExp()
        {
            var stats = BuildStats.GetRockTierBuildStats(rockTier);
            RunData.AddLevelExp(stats.levelExp);
        }

        [ContextMenu("Play Zapped Effect"), PlayModeOnly]
        public void PlayZappedEffect()
        {
            zappedEffectDelegate?.Invoke();
        }
    }
}