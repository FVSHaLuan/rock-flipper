using FHC.Core.Architecture;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Agame.Run.Combat
{
    public class PlayerCursor : ExtendedMonoBehaviourRun
    {
        private List<FlippableByPlayerCursor> flippableHits = new List<FlippableByPlayerCursor>();

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
            flippableHits.Clear();
            if (IsFlippingLocked)
            {
                return;
            }

            ///
            if (BuildStats.enabledMouseHover)
            {
                FindHitsByHovering();
            }
            else
            {
                FindHitsByClicking();
            }

            ///
            foreach (var item in flippableHits)
            {
                if (item.isActiveAndEnabled && item.CooledDown)
                {
                    item.Flippable.TryFlipping(FlipSource.Mouse);
                }
            }
        }

        private void FindHitsByHovering()
        {
            FindHits();
        }        

        private void FindHitsByClicking()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                FindHits();
            }
        }

        private void FindHits()
        {
            if (BuildStats.enabledMouseRadius)
            {
                SimpleCast2D.CircleCast(transformHandle.position, BuildStats.mouseRadius, true, flippableHits);
            }
            else
            {
                SimpleCast2D.PointCast(transformHandle.position, true, flippableHits);
            }
        }
    }

}