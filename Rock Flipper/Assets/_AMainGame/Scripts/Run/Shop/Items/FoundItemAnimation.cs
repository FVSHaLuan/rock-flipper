using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows a found item (e.g. dropped by an opening chest) with an <see cref="ItemAvatar"/>: it pops up from where it
    /// was found, hovers for a moment, then flies to the Items tab button, shrinking as its name fades out.
    /// Visual only: the item is already found when this plays.
    /// </summary>
    public class FoundItemAnimation : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private ItemAvatar avatarPrototype;
        [SerializeField, Tooltip("Where the item flies to: the Items tab button. A UI element in any canvas render mode, or a world-space transform.")]
        private Transform target;

        [Header("Appear")]
        [SerializeField, Min(0)]
        private float appearDuration = 0.35f;
        [SerializeField, Tooltip("How far up the item rises from where it was found while appearing.")]
        private float appearRise = 0.6f;
        [SerializeField, Tooltip("The item's scale over the appearing (0 to 1), relative to the avatar prefab's scale.")]
        private AnimationCurve appearScale = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.7f, 1.2f),
            new Keyframe(1f, 1f));

        [Header("Hover")]
        [SerializeField, Min(0)]
        private float hoverDuration = 0.6f;

        [Header("Flight")]
        [SerializeField, Min(0)]
        private float flightDuration = 0.55f;
        [SerializeField, Tooltip("Flight progress (0 to 1) over the flight time (0 to 1). Starts slow by default, so the item speeds up toward the button.")]
        private AnimationCurve flightProgress = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(1f, 1f, 2f, 2f));
        [SerializeField, Tooltip("How far the flight path bulges upward at its middle.")]
        private float flightArcHeight = 1f;
        [SerializeField, Tooltip("The item's scale on arrival, relative to the avatar prefab's scale.")]
        private float flightEndScale = 0.3f;
        [SerializeField, Min(0), Tooltip("Seconds the name takes to fade out once the flight starts.")]
        private float nameFadeDuration = 0.15f;

        [Space]
        [SerializeField]
        private UnityEvent onAppeared;
        [SerializeField, Tooltip("Called each time an item reaches the target, e.g. to bump the Items tab button.")]
        private UnityEvent onArrived;

        private Canvas targetCanvas;
        private readonly List<ItemAvatar> activeAvatars = new List<ItemAvatar>();

        protected void Start()
        {
            targetCanvas = target.GetComponentInParent<Canvas>(true);
            if (targetCanvas != null)
            {
                targetCanvas = targetCanvas.rootCanvas;
            }
        }

        protected void OnDisable()
        {
            // the animation coroutines die with this component, so their avatars have to go back to the pool here
            foreach (var avatar in activeAvatars)
                avatar.TryReturnToPoolAndDeactivate();
            activeAvatars.Clear();
        }

        /// <summary>
        /// Plays the animation for <paramref name="item"/>, found at <paramref name="origin"/> (world position).
        /// Several can play at once.
        /// </summary>
        public void Play(Item item, Vector3 origin)
        {
            ///
            var avatar = CurrentGeneralPool.TakeInstance(avatarPrototype, this);
            activeAvatars.Add(avatar);
            avatar.SetItem(item);
            avatar.transform.position = origin;
            avatar.transform.localScale = Vector3.zero;
            avatar.gameObject.SetActive(true);

            ///
            StartCoroutine(Animate(avatar, origin));
        }

        private IEnumerator Animate(ItemAvatar avatar, Vector3 origin)
        {
            var avatarTransform = avatar.transform;
            var normalScale = avatarPrototype.transform.localScale;

            // Appear
            onAppeared?.Invoke();
            var appearEnd = origin + Vector3.up * appearRise;
            for (var elapsed = 0f; elapsed < appearDuration; elapsed += Time.deltaTime)
            {
                var progress = elapsed / appearDuration;
                avatarTransform.position = Vector3.Lerp(origin, appearEnd, 1f - (1f - progress) * (1f - progress));
                avatarTransform.localScale = normalScale * appearScale.Evaluate(progress);
                yield return null;
            }
            avatarTransform.position = appearEnd;
            avatarTransform.localScale = normalScale * appearScale.Evaluate(1f);

            // Hover
            yield return new WaitForSeconds(hoverDuration);

            // Flight
            var flightStart = avatarTransform.position;
            var hoverScale = avatarTransform.localScale;
            for (var elapsed = 0f; elapsed < flightDuration; elapsed += Time.deltaTime)
            {
                var progress = flightProgress.Evaluate(elapsed / flightDuration);
                var flightEnd = GetTargetPosition(flightStart.z);
                avatarTransform.position = Vector3.LerpUnclamped(flightStart, flightEnd, progress)
                    + Vector3.up * (Mathf.Sin(progress * Mathf.PI) * flightArcHeight);
                avatarTransform.localScale = Vector3.LerpUnclamped(hoverScale, normalScale * flightEndScale, progress);
                avatar.SetNameAlpha(nameFadeDuration > 0f ? 1f - Mathf.Clamp01(elapsed / nameFadeDuration) : 0f);
                yield return null;
            }

            // Arrival
            activeAvatars.Remove(avatar);
            avatar.TryReturnToPoolAndDeactivate();
            onArrived?.Invoke();
        }

        /// <summary>
        /// The target's position seen by the gameplay camera, at depth <paramref name="z"/>.
        /// Read every frame, since the target can move (layout changes, screen resizes).
        /// </summary>
        private Vector3 GetTargetPosition(float z)
        {
            var targetPosition = target.position;
            if (targetCanvas != null)
            {
                var canvasCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(canvasCamera, targetPosition);
                var gameplayCamera = RunEntry.gameplayCamera;
                screenPoint.z = z - gameplayCamera.transform.position.z;
                targetPosition = gameplayCamera.ScreenToWorldPoint(screenPoint);
            }
            targetPosition.z = z;
            return targetPosition;
        }
    }

}
