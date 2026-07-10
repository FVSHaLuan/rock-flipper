using FH.Core.Architecture.Pool;
using System.Collections;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class ExplosionEffectObject : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private SpriteRenderer circle;
        [SerializeField]
        private float startAlpha = 0.3f;
        [SerializeField]
        private float duration = 0.5f;

        private float startRadius;

        public void Play(float radius)
        {
            this.startRadius = radius;

            ///
            gameObject.SetActive(true);

            ///
            StartCoroutine(Disappear());
        }

        private IEnumerator Disappear()
        {
            float t = 0;
            while (true)
            {
                ///
                var progress = t / duration;
                var radius = Mathf.Lerp(startRadius, 0, progress);
                var alpha = Mathf.Lerp(startAlpha, 0, progress);

                ///
                circle.transform.localScale = Vector3.one * radius * 2;
                circle.color = circle.color.OverrideAlpha(alpha);

                ///
                if (t >= duration)
                {
                    break;
                }

                ///
                t += Time.deltaTime;

                ///
                yield return null;
            }

            ///
            gameObject.SetActive(false);
        }
    }

}