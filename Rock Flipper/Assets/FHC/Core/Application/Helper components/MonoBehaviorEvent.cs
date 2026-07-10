using UnityEngine;
using FH.Core.Architecture;
using System.Collections;

namespace FH.Core.HelperComponent
{
    [DisallowMultipleComponent]
    public class MonoBehaviorEvent : MonoBehaviour
    {
        public event System.Action OnDisabled;

        [Header("Activation")]
        [SerializeField]
        private OrderedEventDispatcher onAwake = new OrderedEventDispatcher();
        [SerializeField]
        private OrderedEventDispatcher onEnable = new OrderedEventDispatcher();
        [SerializeField]
        private OrderedEventDispatcher onEndOfEnabledFrame = new OrderedEventDispatcher();
        [SerializeField]
        private OrderedEventDispatcher onStart = new OrderedEventDispatcher();
        [SerializeField]
        private OrderedEventDispatcher onEndOfStartFrame = new OrderedEventDispatcher();
        [SerializeField]
        private OrderedEventDispatcher onDisable = new OrderedEventDispatcher();

        [Space]
        [SerializeField]
        private OrderedEventDispatcher onParticleCollision;

        protected void Destroy()
        {
            Destroy(gameObject);
        }

        protected void Awake()
        {
            onAwake.Dispatch();
        }

        protected void OnEnable()
        {
            ///
            StartCoroutine(WaitForEndOfFrame(OnEndOfEnabledFrame));

            ///
            onEnable.Dispatch();
        }

        protected void Start()
        {
            ///
            StartCoroutine(WaitForEndOfFrame(OnEndOfStartFrame));

            ///
            onStart.Dispatch();
        }

        protected void OnDisable()
        {
            onDisable.Dispatch();
            OnDisabled?.Invoke();
        }

        protected void OnParticleCollision(GameObject other)
        {
            onParticleCollision?.Dispatch();
        }

        private void OnEndOfEnabledFrame()
        {
            onEndOfEnabledFrame?.Dispatch();
        }

        private void OnEndOfStartFrame()
        {
            onEndOfStartFrame?.Dispatch();
        }

        private IEnumerator WaitForEndOfFrame(System.Action action)
        {
            ///
            yield return new WaitForEndOfFrame();

            ///
            action?.Invoke();
        }
    }

}