using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBot : ExtendedMonoBehaviourRun
    {
        public enum FlipperBotState
        {
            Active,
            Idle,
        }

        [Header("Components")]
        [SerializeField]
        private FlipperBotFlipper flipper;
        [SerializeField]
        private FlipperBotPoolHandler poolHandler;

        [Header("State")]
        [SerializeField, Range(0f, 1f), Tooltip("Randomizes each Active/Idle duration by up to +/- this fraction (e.g. 0.1 = +/-10%) so bots don't all switch state in lockstep.")]
        private float stateTimeVariance = 0.1f;

        private CooldownObject flipCooldown;
        private CooldownObject stateCooldown;

        public FlipperBotPoolHandler PoolHandler => poolHandler;

        public FlipperBotState State { get; private set; } = FlipperBotState.Active;

        protected void Start()
        {
            flipCooldown = new CooldownObject(BuildStats.flipperBotFlippingInterval);
            flipCooldown.StartCoolingDown();

            stateCooldown = new CooldownObject(GetRandomizedStateDuration(BuildStats.flipperBotActiveTime));
            stateCooldown.StartCoolingDown();
        }

        protected void Update()
        {
            UpdateState();

            if (State == FlipperBotState.Active)
            {
                UpdateFlipper();
            }
        }

        private void UpdateState()
        {
            stateCooldown.Update(Time.deltaTime);
            if (!stateCooldown.IsCoolingDown)
            {
                SwitchState(State == FlipperBotState.Active ? FlipperBotState.Idle : FlipperBotState.Active);
            }
        }

        private void SwitchState(FlipperBotState newState)
        {
            State = newState;
            var baseDuration = State == FlipperBotState.Active ? BuildStats.flipperBotActiveTime : BuildStats.flipperBotIdleTime;
            stateCooldown.CooldownTime = GetRandomizedStateDuration(baseDuration);
            stateCooldown.StartCoolingDown();
        }

        private float GetRandomizedStateDuration(float baseDuration)
        {
            return baseDuration * Random.Range(1f - stateTimeVariance, 1f + stateTimeVariance);
        }

        private void UpdateFlipper()
        {
            flipCooldown.CooldownTime = BuildStats.flipperBotFlippingInterval;
            flipCooldown.Update(Time.deltaTime);
            if (!flipCooldown.IsCoolingDown)
            {
                flipper.TryToFlipRocks();
                flipCooldown.StartCoolingDown();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Spawn Flipper Bot"), PlayModeOnly]
        private void Editor_Spawn()
        {
            RunEntry.flipperBotInstanceManager.SpawnFlipperBot(poolHandler);
        }
#endif
    }

}