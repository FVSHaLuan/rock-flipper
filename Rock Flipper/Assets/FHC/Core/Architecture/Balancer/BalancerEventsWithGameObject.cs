using UnityEngine;
using UnityEngine.Events;

namespace FHC.Core.Architecture
{
    public class BalancerEventsWithGameObject : MonoBehaviourWithInit
    {
        [SerializeField]
        private UnityEvent onBalanced;
        [SerializeField]
        private UnityEvent onOffBalanced;

        private BalancerWithObjects balancer = new BalancerWithObjects();

        protected override bool Init()
        {
            ///
            balancer.OnBalanced += Balancer_OnBalanced;
            balancer.OnOffBalanced += Balancer_OnOffBalanced;

            ///
            if (balancer.IsBalanced)
            {
                onBalanced?.Invoke();
            }
            else
            {
                onBalanced?.Invoke();
            }

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

        public void AddLockObject(GameObject gameObject)
        {
            ///
            TryInit();

            ///
            balancer.AddObject(gameObject);
        }

        public void RemoveLockObject(GameObject gameObject)
        {
            ///
            TryInit();

            ///
            balancer.RemoveObject(gameObject);
        }

        public void ClearLockObjects()
        {
            ///
            TryInit();

            ///
            balancer.Reset();
        }
    }

}