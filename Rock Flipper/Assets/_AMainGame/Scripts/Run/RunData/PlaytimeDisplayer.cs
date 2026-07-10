using UnityEngine;

namespace BT.Run
{
    public class PlaytimeDisplayer : ValueDisplayerUnified<float>
    {
        protected override string GetString(float value)
        {
            ///
            return TimeStringHelper.GetStringFromSeconds(value);
        }

        protected override float GetCurrentValue()
        {
            var runData = RunEntry.Instance.RunData;
            return runData.PlayTimeNow;
        }
    }

}