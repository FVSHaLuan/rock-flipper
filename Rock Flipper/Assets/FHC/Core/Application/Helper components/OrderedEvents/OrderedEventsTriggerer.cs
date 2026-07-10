using UnityEngine;
using FH.Core.Architecture;

namespace FH.Core.HelperComponent
{
    [DisallowMultipleComponent]
    public class OrderedEventsTriggerer : MonoBehaviour, IEventTriggerer
    {
        [SerializeField]
        OrderedEventDispatcher events = new OrderedEventDispatcher();

        [ContextMenu("Trigger")]
        public void Trigger()
        {
            events.Dispatch();
        }
    }

}