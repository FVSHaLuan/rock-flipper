using System.Collections;
using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Gives the rock a chance to automatically flip again after it lands (and survives the landing)
    /// </summary>
    public class BouncyRock : RockExtender
    {
        private Coroutine bounceCoroutine;

        protected void Start()
        {
            Rock.OnLanded += Rock_OnLanded;
        }

        protected void OnDisable()
        {
            bounceCoroutine = null;
        }

        private void Rock_OnLanded()
        {
            if (Random.value >= BuildStats.bouncyRockBounceChance)
                return;

            ///
            var bounceDelay = TierStats.landingCooldown;

            ///
            if (bounceDelay <= 0f)
            {
                Bounce();
                return;
            }

            ///
            if (bounceCoroutine != null)
                StopCoroutine(bounceCoroutine);
            bounceCoroutine = StartCoroutine(BounceAfterDelay(bounceDelay));
        }

        private IEnumerator BounceAfterDelay(float bounceDelay)
        {
            yield return new WaitForSeconds(bounceDelay);

            ///
            bounceCoroutine = null;
            Bounce();
        }

        private void Bounce()
        {
            Rock.Flippable.TryFlipping();
        }
    }

}
