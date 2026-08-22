using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBody2DVelocitySetter : MonoBehaviour
{
    [SerializeField]
    new private Rigidbody2D rigidbody2D;
    [SerializeField]
    private Vector2 velocity;

    public void Set()
    {
        rigidbody2D.linearVelocity = velocity;
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }
#endif
}
