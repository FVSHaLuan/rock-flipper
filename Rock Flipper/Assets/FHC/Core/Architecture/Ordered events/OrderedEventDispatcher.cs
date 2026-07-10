using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using SubjectNerd.Utilities;

namespace FH.Core.Architecture
{
    [System.Serializable]
    public class OrderedEventDispatcher
    {
        [SerializeField, Reorderable(noHeader: true, headerString: "Event Set")]
        List<UnityEvent> listeners = new List<UnityEvent>();

        List<UnityEvent> Listeners
        {
            get
            {
                return listeners;
            }

            set
            {
                listeners = value;
            }
        }

        public int UnityEventsCount
        {
            get
            {
                return listeners.Count;
            }
        }

        public int FinalListenersCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < listeners.Count; i++)
                {
                    count += listeners[i].GetPersistentEventCount();
                }
                return count;
            }
        }

        public void Dispatch()
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                var listener = Listeners[i];
                if (listener != null)
                {
                    try
                    {
                        listener.Invoke();
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }
            }
        }


    }
}
