using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    /// <summary>
    /// L VIII: fires a laser at a random airborne rock every shooting interval
    /// (if no rock is airborne when the interval is up, it fires at the first one that is)
    /// </summary>
    public class Prism : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private BeamEffect beam;
        [SerializeField]
        private Transform beamOrigin;
        [SerializeField]
        private float beamDuration = 0.15f;
        [SerializeField]
        private UnityEvent onShot;

        private float shootingTimer;
        private Coroutine beamCoroutine;

        private readonly List<Rock> candidates = new List<Rock>();

        protected void OnEnable()
        {
            shootingTimer = 0f;
            beam.gameObject.SetActive(false);
        }

        protected void OnDisable()
        {
            beamCoroutine = null;
        }

        protected void Update()
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer < BuildStats.prismShootingInterval)
                return;

            ///
            if (!TryPickAirborneRock(out var target))
                return;

            ///
            shootingTimer = 0f;
            Shoot(target);
        }

        private bool TryPickAirborneRock(out Rock target)
        {
            ///
            candidates.Clear();
            foreach (var rock in RunEntry.rockInstanceManager.ActiveRocks)
            {
                if (rock.Flippable.IsFlipping)
                    candidates.Add(rock);
            }

            ///
            if (candidates.Count == 0)
            {
                target = null;
                return false;
            }

            ///
            target = candidates[Random.Range(0, candidates.Count)];
            return true;
        }

        private void Shoot(Rock target)
        {
            ///
            if (beamCoroutine != null)
                StopCoroutine(beamCoroutine);
            beamCoroutine = StartCoroutine(ShowBeam(target));

            ///
            ZapRock(target);

            ///
            onShot?.Invoke();
        }

        private IEnumerator ShowBeam(Rock target)
        {
            beam.gameObject.SetActive(true);

            ///
            var elapsed = 0f;
            while (elapsed < beamDuration && target.isActiveAndEnabled)
            {
                beam.SetStartPoint(BeamOriginPosition);
                beam.SetEndPoint(target.transform.position);

                ///
                yield return null;
                elapsed += Time.deltaTime;
            }

            ///
            beam.gameObject.SetActive(false);
            beamCoroutine = null;
        }

        private Vector3 BeamOriginPosition => beamOrigin != null ? beamOrigin.position : transform.position;

        /// <summary>
        /// What a laser hit does to the rock (undecided in the GDD, intentionally left empty for now)
        /// </summary>
        private void ZapRock(Rock rock)
        {
        }
    }

}
