using UnityEngine;

public class CarMover : MonoBehaviour
{
    public event System.Action OnReachedTarget;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float turningRadius = 2f;
    [SerializeField]
    private float targetReachedThreshold = 0.1f; // Threshold distance to consider the target reached

    public virtual Vector2 Target { get; set; }
    public bool IsPaused { get; set; }

    private void LateUpdate()
    {
        if (IsPaused)
        {
            return;
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector2 currentPosition = transform.position;
        Vector2 directionToTarget = (Target - currentPosition).normalized;

        // Check if the target is reached
        if (Vector2.Distance(currentPosition, Target) <= targetReachedThreshold)
        {
            OnReachedTarget?.Invoke();
            return;
        }

        // Calculate the angle to the target
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Calculate the maximum rotation allowed this frame
        float maxRotationThisFrame = speed / turningRadius * Mathf.Rad2Deg * Time.deltaTime;
        float clampedAngleChange = Mathf.Clamp(angleDifference, -maxRotationThisFrame, maxRotationThisFrame);

        // Apply the rotation
        float newAngle = currentAngle + clampedAngleChange;
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

        // Move forward in the direction the car is facing
        Vector3 forwardMovement = transform.up * speed * Time.deltaTime;
        transform.position += forwardMovement;
    }
}
