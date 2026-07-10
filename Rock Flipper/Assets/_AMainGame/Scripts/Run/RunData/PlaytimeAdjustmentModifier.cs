using UnityEngine;

namespace Agame.Run
{
    public class PlaytimeAdjustmentModifier : ExtendedMonoBehaviourRun
    {
#if UNITY_EDITOR
        protected void Update()
        {
            ///
            if (entry.timeScaleManager.IsGameplayBeingPaused)
            {
                RunData.PlayTimeAdjustment -= Time.unscaledDeltaTime;
            }
        }
#endif
    }

}