using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DSpeedLimiter : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;

    new private Rigidbody2D rigidbody2D;

    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

    protected void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        if (rigidbody2D.linearVelocity.sqrMagnitude > (MaxSpeed * MaxSpeed))
        {
            rigidbody2D.linearVelocity = rigidbody2D.linearVelocity.normalized * MaxSpeed;
        }
    }

}
