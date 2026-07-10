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
        private PooledAudioPlayer coreHitByBallPlayer;

        [Space]
        [SerializeField]
        private UnityEvent playCoreHitByBallDelegate;
        [SerializeField]
        private UnityEvent playCoreDeathDelegate;

        private BalancerWithObjects clickLoopPlayerUnmutedBalancer = new BalancerWithObjects();
        private float maxCoreHitByBallVolume;

        protected override bool Init()
        {
            ///
            maxCoreHitByBallVolume = coreHitByBallPlayer.selfVolume;

            ///
            return base.Init();
        }

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

        [ContextMenu("PlayCoreHitByBall"), PlayModeOnly]
        public void PlayCoreHitByBall()
        {
            ///
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            UpdateCoreHitByBallVolume();

            ///
            playCoreHitByBallDelegate?.Invoke();
        }

        [ContextMenu("PlayCoreDeath"), PlayModeOnly]
        public void PlayCoreDeath()
        {
            ///
            if (CurrentRunState != RunState.Combat)
            {
                return;
            }

            ///
            playCoreDeathDelegate?.Invoke();
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

        public void UpdateCoreHitByBallVolume()
        {
            coreHitByBallPlayer.selfVolume = maxCoreHitByBallVolume * gameSetting.coreHitSoundEffectVolume;
        }
    }

}