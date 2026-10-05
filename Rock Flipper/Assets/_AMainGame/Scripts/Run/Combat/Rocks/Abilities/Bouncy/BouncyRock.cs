using System.Collections;
using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Gives the rock a chance to automatically flip again after it lands (and survives the landing)
    /// </summary>
    public class BouncyRock : RockExtender
    {
        [SerializeField, Range(0f, 1f)]
        private float bounceChance = 0.25f;
        [SerializeField, Min(0f)]
        private float bounceDelay = 0f;

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
            if (Random.value >= bounceChance)
                return;

            ///
            if (bounceDelay <= 0f)
            {
                Bounce();
                return;
            }

            ///
            if (bounceCoroutine != null)
                StopCoroutine(bounceCoroutine);
            bounceCoroutine = StartCoroutine(BounceAfterDelay());
        }

        private IEnumerator BounceAfterDelay()
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
