using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Wobbles a grounded flippable (rock, chest) when the cursor starts hovering over it but hovering can't flip it:
    /// mouse hover isn't unlocked yet, or its landing cooldown isn't over.
    /// Visual only. Goes next to its <see cref="FlippableByPlayerCursor"/>, and animates the transform of its <see cref="view"/>.
    /// </summary>
    public class FlippableHoverAnimation : MonoBehaviour
    {
        [SerializeField, Tooltip("Tilted and swollen by the wobble. Use the view, not the root, so the shadow and HP bar stay put.")]
        private Transform view;

        [Space]
        [SerializeField, Min(0)]
        private float duration = 0.35f;
        [SerializeField, Tooltip("Back-and-forth cycles per second.")]
        private float wobbleFrequency = 6f;
        [SerializeField, Tooltip("Max tilt in degrees, at the start of the wobble. It fades out to 0 by the end.")]
        private float wobbleAngle = 10f;
        [SerializeField, Tooltip("The view's scale over the wobble (0 to 1), relative to its normal scale.")]
        private AnimationCurve wobbleScale = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.3f, 1.1f),
            new Keyframe(1f, 1f));

        private FlippableByPlayerCursor flippableByPlayerCursor;
        private Quaternion normalViewRotation;
        private Vector3 normalViewScale;
        private float elapsed;
        private bool isPlaying;

        protected void Awake()
        {
            normalViewRotation = view.localRotation;
            normalViewScale = view.localScale;
        }

        protected void Start()
        {
            flippableByPlayerCursor = GetComponentInParent<FlippableByPlayerCursor>();
            flippableByPlayerCursor.OnStartedHovering += FlippableByPlayerCursor_OnStartedHovering;
            flippableByPlayerCursor.Flippable.OnStartedFlipping += Flippable_OnStartedFlipping;
        }

        protected void OnDisable()
        {
            Stop();
        }

        private void FlippableByPlayerCursor_OnStartedHovering()
        {
            ///
            // Locked: e.g. a chest that is opening, whose view ChestOpeningAnimation drives.
            var flippable = flippableByPlayerCursor.Flippable;
            if (flippable.IsFlipping || flippable.IsFlippingLocked)
                return;

            ///
            if (RunEntry.Instance.BuildStats.enabledMouseHover && flippableByPlayerCursor.CooledDown)
                return;

            ///
            Play();
        }

        private void Flippable_OnStartedFlipping()
        {
            Stop();
        }

        [ContextMenu("Play"), PlayModeOnly]
        private void Play()
        {
            elapsed = 0f;
            isPlaying = true;
        }

        private void Stop()
        {
            if (!isPlaying)
                return;

            ///
            isPlaying = false;
            view.localRotation = normalViewRotation;
            view.localScale = normalViewScale;
        }

        // Not late, so a chest's landing squash (ChestFlipAnimation scales the same view in LateUpdate) wins over the swell.
        protected void Update()
        {
            if (!isPlaying)
            {
                return;
            }

            ///
            elapsed += Time.deltaTime;
            if (elapsed >= duration)
            {
                Stop();
                return;
            }

            ///
            var progress = elapsed / duration;
            var angle = Mathf.Sin(elapsed * wobbleFrequency * 2f * Mathf.PI) * wobbleAngle * (1f - progress);
            view.localRotation = normalViewRotation * Quaternion.Euler(0f, 0f, angle);
            view.localScale = normalViewScale * wobbleScale.Evaluate(progress);
        }
    }

}
