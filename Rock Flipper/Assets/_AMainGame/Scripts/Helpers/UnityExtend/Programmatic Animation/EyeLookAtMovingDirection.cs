using UnityEngine;
using UnityEngine;

public class EyeLookAtMovingDirection : MonoBehaviour
{
    [SerializeField]
    private Transform moverTransform;
    [SerializeField]
    private float maxSpeed=5;
    [SerializeField]
    private float maxEyeDistance = 0.5f;

    private Vector3 _previousPosition;

    private void OnEnable()
    {
        _previousPosition = moverTransform.position;
    }

    private void LateUpdate()
    {
        UpdateEyeDirection();
    }

    private void UpdateEyeDirection()
    {
        Vector3 movement = moverTransform.position - _previousPosition;
        Vector3 velocity = movement / Time.deltaTime;
        float speed = velocity.magnitude;

        if (speed > 0.0001f)
        {
            Vector3 direction = velocity / speed;
            float distance = Mathf.Clamp(speed, 0f, maxSpeed) / maxSpeed * maxEyeDistance;
            transform.localPosition = direction * distance;
        }

        _previousPosition = moverTransform.position;
    }
}
