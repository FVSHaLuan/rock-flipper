using FH.Core.Architecture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FHC.Core.Architecture
{
    public class BalancerWithObjectsBase<T> : IBalancerWithObjectsBase<T> where T : class
    {
        public event Action OnBalanceChanged;
        public event Action OnBalanced;
        public event Action OnOffBalanced;

        [Space]
        [SerializeField]
        private OrderedEventDispatcher onBalanced;
        [SerializeField]
        private OrderedEventDispatcher onOffBalanced;

        private HashSet<T> objects = new HashSet<T>();

#if UNITY_EDITOR        
        private List<DebugObject> debugObjects = new List<DebugObject>();
        private bool editor_IsBalance;
#endif

        public bool IsBalanced => objects.Count == 0;

        public int ObjectCount => objects.Count;

        public bool Contains(T @object)
        {
            return objects.Contains(@object);
        }

        protected void InvokeBalancingEvents()
        {
            if (ObjectCount == 0)
            {
                onBalanced?.Dispatch();
                OnBalanced?.Invoke();
                OnBalanceChanged?.Invoke();
            }
            else
            {
                onOffBalanced?.Dispatch();
                OnOffBalanced?.Invoke();
                OnBalanceChanged?.Invoke();
            }
        }

        /// <summary>
        /// Becomes balanced instantly
        /// </summary>
        /// <returns></returns>
        public bool Reset()
        {
            ///
            if (objects.Count > 0)
            {
                ///
                objects.Clear();

                ///
                InvokeBalancingEvents();

                ///
#if UNITY_EDITOR
                debugObjects.Clear();
#endif

                ///
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddObject(T @object)
        {
            ///
            var savedBalancing = IsBalanced;

            ///
#if UNITY_EDITOR
            if (@object.GetType().IsValueType)
            {
                throw new System.ArgumentException("Can not add value type");
            }
#endif

            ///
            if (@object == null)
            {
                throw new System.ArgumentNullException();
            }

            ///
            bool rs = objects.Add(@object);

            ///
            if (rs)
            {
                ///
#if UNITY_EDITOR
                debugObjects.Add(DebugObject.FromObject(@object));
                editor_IsBalance = IsBalanced;
#endif

                ///
                if (savedBalancing != IsBalanced)
                {
                    InvokeBalancingEvents();
                }
            }

            ///
            return rs;
        }

        public bool RemoveObject(T @object)
        {
            ///
            var savedBalancing = IsBalanced;

            ///
            if (@object == null)
            {
                throw new System.ArgumentNullException();
            }

            ///
            bool rs = objects.Remove(@object);

            ///
            if (rs)
            {
                ///
#if UNITY_EDITOR
                ///
                for (int i = debugObjects.Count - 1; i >= 0; i--)
                {
                    if (debugObjects[i].OriginalObject == @object)
                    {
                        debugObjects.RemoveAt(i);
                    }
                }

                ///
                editor_IsBalance = IsBalanced;
#endif

                ///
                if (savedBalancing != IsBalanced)
                {
                    InvokeBalancingEvents();
                }
            }

            ///
            return rs;
        }
    }
}
