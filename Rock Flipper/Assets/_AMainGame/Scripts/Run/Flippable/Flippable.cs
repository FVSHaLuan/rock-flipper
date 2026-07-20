using UnityEngine;

namespace Agame.Run.Combat
{
    public class Flippable : ExtendedMonoBehaviourRun
    {
        private const int LandingPositionMaxTrialCount = 20;

        public event System.Action OnStartedFlipping;
        public event System.Action OnFinishedFlipping;
        public event System.Action OnUpdatedFlipping;
        public event System.Action OnUpdatedProgress;

        [SerializeField]
        private float baseDuration = 2f;
        [SerializeField]
        private float baseFlippingDistance = 0.2f;
        [SerializeField]
        private float baseFlippingHeight = 2f;

        [Space]
        [SerializeField]
        private float playfieldPadding = 0.5f;

        private float flippingDuration = 1;
        private float flippingTimeElapsed;
        private Vector2 flippingStartPosition;
        private Vector2 flippingEndPosition;

        public bool IsFlipping { get; private set; }
        public float FlippingProgress { get; private set; }
        public float FlippingHeightProgress { get; private set; }
        public Vector2 FlippingGroundPosition { get; private set; }

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

        public void ForceFlipping(float duration, Vector2 landingPosition, float height)
        {
            ///
            ResetFlippingState();

            ///
            StartFlipping(duration, landingPosition, height);
        }

        private void GetRandomFlippingParameters(out float duration, out Vector2 landingPosition, out float height)
        {
            ///
            duration = baseDuration;

            ///            
            landingPosition = GetRandomLandingPosition();

            ///
            height = baseFlippingHeight;
        }

        private Vector2 GetRandomLandingPosition()
        {
            ///
            float distance = baseFlippingDistance;

            ///
            var clampedPosition = (Vector2)Playfield.Clamp(transform.position, playfieldPadding * Vector2.one);

            ///
            Playfield.GetBounds(out var min, out var max, playfieldPadding);

            ///
            int trialCount = 0;
            Vector2 landingPosition = Playfield.CenterPoint;

            ///
            while (trialCount <= LandingPositionMaxTrialCount)
            {
                ///
                float directionX = Random.Range(-1f, 1f);
                float directionY = Random.Range(-1f, 1f);

                ///
                if (trialCount == LandingPositionMaxTrialCount)
                {
                    if (clampedPosition.x - min.x < distance)
                        directionX = Mathf.Abs(directionX);
                    else if (max.x - clampedPosition.x < distance)
                        directionX = -Mathf.Abs(directionX);

                    if (clampedPosition.y - min.y < distance)
                        directionY = Mathf.Abs(directionY);
                    else if (max.y - clampedPosition.y < distance)
                        directionY = -Mathf.Abs(directionY);
                }

                ///
                Vector2 direction = new Vector2(directionX, directionY).normalized;
                landingPosition = clampedPosition + direction * distance;

                // Validation: Check if the landing position is within the playfield bounds
                if (landingPosition.x >= min.x && landingPosition.x <= max.x &&
                    landingPosition.y >= min.y && landingPosition.y <= max.y)
                {
                    break;
                }

                ///
                trialCount++;
            }

            ///
            return landingPosition;
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

            ///
            OnStartedFlipping?.Invoke();
        }

        protected void OnDisable()
        {
            ResetFlippingState();
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
            OnUpdatedFlipping?.Invoke();

            ///
            if (flippingTimeElapsed >= flippingDuration)
            {
                ResetFlippingState();

                ///
                OnFinishedFlipping?.Invoke();
            }
        }

        private void ResetFlippingState()
        {
            IsFlipping = false;
            flippingTimeElapsed = 0;
            FlippingProgress = 0;
            FlippingHeightProgress = 0;
        }

        private void UpdateFlippingPosition()
        {
            ///
            FlippingHeightProgress = Mathf.Sin(FlippingProgress * Mathf.PI);
            var height = FlippingHeightProgress * baseFlippingHeight;

            ///
            FlippingGroundPosition = Vector2.Lerp(flippingStartPosition, flippingEndPosition, FlippingProgress);

            ///
            var th = transformHandle;
            th.position = FlippingGroundPosition + Vector2.up * height;
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