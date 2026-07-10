using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public abstract class SpecificCollisionEvents : MonoBehaviour
    {
        [SerializeField]
        private TagCompareMode tagCompareMode = TagCompareMode.Rigidbody;

        [Space]
        [SerializeField]
        protected UnityEvent<Collider2D> onEnter;
        [SerializeField]
        protected UnityEvent<Collider2D> onExit;

        private enum TagCompareMode
        {
            Rigidbody = 0,
            Collider = 1
        }

        protected abstract string TargetTag { get; }
        protected virtual bool ShouldHandleOnStay => false;

        protected virtual void OnStay(Collider2D collider2D) { }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (CompareTag(collision))
            {
                onEnter?.Invoke(collision.collider);
            }
        }

        protected void OnCollisionExit2D(Collision2D collision)
        {
            if (CompareTag(collision))
            {
                onExit?.Invoke(collision.collider);
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (CompareTag(collision))
            {
                onEnter?.Invoke(collision);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (CompareTag(collision))
            {
                onExit?.Invoke(collision);
            }
        }

        protected void OnCollisionStay2D(Collision2D collision)
        {
            ///
            if (!ShouldHandleOnStay)
            {
                return;
            }

            ///
            if (CompareTag(collision))
            {
                OnStay(collision.collider);
            }
        }

        protected void OnTriggerStay2D(Collider2D collision)
        {
            ///
            if (!ShouldHandleOnStay)
            {
                return;
            }

            ///
            if (CompareTag(collision))
            {
                OnStay(collision);
            }
        }

        private bool CompareTag(Collider2D collider2D)
        {
            if (tagCompareMode == TagCompareMode.Rigidbody)
            {
                return collider2D.attachedRigidbody.CompareTag(TargetTag);
            }
            else if (tagCompareMode == TagCompareMode.Collider)
            {
                return collider2D.CompareTag(TargetTag);
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        private bool CompareTag(Collision2D collision2D)
        {
            if (tagCompareMode == TagCompareMode.Rigidbody)
            {
                return collision2D.rigidbody.CompareTag(TargetTag);
            }
            else if (tagCompareMode == TagCompareMode.Collider)
            {
                return collision2D.collider.CompareTag(TargetTag);
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
    }

}