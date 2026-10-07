using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Agame.Run.Combat
{
    /// <summary>
    /// L VIII: fires a laser at a random airborne rock every shooting interval
    /// (if no rock is airborne when the interval is up, it fires at the first one that is)
    /// </summary>
    public class Prism : ExtendedMonoBehaviourRun
    {
        [SerializeField, FormerlySerializedAs("beam")]
        private BeamEffect beamPrototype;
        [SerializeField]
        private Transform beamOrigin;
        [SerializeField]
        private float beamDuration = 0.15f;
        [SerializeField]
        private UnityEvent onShot;

        private float shootingTimer;

        private readonly List<Rock> candidates = new List<Rock>();
        private readonly List<BeamEffect> activeBeams = new List<BeamEffect>();

        protected void OnEnable()
        {
            shootingTimer = 0f;
        }

        protected void OnDisable()
        {
            // the beam coroutines die with this component, so their beams have to go back to the pool here
            foreach (var beam in activeBeams)
                beam.TryReturnToPoolAndDeactivate();
            activeBeams.Clear();
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
            StartCoroutine(ShowBeam(target));

            ///
            ZapRock(target);

            ///
            onShot?.Invoke();
        }

        private IEnumerator ShowBeam(Rock target)
        {
            var beam = CurrentGeneralPool.TakeInstance(beamPrototype, this);
            activeBeams.Add(beam);
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
            activeBeams.Remove(beam);
            beam.TryReturnToPoolAndDeactivate();
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
