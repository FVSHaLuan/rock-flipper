using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace FH.Core.Architecture.Pool
{
    public abstract class MultiPrototypesPool<T> : IMultiPrototypesPool<T> where T : IMultiPrototypesPoolMember<T>
    {
        Dictionary<int, T> prototypesDictionary = new Dictionary<int, T>();
        Dictionary<int, List<T>> instancesDictionary = new Dictionary<int, List<T>>();
        Dictionary<int, ValueHandle<int>> activeMembersCount = new Dictionary<int, ValueHandle<int>>(); // Keeps track of how many memebers are currently not in the pool

        private int lastUsageId = int.MinValue;

        public int LastUsageId => lastUsageId;

        #region IMultiPrototypesPool<T>
        public bool ContainsPrototype(int prototypeId)
        {
            return prototypesDictionary.ContainsKey(prototypeId);
        }

        public virtual void PushInstance(T memberInstance)
        {
            int prototypeId = memberInstance.PrototypeId;
            Assert.IsTrue(instancesDictionary.ContainsKey(prototypeId));
            memberInstance.Pool = this;
            memberInstance.IsInPool = true;
            instancesDictionary[prototypeId].Add(memberInstance);

            ///
            memberInstance.HandleGoingToPool();

            ///
            DecreaseActiveMemberCount(memberInstance.PrototypeId);
        }

        public void PushPrototype(T memberPrototype)
        {
            int prototypeId = memberPrototype.PrototypeId;
            Assert.IsFalse(instancesDictionary.ContainsKey(prototypeId));
            prototypesDictionary.Add(prototypeId, memberPrototype);
            instancesDictionary.Add(prototypeId, new List<T>());
            memberPrototype.IsInPool = true;
            memberPrototype.IsPrototype = true;
        }

        public bool RemovePrototype(T memberPrototype)
        {
            int prototypeId = memberPrototype.PrototypeId;
            return instancesDictionary.Remove(prototypeId);
        }

        public virtual T TakeInstance(int prototypeId, bool forceCloning = true)
        {
            Assert.IsTrue(instancesDictionary.ContainsKey(prototypeId), string.Format("prototypeId: {0}", prototypeId));

            ///
            T instance;

            ///
            if (instancesDictionary[prototypeId].Count > 0)
            {
                instance = TakeInstanceAvailableInDictionary(prototypeId);
            }
            else
            {
                if (forceCloning)
                {
                    instance = TakeInstanceByClonning(prototypeId);
                }
                else
                {
                    throw new Exception("Pool memebers are not available to take");
                }
            }

            ///
            IncreaseActiveMemberCount(prototypeId);

            ///
            IResuable poolMember = instance as IResuable;
            if (poolMember != null)
            {
                poolMember.UsageId = ++lastUsageId;
            }

            ///
            instance.IsPrototype = false;

            ///
            instance.HandleGettingOutOfPool();

            ///
            return instance;
        }

        public bool RemoveInstance(T inPoolInstance)
        {
            ///
            if (!inPoolInstance.IsInPool)
            {
                return false;
            }

            ///
            var prototypeId = inPoolInstance.PrototypeId;

            ///
            List<T> instanceList;

            ///
            if (instancesDictionary.TryGetValue(prototypeId, out instanceList))
            {
                return instanceList.Remove(inPoolInstance);
            }

            ///
            return false;
        }
        #endregion

        public ValueHandle<int> GetActiveMembersCountValueHandle(int prototype)
        {
            ValueHandle<int> valueHandle;

            ///
            if (!activeMembersCount.TryGetValue(prototype, out valueHandle))
            {
                valueHandle = new ValueHandle<int>();
                activeMembersCount.Add(prototype, valueHandle);
            }

            ///
            return valueHandle;
        }

        private void IncreaseActiveMemberCount(int prototypeId)
        {
            GetActiveMembersCountValueHandle(prototypeId).Value++;
        }

        private void DecreaseActiveMemberCount(int prototypeId)
        {
            var valueHandle = GetActiveMembersCountValueHandle(prototypeId);

            ///
            valueHandle.Value--;

            ///
            if (valueHandle.Value < 0)
            {
                valueHandle.Value = 0;
            }
        }

        private T TakeInstanceAvailableInDictionary(int prototypeId)
        {
            Assert.IsTrue(instancesDictionary[prototypeId].Count > 0);
            List<T> list = instancesDictionary[prototypeId];
            int lastIndex = list.Count - 1;
            T instance = list[lastIndex];
            (instance as IMultiPrototypesPoolMember<T>).IsInPool = false;
            list.RemoveAt(lastIndex);
            return instance;
        }

        private T TakeInstanceByClonning(int prototypeId)
        {
            ///
            T instance = (prototypesDictionary[prototypeId] as ICloneable<T>).Clone();
            (instance as IMultiPrototypesPoolMember<T>).Pool = this;
            (instance as IMultiPrototypesPoolMember<T>).IsInPool = false;

            ///
            return instance;
        }
    }

}