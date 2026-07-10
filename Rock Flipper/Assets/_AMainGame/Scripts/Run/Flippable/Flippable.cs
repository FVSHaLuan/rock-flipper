using UnityEngine;

namespace Agame.Run.Combat
{
    public class Flippable : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private float baseDuration = 2f;
        [SerializeField]
        private float baseRange = 3f;
        [SerializeField]
        private float baseMinDistance = 0.2f;
        [SerializeField]
        private float baseMaxHeight = 2f;
        [SerializeField]
        private float baseMinHeight = 0.5f;

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
            float randomDistance = Random.Range(baseMinDistance, baseRange);
            landingPosition = Random.insideUnitCircle.normalized * randomDistance + (Vector2)transform.position;

            ///
            height = Random.Range(baseMinHeight, baseMaxHeight);
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

        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Try Flipping"), PlayModeOnly]
        private void Editor_TryFlipping()
        {

        }
#endif
    }

}