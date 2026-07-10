using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace FH.Core.Architecture.Pool
{
    [DisallowMultipleComponent]
    public class MultiPrototypesPoolMemeberMonoBehavior<T> : MonoBehaviourWithInit, IMultiPrototypesPoolMember<T> where T : MultiPrototypesPoolMemeberMonoBehavior<T>
    {
        bool inPool = false;
        bool isPrototype;
        IMultiPrototypesPool<T> pool;
        int prototypeId;
        bool prototypeIdOverridden = false;

        #region IPoolMemberBase<IGreenPower>
        public bool IsInPool
        {
            get
            {
                return inPool;
            }

            set
            {
                inPool = value;
            }
        }
        public bool IsPrototype
        {
            get => isPrototype;
            set => isPrototype = value;
        }
        public int UsageId { get; set; }
        #endregion

        #region IMultiPrototypesPoolMember<IGreenPower>
        public IMultiPrototypesPool<T> Pool
        {
            get
            {
                return pool;
            }

            set
            {
                pool = value;
            }
        }

        public int PrototypeId
        {
            get
            {
                if (prototypeIdOverridden)
                {
                    return prototypeId;
                }
                else
                {
                    return GetEntityId().GetHashCode();
                }
            }

            set
            {
                prototypeIdOverridden = true;
                prototypeId = value;
            }
        }
        #endregion

        #region ICloneable<T>
        public T Clone()
        {
            MultiPrototypesPoolMemeberMonoBehavior<T> clone = Instantiate(this);
            clone.PrototypeId = PrototypeId;
            clone.prototypeIdOverridden = true;
            clone.OnClone();
            Assert.IsNotNull(clone as T);
            return clone as T;
        }
        #endregion

        public virtual void HandleGettingOutOfPool() { }
        public virtual void HandleGoingToPool() { }

        protected virtual void OnClone() { }

        protected void OnDestroy()
        {
            if (inPool
                && pool != null)
            {
                if (!IsPrototype)
                {
                    pool.RemoveInstance(this as T);
                }
                else
                {
                    pool.RemovePrototype(this as T);
                }
            }
            else
            {
                inPool = false;
            }
        }
    }
}
