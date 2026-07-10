using UnityEngine;
using System.Collections;

namespace FH.Core.Architecture.Pool
{
    public interface IMultiPrototypesPoolMember<T> : IPoolMemberBase<T> where T : IMultiPrototypesPoolMember<T>
    {
        /// <summary>
        /// Should only be set by IPool
        /// </summary>
        public IMultiPrototypesPool<T> Pool { get; set; }
        public int PrototypeId { get; set; }

        public void HandleGettingOutOfPool();
        public void HandleGoingToPool();
    }
}
