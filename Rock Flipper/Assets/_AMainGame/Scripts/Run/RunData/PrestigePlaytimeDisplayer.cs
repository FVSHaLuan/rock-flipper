using UnityEngine;

namespace Agame.Run
{
    public class PrestigePlaytimeDisplayer : ValueDisplayerUnified<float>
    {
        [SerializeField]
        private bool isPreviousPrestige;

        protected override string GetString(float value)
        {
            ///
            return TimeStringHelper.GetStringFromSeconds(value);
        }

        protected override float GetCurrentValue()
        {
            var runData = RunEntry.Instance.RunData;
            return isPreviousPrestige ? runData.PreviousPrestigePlayTime : runData.PrestigePlayTimeNow;
        }
    }
}
