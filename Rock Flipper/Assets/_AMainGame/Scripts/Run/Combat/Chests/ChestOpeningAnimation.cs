using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    /// <summary>
    /// What an opening chest looks like: it shakes harder and harder while swelling, then pops (grows a bit and shrinks
    /// to nothing). Visual only: <see cref="Chest"/> decides when it plays and what happens around it.
    /// Goes on the chest's root, which it scales during the pop.
    /// </summary>
    public class ChestOpeningAnimation : MonoBehaviour
    {
        [SerializeField, Tooltip("Shaken and swollen during the shake. Its position is where the found item appears.")]
        private Transform view;

        [Header("Shake")]
        [SerializeField, Min(0)]
        private float shakeDuration = 0.45f;
        [SerializeField, Tooltip("Back-and-forth cycles per second.")]
        private float shakeFrequency = 14f;
        [SerializeField, Tooltip("Max tilt in degrees, reached at the end of the shake.")]
        private float shakeAngle = 12f;
        [SerializeField, Tooltip("The view's scale at the end of the shake, relative to its normal scale.")]
        private float shakeEndScale = 1.15f;

        [Header("Pop")]
        [SerializeField, Min(0)]
        private float popDuration = 0.2f;
        [SerializeField, Tooltip("The whole chest's scale over the pop (0 to 1), relative to its normal scale.")]
        private AnimationCurve popScale = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.35f, 1.25f),
            new Keyframe(1f, 0f));

        [Space]
        [SerializeField]
        private UnityEvent onShakeStarted;
        [SerializeField]
        private UnityEvent onPopped;

        private Quaternion normalViewRotation;
        private Vector3 normalViewScale;
        private Vector3 normalScale;

        /// <summary>
        /// Where the found item appears: the center of the chest's view.
        /// </summary>
        public Vector3 ItemOrigin => view.position;

        protected void Awake()
        {
            normalViewRotation = view.localRotation;
            normalViewScale = view.localScale;
            normalScale = transform.localScale;
        }

        /// <summary>
        /// Plays the whole animation. <paramref name="onPopping"/> is called as the chest pops, when its content comes out.
        /// The chest is left popped (scale 0); <see cref="ResetView"/> brings it back.
        /// </summary>
        public IEnumerator Play(System.Action onPopping)
        {
            ///
            onShakeStarted?.Invoke();
            for (var elapsed = 0f; elapsed < shakeDuration; elapsed += Time.deltaTime)
            {
                var progress = elapsed / shakeDuration;
                var angle = Mathf.Sin(elapsed * shakeFrequency * 2f * Mathf.PI) * shakeAngle * progress;
                view.localRotation = normalViewRotation * Quaternion.Euler(0f, 0f, angle);
                view.localScale = normalViewScale * Mathf.Lerp(1f, shakeEndScale, progress);
                yield return null;
            }
            view.localRotation = normalViewRotation;

            ///
            onPopping?.Invoke();
            onPopped?.Invoke();
            for (var elapsed = 0f; elapsed < popDuration; elapsed += Time.deltaTime)
            {
                transform.localScale = normalScale * popScale.Evaluate(elapsed / popDuration);
                yield return null;
            }
            transform.localScale = normalScale * popScale.Evaluate(1f);
        }

        /// <summary>
        /// Undoes the animation, for a pooled chest starting a new life.
        /// </summary>
        public void ResetView()
        {
            view.localRotation = normalViewRotation;
            view.localScale = normalViewScale;
            transform.localScale = normalScale;
        }
    }

}
