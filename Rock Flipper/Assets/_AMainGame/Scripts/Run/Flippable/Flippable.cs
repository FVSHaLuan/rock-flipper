using UnityEngine;

namespace Agame.Run.Combat
{
    public class Flippable : ExtendedMonoBehaviourRun
    {
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
            height =baseFlippingHeight;
        }

        private void StartFlipping(float duration, Vector2 landingPosition, float height)
        {
            ///
            IsFlipping = true;
            flippingTimeElapsed = 0;

            ///
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
            UpdateFlippingPosition();

            ///
            if (flippingTimeElapsed >= flippingDuration)
            {
                IsFlipping = false;
                flippingTimeElapsed = 0;
                FlippingProgress = 0;
            }
        }

        private void UpdateFlippingPosition()
        {
            var th = transformHandle;
            th.position = Vector2.Lerp(flippingStartPosition, flippingEndPosition, FlippingProgress);
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