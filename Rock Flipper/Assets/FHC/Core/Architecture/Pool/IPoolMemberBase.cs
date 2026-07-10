using UnityEngine;
using System.Collections;

namespace FH.Core.Architecture.Pool
{
    public interface IPoolMemberBase<T> : ICloneable<T>, IResuable
    {
        /// <summary>
        /// Should only be set by IPool
        /// </summary>
        bool IsInPool { get; set; }

        /// <summary>
        /// Should only be set by IPool
        /// </summary>
        bool IsPrototype { get; set; }
    }

}