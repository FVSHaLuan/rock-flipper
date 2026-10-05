using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Gives the rock a chance to flip by itself each time it has stayed on the ground for long enough
    /// (on a failed roll, the countdown restarts)
    /// </summary>
    public class RestlessRock : RockExtender
    {
        private float groundedTime;

        protected void OnEnable()
        {
            groundedTime = 0f;
        }

        protected void Update()
        {
            ///
            if (Rock.Flippable.IsFlipping)
            {
                groundedTime = 0f;
                return;
            }

            ///
            groundedTime += Time.deltaTime;
            if (groundedTime < BuildStats.restlessRockSelfFlipDelay)
                return;

            ///
            groundedTime = 0f;
            if (Random.value >= BuildStats.restlessRockSelfFlipChance)
                return;

            ///
            SelfFlip();
        }

        private void SelfFlip()
        {
            Rock.Flippable.TryFlipping();
        }
    }

}
