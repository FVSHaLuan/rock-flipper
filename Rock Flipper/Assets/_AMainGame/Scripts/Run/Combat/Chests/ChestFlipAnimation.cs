using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Squash and stretch of a chest's view: it stretches as it takes off and squashes as it lands, scaling around
    /// a pivot near its bottom so it stays on the ground. Visual only. Stops when the chest starts opening,
    /// since <see cref="ChestOpeningAnimation"/> then drives the same view.
    /// </summary>
    public class ChestFlipAnimation : MonoBehaviour
    {
        [SerializeField]
        private Chest chest;
        [SerializeField, Tooltip("Squashed and stretched. Also rotated by FlippableRotator while flipping.")]
        private Transform view;
        [SerializeField, Tooltip("In the view's local space: the point that stays put while the view is scaled.")]
        private Vector2 pivot = new Vector2(0f, -0.3f);

        [Header("Take Off")]
        [SerializeField, Min(0)]
        private float takeOffDuration = 0.4f;
        // Starts squashed and snaps into a stretch that is also bigger overall: the chest already spins as it
        // takes off, which hides a stretch alone.
        [SerializeField, Tooltip("Width over the take-off (0 to 1), relative to the view's normal scale.")]
        private AnimationCurve takeOffScaleX = new AnimationCurve(
            new Keyframe(0f, 1.3f),
            new Keyframe(0.12f, 1.1f),
            new Keyframe(0.5f, 0.96f),
            new Keyframe(1f, 1f));
        [SerializeField, Tooltip("Height over the take-off (0 to 1), relative to the view's normal scale.")]
        private AnimationCurve takeOffScaleY = new AnimationCurve(
            new Keyframe(0f, 0.75f),
            new Keyframe(0.12f, 1.45f),
            new Keyframe(0.5f, 0.96f),
            new Keyframe(1f, 1f));

        [Header("Landing")]
        [SerializeField, Min(0)]
        private float landingDuration = 0.35f;
        [SerializeField, Tooltip("Width over the landing (0 to 1), relative to the view's normal scale.")]
        private AnimationCurve landingScaleX = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.15f, 1.25f),
            new Keyframe(0.45f, 0.93f),
            new Keyframe(0.75f, 1.02f),
            new Keyframe(1f, 1f));
        [SerializeField, Tooltip("Height over the landing (0 to 1), relative to the view's normal scale.")]
        private AnimationCurve landingScaleY = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.15f, 0.72f),
            new Keyframe(0.45f, 1.1f),
            new Keyframe(0.75f, 0.97f),
            new Keyframe(1f, 1f));

        private Vector3 normalViewPosition;
        private Vector3 normalViewScale;

        private AnimationCurve currentScaleX;
        private AnimationCurve currentScaleY;
        private float currentDuration;
        private float elapsed;

        private bool IsPlaying => currentScaleX != null;

        protected void Awake()
        {
            normalViewPosition = view.localPosition;
            normalViewScale = view.localScale;
        }

        protected void Start()
        {
            chest.Flippable.OnStartedFlipping += Flippable_OnStartedFlipping;
            chest.OnLanded += Chest_OnLanded;
            chest.OnOpened += Chest_OnOpened;
            chest.OnStartedNewLife += Chest_OnStartedNewLife;
        }

        protected void OnDisable()
        {
            Stop();
        }

        private void Flippable_OnStartedFlipping()
        {
            Play(takeOffScaleX, takeOffScaleY, takeOffDuration);
        }

        private void Chest_OnLanded()
        {
            Play(landingScaleX, landingScaleY, landingDuration);
        }

        private void Chest_OnOpened()
        {
            Stop();
        }

        private void Chest_OnStartedNewLife()
        {
            Stop();
        }

        private void Play(AnimationCurve scaleX, AnimationCurve scaleY, float duration)
        {
            currentScaleX = scaleX;
            currentScaleY = scaleY;
            currentDuration = duration;
            elapsed = 0f;
        }

        private void Stop()
        {
            if (!IsPlaying)
                return;

            ///
            currentScaleX = null;
            currentScaleY = null;
            view.localPosition = normalViewPosition;
            view.localScale = normalViewScale;
        }

        // Late so the pivot correction uses the rotation FlippableRotator set this frame.
        protected void LateUpdate()
        {
            if (!IsPlaying)
                return;

            ///
            elapsed += Time.deltaTime;
            if (elapsed >= currentDuration)
            {
                Stop();
                return;
            }

            ///
            var progress = elapsed / currentDuration;
            var factor = new Vector3(currentScaleX.Evaluate(progress), currentScaleY.Evaluate(progress), 1f);
            view.localScale = Vector3.Scale(normalViewScale, factor);

            ///
            // Shifts the view so the pivot stays where it is at normal scale.
            var pivotShift = Vector3.Scale(Vector3.Scale(normalViewScale, Vector3.one - factor), pivot);
            view.localPosition = normalViewPosition + view.localRotation * pivotShift;
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            chest = GetComponent<Chest>();
        }
#endif
    }

}
