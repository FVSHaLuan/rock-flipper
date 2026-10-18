using FHC.Core.Architecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Agame.Run.Combat
{
    public class CombatSoundEffectManager : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameAudioController clickLoopPlayer;

        [Header("Rocks")]
        [SerializeField, FormerlySerializedAs("playStartFlippingDelegate")]
        private UnityEvent playRockStartFlippingDelegate;
        [SerializeField, FormerlySerializedAs("playNormalLandingDelegate")]
        private UnityEvent playRockNormalLandingDelegate;
        [SerializeField, FormerlySerializedAs("playBreakingLandingDelegate")]
        private UnityEvent playRockBreakingLandingDelegate;

        [Header("Chests")]
        [SerializeField]
        private UnityEvent playChestStartFlippingDelegate;
        [SerializeField]
        private UnityEvent playChestNormalLandingDelegate;
        [SerializeField]
        private UnityEvent playChestOpeningLandingDelegate;

        [Header("Items")]
        [SerializeField]
        private UnityEvent playItemArrivalDelegate;

        [Header("Monoliths")]
        [SerializeField]
        private UnityEvent playPrismShotDelegate;

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

        public void PlayRockStartFlipping()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playRockStartFlippingDelegate?.Invoke();
        }

        public void PlayRockNormalLanding()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playRockNormalLandingDelegate?.Invoke();
        }

        public void PlayRockBreakingLanding()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playRockBreakingLandingDelegate?.Invoke();
        }

        public void PlayChestStartFlipping()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playChestStartFlippingDelegate?.Invoke();
        }

        public void PlayChestNormalLanding()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playChestNormalLandingDelegate?.Invoke();
        }

        public void PlayChestOpeningLanding()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playChestOpeningLandingDelegate?.Invoke();
        }

        public void PlayItemArrival()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playItemArrivalDelegate?.Invoke();
        }

        public void PlayPrismShot()
        {
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playPrismShotDelegate?.Invoke();
        }
    }

}