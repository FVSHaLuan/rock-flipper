using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Agame.Run.Combat
{
    /// <summary>
    /// L VIII: every shooting interval, fires a random number of beams (between min and max beam count, max if min > max)
    /// at distinct random airborne rocks, capped by how many rocks are airborne
    /// (if no rock is airborne when the interval is up, it fires as soon as one is)
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
            CollectAirborneRocks();
            if (candidates.Count == 0)
                return;

            ///
            shootingTimer = 0f;
            Shoot(Mathf.Min(RollBeamCount(), candidates.Count));
        }

        private void CollectAirborneRocks()
        {
            candidates.Clear();
            foreach (var rock in RunEntry.rockInstanceManager.ActiveRocks)
            {
                if (rock.Flippable.IsFlipping)
                    candidates.Add(rock);
            }
        }

        private int RollBeamCount()
        {
            var min = BuildStats.prismMinBeamCount;
            var max = BuildStats.prismMaxBeamCount;
            return min > max ? max : Random.Range(min, max + 1);
        }

        /// <summary>
        /// Each beam hits a different rock, picked at random from <see cref="candidates"/>
        /// </summary>
        private void Shoot(int beamCount)
        {
            if (beamCount <= 0)
                return;

            ///
            for (var i = 0; i < beamCount; i++)
            {
                // partial Fisher-Yates: move a random remaining candidate to slot i
                var pick = Random.Range(i, candidates.Count);
                (candidates[i], candidates[pick]) = (candidates[pick], candidates[i]);

                var target = candidates[i];
                StartCoroutine(ShowBeam(target));
                ZapRock(target);
            }

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
                target.PlayZappedEffect();

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
        /// What a beam hit does to the rock (undecided in the GDD, intentionally left empty for now)
        /// </summary>
        private void ZapRock(Rock rock)
        {
        }
    }

}
