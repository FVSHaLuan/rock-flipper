using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBot : ExtendedMonoBehaviourRun
    {
        [Header("Components")]
        [SerializeField]
        private FlipperBotFlipper flipper;
        [SerializeField]
        private FlipperBotPoolHandler poolHandler;

        private CooldownObject flipCooldown;

        public FlipperBotPoolHandler PoolHandler => poolHandler;

        protected void Start()
        {
            flipCooldown = new CooldownObject(BuildStats.flipperBotFlippingInterval);
            flipCooldown.StartCoolingDown();
        }

        protected void Update()
        {
            UpdateFlipper();
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
    }

}