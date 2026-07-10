using FH.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FHC.Core.Architecture
{
    [System.Serializable]
    public struct Balancer
    {
        public event System.Action OnBalanceChanged;
        public event System.Action OnBalanced;
        public event System.Action OnOffBalanced;

        [SerializeField]
        private int value;

        [Space]
        [SerializeField]
        private OrderedEventDispatcher onBalanced;
        [SerializeField]
        private OrderedEventDispatcher onOffBalanced;

        public bool IsBalanced => value == 0;

        public int Value
        {
            get => value;

            set
            {
                ///
                if (this.value == value)
                {
                    return;
                }

                ///
                var savedBalancing = IsBalanced;

                ///
                this.value = value;

                ///
                if (savedBalancing != IsBalanced)
                {
                    InvokeBalancingEvents();
                }
            }
        }

        public void SetInitialValue(int initialValue)
        {
            value = initialValue;
            InvokeBalancingEvents();
        }

        private void InvokeBalancingEvents()
        {
            if (value == 0)
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

        public void MoveToBalance(int amount)
        {
            Value = (int)Mathf.MoveTowards(Value, 0, Mathf.Abs(amount));
        }

    }
}