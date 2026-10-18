using FHC.Core.Architecture;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public class CombatSoundEffectManager : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameAudioController clickLoopPlayer;
        [SerializeField]
        private UnityEvent playStartFlippingDelegate;
        [SerializeField]
        private UnityEvent playNormalLandingDelegate;
        [SerializeField]
        private UnityEvent playBreakingLandingDelegate;

        private BalancerWithObjects clickLoopPlayerUnmutedBalancer = new BalancerWithObjects();

        protected void OnDestroy()
        {
            ClickHold.OnAnyClickHoldStarted -= ClickHold_OnAnyClickHoldStarted;
            ClickHold.OnAnyClickHoldCancelled -= ClickHold_OnAnyClickHoldCancelled;
        }

        protected void Start()
        {
            ClickHold.OnAnyClickHoldStarted += ClickHold_OnAnyClickHoldStarted;
            ClickHold.OnAnyClickHoldCancelled += ClickHold_OnAnyClickHoldCancelled;
        }

        private void ClickHold_OnAnyClickHoldCancelled(ClickHold obj)
        {
            StopClickLoop(obj);
        }

        private void ClickHold_OnAnyClickHoldStarted(ClickHold obj)
        {
            PlayClickLoop(obj);
        }

        [ContextMenu("PlayClickLoop"), PlayModeOnly]
        private void PlayClickLoop()
        {
            PlayClickLoop(this);
        }

        [ContextMenu("StopClickLoop"), PlayModeOnly]
        private void StopClickLoop()
        {
            StopClickLoop(this);
        }

        public void PlayClickLoop(object lockObject)
        {
            clickLoopPlayerUnmutedBalancer.AddObject(lockObject);
            clickLoopPlayer.Play();
        }

        public void StopClickLoop(object lockObject)
        {
            clickLoopPlayerUnmutedBalancer.RemoveObject(lockObject);
            if (clickLoopPlayerUnmutedBalancer.IsBalanced)
            {
                clickLoopPlayer.StopImmediately();
            }
        }

        public void PlayStartFlipping()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playStartFlippingDelegate?.Invoke();
        }

        public void PlayNormalLanding()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playNormalLandingDelegate?.Invoke();
        }

        public void PlayBreakingLanding()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playBreakingLandingDelegate?.Invoke();
        }
    }

}