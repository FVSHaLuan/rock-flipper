using UnityEngine;

namespace Agame.Run.Combat
{
    public class PlayerCursor : ExtendedMonoBehaviourRun
    {
        protected void Update()
        {
            var th = transformHandle;
            th.position = entry.GetPointerPositionViaConversionCamera();
        }
    }

}