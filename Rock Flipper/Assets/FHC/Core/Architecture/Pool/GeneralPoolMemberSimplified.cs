using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Architecture.Pool
{
    public class GeneralPoolMemberSimplified : MultiPrototypesPoolMemeberMonoBehavior<GeneralPoolMemberSimplified>
    {
        public object RequestedObject { get; set; }

        public virtual bool TryReturnToPool()
        {
            if (Pool != null && !IsInPool)
            {
                ///
                Pool.PushInstance(this);

                ///
                RequestedObject = null;

                ///
                return true;
            }
            else
            {
                return false;
            }

        }

        [ContextMenu("TryReturnToPoolAndDeactivate"), PlayModeOnly]
        public virtual bool TryReturnToPoolAndDeactivate()
        {
            ///
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                ///
                Debug.LogError("only works while playing");

                ///
                return false;
            }
#endif

            ///
            if (TryReturnToPool())
            {
                ///
                gameObject.SetActive(false);

                ///
                return true;
            }

            ///
            return false;
        }

        public void TryReturnToPoolAndDeactivateVoid()
        {
            TryReturnToPoolAndDeactivate();
        }

        public void TryReturnToPoolAndDeactivateVoidDelay(float delay)
        {
            StartCoroutine(TryReturnToPoolAndDeactivateVoid(delay));
        }

        private IEnumerator TryReturnToPoolAndDeactivateVoid(float delay)
        {
            yield return new WaitForSeconds(delay);
            TryReturnToPoolAndDeactivateVoid();
        }
    }
}