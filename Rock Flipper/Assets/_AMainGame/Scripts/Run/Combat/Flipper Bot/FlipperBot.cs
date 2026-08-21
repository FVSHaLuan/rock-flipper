using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBot : ExtendedMonoBehaviourRun
    {
        [Header("Components")]
        [SerializeField]
        private FlipperBotFlipper flipper;

        private CooldownObject flipCooldown;

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