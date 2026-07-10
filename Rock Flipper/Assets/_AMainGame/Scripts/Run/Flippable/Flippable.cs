using UnityEngine;

namespace Agame.Run.Combat
{
    public class Flippable : ExtendedMonoBehaviourRun
    {
        public event System.Action OnUpdatedFlipping;
        public event System.Action OnUpdatedProgress;

        [SerializeField]
        private float baseDuration = 2f;
        [SerializeField]
        private float baseFlippingDistance = 0.2f;
        [SerializeField]
        private float baseFlippingHeight = 2f;

        private float flippingDuration = 1;
        private float flippingTimeElapsed;
        private Vector2 flippingStartPosition;
        private Vector2 flippingEndPosition;

        public bool IsFlipping { get; private set; }
        public float FlippingProgress { get; private set; }
        public float HeightProgress { get; private set; }

        public bool TryFlipping()
        {
            ///
            if (IsFlipping)
                return false;

            ///
            GetRandomFlippingParameters(out var duration, out var landingPosition, out var height);
            StartFlipping(duration, landingPosition, height);

            ///
            return true;
        }

        private void GetRandomFlippingParameters(out float duration, out Vector2 landingPosition, out float height)
        {
            ///
            duration = baseDuration;

            ///
            float distance = baseFlippingDistance;
            landingPosition = Random.insideUnitCircle.normalized * distance + (Vector2)transform.position;

            ///
            height = baseFlippingHeight;
        }

        private void StartFlipping(float duration, Vector2 landingPosition, float height)
        {
            ///
            IsFlipping = true;
            flippingTimeElapsed = 0;

            ///
            flippingDuration = duration;
            flippingStartPosition = transform.position;
            flippingEndPosition = landingPosition;
        }

        protected void Update()
        {
            if (!IsFlipping)
            {
                return;
            }

            ///
            UpdateFlipping();
        }

        private void UpdateFlipping()
        {
            ///
            flippingTimeElapsed += Time.deltaTime;
            FlippingProgress = Mathf.Clamp01(flippingTimeElapsed / flippingDuration);

            ///
            OnUpdatedProgress?.Invoke();

            ///
            UpdateFlippingPosition();

            ///
            if (flippingTimeElapsed >= flippingDuration)
            {
                IsFlipping = false;
                flippingTimeElapsed = 0;
                FlippingProgress = 0;
                HeightProgress = 0;
            }

            ///
            OnUpdatedFlipping?.Invoke();
        }

        private void UpdateFlippingPosition()
        {
            ///
            HeightProgress = Mathf.Sin(FlippingProgress * Mathf.PI);
            var height = HeightProgress * baseFlippingHeight;

            ///
            var th = transformHandle;
            th.position = Vector2.Lerp(flippingStartPosition, flippingEndPosition, FlippingProgress) + Vector2.up * height;
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Try Flipping"), PlayModeOnly]
        private void Editor_TryFlipping()
        {
            Debug.Log($"Try flipping: {TryFlipping()}");
        }
#endif
    }

}