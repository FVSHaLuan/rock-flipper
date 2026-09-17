using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Agame.Run.Combat
{
    public class PlayerCursor : ExtendedMonoBehaviourRun
    {
        private List<FlippableByPlayerCursor> flippableHits = new List<FlippableByPlayerCursor>();

        protected void Update()
        {
            var th = transformHandle;
            th.position = entry.GetPointerPositionViaConversionCamera();
        }

        protected void LateUpdate()
        {
            flippableHits.Clear();
            if (BuildStats.enabledPlayerCursorHover)
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
                    item.Flippable.TryFlipping();
                }
            }
        }

        private void FindHitsByHovering()
        {
            if (BuildStats.enabledPlayerCursorRadius)
            {
                SimpleCast2D.CircleCast(transformHandle.position, BuildStats.playerCursorRadius, true, flippableHits);
            }
            else
            {
                SimpleCast2D.PointCast(transformHandle.position, true, flippableHits);
            }
        }

        private void FindHitsByClicking()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                SimpleCast2D.CircleCast(transformHandle.position, BuildStats.playerCursorRadius, true, flippableHits);
            }
        }
    }

}