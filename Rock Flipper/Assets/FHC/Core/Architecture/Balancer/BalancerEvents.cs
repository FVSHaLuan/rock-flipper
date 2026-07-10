using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FHC.Core.Architecture
{
    public class BalancerEvents : MonoBehaviourWithInit
    {
        [SerializeField]
        private UnityEvent onBalanced;
        [SerializeField]
        private UnityEvent onOffBalanced;

        private Balancer balancer;

        protected override bool Init()
        {
            ///
            balancer.OnBalanced += Balancer_OnBalanced;
            balancer.OnOffBalanced += Balancer_OnOffBalanced;

            ///
            return base.Init();
        }

        private void Balancer_OnOffBalanced()
        {
            onOffBalanced?.Invoke();
        }

        private void Balancer_OnBalanced()
        {
            onBalanced?.Invoke();
        }

        public void IncreaseValue()
        {
            ///
            TryInit();

            ///
            balancer.Value++;
        }

        public void DecreaseValue()
        {
            ///
            TryInit();

            ///
            balancer.Value--;
        }
    }
}