using UnityEngine;

namespace FH.Core.Architecture.Pool
{
    public abstract class GeneralPoolMemberSimplifiedHandler<T> : GeneralPoolMemberSimplified
    {
        public abstract T TargetObject { get; }
    }

}