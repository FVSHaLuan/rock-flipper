using UnityEngine;

namespace BT.Run.Dev
{
    [RequireComponent(typeof(UnifiedText))]
    public class DataSlotDisplayer : ExtendedMonoBehaviourRun
    {
        protected void OnEnable()
        {
            GetComponent<UnifiedText>().Text = $"{RunEntry.RunData.SlotId}";
        }

    }

}