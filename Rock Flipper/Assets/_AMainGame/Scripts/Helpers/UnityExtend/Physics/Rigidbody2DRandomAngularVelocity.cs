using FMod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DRandomAngularVelocity : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody2D;
    [SerializeField]
    private RandomFloat angularSpeedRange;

    public void Apply()
    {
        float angularVelocity = angularSpeedRange;
        angularVelocity *= Random.value < 0.5f ? 1 : -1;
        rigidbody2D.angularVelocity = angularVelocity;
    }
}
