using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Conditions
{
    public abstract class Condition : ScriptableObjectWithInit
    {
        public event System.Action OnConditionMet
        {
            add
            {
                ///
                OnConditionMetWrapped += value;

                ///
                if (!isActive)
                {
                    ///
                    isActive = true;

                    ///
                    OnActivated();
                }
            }

            remove
            {
                ///
                OnConditionMetWrapped -= value;

                ///
                if (isActive && OnConditionMetWrapped == null)
                {
                    ///
                    isActive = false;

                    ///
                    OnDeactivated();
                }
            }
        }

        private event System.Action OnConditionMetWrapped;

        [System.NonSerialized]
        private bool isActive;

        public abstract bool CurrentlyMet { get; }

        protected void InvokeOnConditionMet() => OnConditionMetWrapped?.Invoke();

        protected virtual void OnActivated() { }
        protected virtual void OnDeactivated() { }

#if UNITY_EDITOR
        [ContextMenu("Editor_ShowIsCurrentlyMet"), PlayModeOnly]
        private void Editor_ShowIsCurrentlyMet()
        {
            Debug.Log(CurrentlyMet);
        }
#endif
    }

}