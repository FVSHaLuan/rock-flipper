using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Shows a Flipper Bot's Idle state: the eye ball fades to gray and sleeping particles (zzz) come out of its head
    /// </summary>
    public class FlipperBotIdleVisual : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private FlipperBot flipperBot;

        [Header("Eye Ball")]
        [SerializeField]
        private SpriteRenderer eyeBallRenderer;
        [SerializeField]
        private Color idleEyeBallColor = Color.gray;
        [SerializeField, Tooltip("Seconds the eye ball takes to fade between its Active and Idle colors.")]
        private float eyeBallColorFadeDuration = 0.3f;

        [Header("Sleeping Particles")]
        [SerializeField]
        private ParticleSystem sleepingParticles;

        private Color activeEyeBallColor;
        private float idleBlend;

        private bool IsIdle => flipperBot.State == FlipperBot.FlipperBotState.Idle;

        protected override void ExtendedAwake()
        {
            activeEyeBallColor = eyeBallRenderer.color;
        }

        protected void OnEnable()
        {
            flipperBot.OnStateChanged += FlipperBot_OnStateChanged;

            // a pooled bot can come back in any state, so snap to it without fading
            idleBlend = IsIdle ? 1f : 0f;
            ApplyEyeBallColor();
            UpdateSleepingParticles(clearOnStop: true);
        }

        protected void OnDisable()
        {
            flipperBot.OnStateChanged -= FlipperBot_OnStateChanged;
        }

        protected void Update()
        {
            var targetBlend = IsIdle ? 1f : 0f;
            if (idleBlend == targetBlend)
                return;

            var step = eyeBallColorFadeDuration > 0f ? Time.deltaTime / eyeBallColorFadeDuration : 1f;
            idleBlend = Mathf.MoveTowards(idleBlend, targetBlend, step);
            ApplyEyeBallColor();
        }

        private void FlipperBot_OnStateChanged()
        {
            UpdateSleepingParticles(clearOnStop: false);
        }

        private void ApplyEyeBallColor()
        {
            eyeBallRenderer.color = Color.Lerp(activeEyeBallColor, idleEyeBallColor, idleBlend);
        }

        private void UpdateSleepingParticles(bool clearOnStop)
        {
            if (IsIdle)
            {
                if (!sleepingParticles.isPlaying)
                {
                    sleepingParticles.Play(true);
                }
            }
            else
            {
                // let the zzz already out finish rising when the bot wakes up
                sleepingParticles.Stop(true, clearOnStop ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

}
