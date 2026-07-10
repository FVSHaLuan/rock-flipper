using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DFreezer : MonoBehaviourWithInit
{
    [SerializeField]
    private bool freezeLocalPosition;

    private bool isFreezing;
    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    protected new Rigidbody2D rigidbody2D;
    private Vector3 savedLocalPosition;

    protected override bool Init()
    {
        ///
        rigidbody2D = GetComponent<Rigidbody2D>();

        ///
        return base.Init();
    }

    public void StartFreezing()
    {
        ///
        Init();

        ///
        if (isFreezing)
        {
            return;
        }

        ///
        isFreezing = true;

        // Save
        savedVelocity = rigidbody2D.linearVelocity;
        savedAngularVelocity = rigidbody2D.angularVelocity;
        savedLocalPosition = transform.localPosition;

        ///
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.linearVelocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0;
    }

    public void Unfreezing()
    {
        ///
        if (!isFreezing)
        {
            return;
        }

        ///
        isFreezing = false;

        ///
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.linearVelocity = savedVelocity;
        rigidbody2D.angularVelocity = savedAngularVelocity;
    }

    protected void Update()
    {
        if (isFreezing)
        {
            ///
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0;

            ///
            if (freezeLocalPosition)
            {
                transform.localPosition = savedLocalPosition;
            }
        }
    }
}
