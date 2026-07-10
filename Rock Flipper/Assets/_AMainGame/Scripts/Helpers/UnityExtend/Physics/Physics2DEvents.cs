using FH.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Physics2DEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject targetRigidbody;
    [SerializeField]
    private GameObject targetCollider;
    [SerializeField]
    private LayerMask targetLayerMask = ~0;

    [Space]
    [SerializeField]
    private OrderedEventDispatcher onCollisionEnter;
    [SerializeField]
    private OrderedEventDispatcher onCollisionStay;
    [SerializeField]
    private OrderedEventDispatcher onCollisionExit;

    [Space]
    [SerializeField]
    private OrderedEventDispatcher onTriggerEnter;
    [SerializeField]
    private OrderedEventDispatcher onTriggerStay;
    [SerializeField]
    private OrderedEventDispatcher onTriggerExit;

    private bool IsValidCollision(Collision2D collision)
    {
        ///
        if (!targetLayerMask.ContainsLayers(collision.collider.gameObject.layer))
        {
            return false;
        }

        ///
        if (targetRigidbody != null)
        {
            if (collision.rigidbody.gameObject != targetRigidbody)
            {
                return false;
            }
        }

        ///
        if (targetCollider != null)
        {
            if (collision.collider.gameObject != targetCollider)
            {
                return false;
            }
        }

        ///
        return true;
    }

    private bool IsValidCollision(Collider2D collision)
    {
        ///
        if (!targetLayerMask.ContainsLayers(collision.gameObject.layer))
        {
            return false;
        }

        ///
        if (targetRigidbody != null)
        {
            if (collision.attachedRigidbody.gameObject != targetRigidbody)
            {
                return false;
            }
        }

        ///
        if (targetCollider != null)
        {
            if (collision.gameObject != targetCollider)
            {
                return false;
            }
        }

        ///
        return true;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsValidCollision(collision))
        {
            onCollisionEnter?.Dispatch();
        }
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (IsValidCollision(collision))
        {
            onCollisionStay?.Dispatch();
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (IsValidCollision(collision))
        {
            onCollisionExit?.Dispatch();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidCollision(collision))
        {
            onTriggerEnter?.Dispatch();
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (IsValidCollision(collision))
        {
            onTriggerStay?.Dispatch();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidCollision(collision))
        {
            onTriggerExit?.Dispatch();
        }
    }
}
