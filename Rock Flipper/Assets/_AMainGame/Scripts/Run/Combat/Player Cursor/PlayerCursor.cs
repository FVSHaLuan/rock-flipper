using FHC.Core.Architecture;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Agame.Run.Combat
{
    public class PlayerCursor : ExtendedMonoBehaviourRun
    {
        private List<FlippableByPlayerCursor> flippableHits = new List<FlippableByPlayerCursor>();
        private List<FlippableByPlayerCursor> lastFlippableHits = new List<FlippableByPlayerCursor>();

        private BalancerWithObjects flippingLockBalancer = new BalancerWithObjects();

        public bool IsFlippingLocked => !flippingLockBalancer.IsBalanced;

        public void AddFlippingLock(object @object)
        {
            flippingLockBalancer.AddObject(@object);
        }

        public void RemoveFlippingLock(object @object)
        {
            flippingLockBalancer.RemoveObject(@object);
        }

        protected void Update()
        {
            var th = transformHandle;
            th.position = entry.GetPointerPositionViaConversionCamera();
        }

        protected void LateUpdate()
        {
            (lastFlippableHits, flippableHits) = (flippableHits, lastFlippableHits);
            flippableHits.Clear();
            if (IsFlippingLocked)
            {
                return;
            }

            ///
            FindHits();

            ///
            StartHoveringNewHits();

            ///
            if (BuildStats.enabledMouseHover)
            {
                FlipHits();
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame && FlipHits())
            {
                entry.clickParticleManager.Play();
            }
        }

        private void StartHoveringNewHits()
        {
            foreach (var item in flippableHits)
            {
                if (item.isActiveAndEnabled && !lastFlippableHits.Contains(item))
                {
                    item.StartHovering();
                }
            }
        }

        /// <summary>
        /// Returns whether anything started flipping
        /// </summary>
        private bool FlipHits()
        {
            var flippedAny = false;
            foreach (var item in flippableHits)
            {
                if (item.isActiveAndEnabled && item.CooledDown && item.Flippable.TryFlipping(FlipSource.Mouse))
                {
                    flippedAny = true;
                }
            }
            return flippedAny;
        }

        private void FindHits()
        {
            FindHits(transformHandle.position, flippableHits);
        }

        /// <summary>
        /// The flippables a cursor at <paramref name="position"/> would hit (point or mouse-radius circle)
        /// </summary>
        public void FindHits(Vector2 position, List<FlippableByPlayerCursor> results)
        {
            if (BuildStats.enabledMouseRadius)
            {
                SimpleCast2D.CircleCast(position, BuildStats.mouseRadius, true, results);
            }
            else
            {
                SimpleCast2D.PointCast(position, true, results);
            }
        }
    }

}