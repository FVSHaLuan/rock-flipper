using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Makes the rock flip by itself once it has stayed on the ground for long enough
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
            SelfFlip();
        }

        private void SelfFlip()
        {
            Rock.Flippable.TryFlipping();
        }
    }

}
