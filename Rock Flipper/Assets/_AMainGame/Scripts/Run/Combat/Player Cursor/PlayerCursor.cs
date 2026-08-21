using System.Collections.Generic;
using UnityEngine;

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
            if (BuildStats.enabledPlayerCursorRadius)
            {
                SimpleCast2D.CircleCast(transformHandle.position, BuildStats.playerCursorRadius, true, flippableHits);
            }
            else
            {
                SimpleCast2D.PointCast(transformHandle.position, true, flippableHits);
            }

            ///
            foreach (var item in flippableHits)
            {
                if (item.isActiveAndEnabled)
                {
                    item.Flippable.TryFlipping();
                }
            }
        }
    }

}