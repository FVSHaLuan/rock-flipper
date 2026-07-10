using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using FH.Core.Architecture;
using UnityEngine.Rendering;

namespace FH.Core.Gameplay.HelperComponent
{
    public class MoveTo : OutsiteTargetTransform
    {
        public event System.Action OnComplete;
        public event System.Action OnUpdate;

        [Space]
        [SerializeField]
        private PositionProvider destination;
        [SerializeField]
        private bool zStay = false;
        [SerializeField]
        private bool upVectorPointToDirection;

        [Space]
        [SerializeField]
        private float duration;
        [SerializeField]
        private float maxAdditionalDuration = 0;
        [SerializeField]
        private float speed = -1;
        [SerializeField]
        private float deltaTimeAcceleration = 0;



        [Space]
        [SerializeField]
        private bool smoothStep = false;

        [Space]
        [SerializeField]
        private TimeScaleMode timeScaleMode = TimeScaleMode.ScaledTime;

        [Space]
        [SerializeField]
        private UnityEvent onComplete = new UnityEvent();

        private int scheduledMoveFrame;
        private bool isMoving = false;
        private IPositionProvider positionProvider;
        private float progress;

        public bool PausingFlag { get; set; }
        public bool IsMoving => isMoving;
        public float Progress
        {
            get
            {
                if (!IsMoving)
                {
                    return 0;
                }
                else
                {
                    return progress;
                }
            }
        }

        public IPositionProvider Destination
        {
            get
            {
                ///
                if (destination != null)
                {
                    return destination;
                }

                ///
                return positionProvider;
            }
            set
            {
                positionProvider = value;
                destination = null;
            }
        }

        public void SetStartPosition(Vector3 position)
        {
            TargetPosition = position;
        }

        public void StartMove()
        {
            ///
            if (!isActiveAndEnabled)
            {
                scheduledMoveFrame = Time.frameCount;
                return;
            }

            ///
            if (isMoving)
            {
                return;
            }

            ///
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            ///         
            isMoving = true;

            ///
            var startPos = TargetPosition;

            ///
            if (speed > 0)
            {
                duration = Vector2.Distance(startPos, Destination.Position) / speed;
            }

            ///
            var effectiveDuration = duration + Random.Range(0, maxAdditionalDuration);

            ///
            float t = 0;
            float deltaTimeSpeed = 0;
            while (t <= effectiveDuration)
            {
                ///
                while (PausingFlag)
                {
                    yield return null;
                }

                ///
                var deltaTime = GetDeltaTime();
                deltaTimeSpeed += deltaTimeAcceleration * deltaTime;
                var effecttiveDeltaTime = deltaTime + deltaTime * deltaTimeSpeed;

                ///
                t += effecttiveDeltaTime;

                ///
                Vector3 newPos;
                var step = t / effectiveDuration;
                if (smoothStep)
                {
                    ///
                    var smoothStep = Mathf.SmoothStep(0, 1, step);
                    newPos = Vector3.Lerp(startPos, Destination.Position, smoothStep);

                    ///
                    progress = smoothStep;
                }
                else
                {
                    ///
                    newPos = Vector3.Lerp(startPos, Destination.Position, step);

                    ///
                    progress = step;
                }

                ///
                if (zStay)
                {
                    newPos.z = startPos.z;
                }

                ///
                if (upVectorPointToDirection)
                {
                    TargetTransform.up = newPos - startPos;
                }

                ///
                TargetPosition = newPos;

                ///
                OnUpdate?.Invoke();

                ///
                yield return null;
            }

            // put to destination
            var finalPos = Destination.Position;
            if (zStay)
            {
                finalPos.z = startPos.z;
            }
            TargetPosition = finalPos;

            ///
            isMoving = false;

            ///
            onComplete.Invoke();
            OnComplete?.Invoke();
        }

        private float GetDeltaTime()
        {
            if (timeScaleMode == TimeScaleMode.ScaledTime)
            {
                return Time.deltaTime;
            }
            else
            {
                return Entry.Instance.timeScaleManager.GetDeltaTime(timeScaleMode);
            }
        }

        public void MoveImmediately()
        {
            TargetPosition = Destination.Position;
        }

        protected void OnEnable()
        {
            isMoving = false;
            PausingFlag = false;

            ///
            if (scheduledMoveFrame == Time.frameCount)
            {
                StartMove();
            }
        }
    }

}