using UnityEngine;
using System.Collections;

namespace FH.Core.Architecture.Pool
{
    public interface IMultiPrototypesPool<T> : IPoolBase<T> where T : IMultiPrototypesPoolMember<T>
    {
        void PushInstance(T memberInstance);
        bool RemoveInstance(T inPoolInstance);
        void PushPrototype(T memberPrototype);
        bool RemovePrototype(T memberPrototype);
        T TakeInstance(int prototypeId, bool forceCloning);
        bool ContainsPrototype(int prototypeId);
    }

}