using FMod;
using OneLine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotationBySpeed : MonoBehaviour
{
    [SerializeField, OneLineWithHeader]
    private float maxAngularSpeed = 360;
    [SerializeField, Min(0.00001f)]
    private float maxLinearSpeed = 10;

    private new Rigidbody2D rigidbody2D;
    private float currentMaxAngularSpeed;

    protected void Awake()
    {
        RandomizeCurrentMaxAngularSpeed();

        ///
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void LateUpdate()
    {
        var speed = rigidbody2D.linearVelocity.magnitude;

        ///
        if (Mathf.Approximately(rigidbody2D.angularVelocity, 0))
        {
            RandomizeCurrentMaxAngularSpeed();
        }

        ///
        rigidbody2D.angularVelocity = Mathf.Lerp(0, currentMaxAngularSpeed, speed / maxLinearSpeed);
    }

    private void RandomizeCurrentMaxAngularSpeed()
    {
        currentMaxAngularSpeed = maxAngularSpeed * (Random.value >= 0.5f ? 1 : -1);
    }
}
