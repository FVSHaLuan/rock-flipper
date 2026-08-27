using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public class Rock : MonoBehaviour
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
                var isNewRockPure = Random.value <= Tier.GetPurityChance();
                if (isNewRockPure && isPure)
                {
                    StartNewLife(true);
                }
                else
                {
                    PoolHandler.TryReturnToPoolAndDeactivate();
                    RunEntry.Instance.rockInstanceManager.SpawnAsReplacement(rockPoolHandler, transform.position);
                }
            }
            else
            {
                onFinishedFlipping?.Invoke();
                OnHPChanged?.Invoke();
            }
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