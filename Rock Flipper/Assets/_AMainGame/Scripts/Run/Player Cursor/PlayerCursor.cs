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
            SimpleCast2D.PointCast(transformHandle.position, true, flippableHits);
            foreach (var item in flippableHits)
            {
                item.Flippable.TryFlipping();
            }
        }
    }

}