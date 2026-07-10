using UnityEngine;

namespace BT.Run.Combat
{
    public class RotateAccordingToMovement : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed = 360f;

        private Vector3 _previousPosition;

        private void OnEnable()
        {
            _previousPosition = transform.position;
        }

        private void LateUpdate()
        {
            RotateAccordingToMovementDirection();
        }

        private void RotateAccordingToMovementDirection()
        {
            Vector3 movementDirection = transform.position - _previousPosition;

            if (movementDirection.sqrMagnitude > 0.0001f) // Check if there is significant movement
            {
                float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg - 90f;
                float currentAngle = transform.eulerAngles.z;
                float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
                float maxRotationThisFrame = rotationSpeed * Time.deltaTime;
                float clampedAngleChange = Mathf.Clamp(angleDifference, -maxRotationThisFrame, maxRotationThisFrame);
                float newAngle = currentAngle + clampedAngleChange;
                transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            }

            _previousPosition = transform.position;
        }
    }
}