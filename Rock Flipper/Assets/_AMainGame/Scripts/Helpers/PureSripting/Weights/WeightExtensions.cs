using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    public static class WeightExtensions
    {
        public static T PickOne<T>(this List<T> weightedFlexibleList, float totalWeight) where T : IWeightedFlexible
        {
            ///
            float totalFixedWeight = 0f;
            float totalFlexibleWeight = 0f;
            bool hasFlexible = false;

            ///
            int maxNotNullIndex = -1;

            ///
            for (int i = 0; i < weightedFlexibleList.Count; i++)
            {
                ///
                var item = weightedFlexibleList[i];

                ///
                if (item == null)
                {
                    continue;
                }

                maxNotNullIndex = i;

                ///
                if (item.IsFlexible)
                {
                    ///
                    totalFlexibleWeight += item.Weight;

                    ///
                    hasFlexible = true;
                }
                else
                {
                    ///
                    totalFixedWeight += item.Weight;
                }
            }

            if (maxNotNullIndex < 0)
            {
                throw new System.ArgumentNullException("All items of the list are null!");
            }

            ///
            float effectiveFlexibleWeight = totalWeight - totalFixedWeight;
            if (effectiveFlexibleWeight < 0)
            {
                effectiveFlexibleWeight = 0f;
            }

            ///
            var maxRandomValue = hasFlexible ? Mathf.Max(totalWeight, totalFixedWeight) : totalFixedWeight;
            var randomValue = Random.Range(0, maxRandomValue);

            ///
            float currentRange = 0;
            for (int i = 0; i < weightedFlexibleList.Count; i++)
            {
                ///
                var item = weightedFlexibleList[i];

                ///
                if (i == maxNotNullIndex)
                {
                    return item;
                }

                ///
                if (!item.IsFlexible)
                {
                    currentRange += item.Weight;
                }
                else
                {
                    currentRange += item.Weight / totalFlexibleWeight * effectiveFlexibleWeight;
                }

                ///
                if (randomValue <= currentRange)
                {
                    return item;
                }
            }

            ///
            throw new System.Exception("T PickOne<T>(this List<T> weightedFlexibleList, float totalWeight) couldn't return anything");
        }

        /// <summary>
        /// Pick a random weighted item without removing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weightedList"></param>
        /// <returns></returns>
        public static T PickOneIn<T>(IRandomGenerator random, params T[] weightedList) where T : IWeighted
        {
            ///
            if (weightedList.Length == 0)
            {
                throw new System.ArgumentException();
            }

            ///
            float weightSum = 0;
            for (int i = 0; i < weightedList.Length; i++)
            {
                weightSum += weightedList[i].Weight;
            }

            ///
            float randomValue = random.Range(0, weightSum);

            ///           
            float currentWeight = 0;
            for (int i = 0; i < weightedList.Length; i++)
            {
                currentWeight += weightedList[i].Weight;
                if (currentWeight > randomValue || i == (weightedList.Length - 1))
                {
                    return weightedList[i];
                }
            }

            ///
            throw new System.Exception();
        }

        /// <summary>
        /// Pick a random weighted item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weightedList"></param>
        /// <returns></returns>
        public static T PickOne<T>(this List<T> weightedList, IRandomGenerator random, bool willRemove = false) where T : IWeighted
        {
            return PickOne(weightedList, out _, random, willRemove);
        }

        /// <summary>
        /// Pick a random weighted item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weightedList"></param>
        /// <returns></returns>
        public static T PickOne<T>(this List<T> weightedList, out int itemIndex, IRandomGenerator random, bool willRemove = false) where T : IWeighted
        {
            return PickOne(weightedList, out itemIndex, ignoredIndex: -1, random, willRemove);
        }

        /// <summary>
        /// Pick a random weighted item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weightedList"></param>
        /// <returns></returns>
        public static T PickOne<T>(this List<T> weightedList, out int itemIndex, int ignoredIndex, IRandomGenerator random, bool willRemove = false) where T : IWeighted
        {
            ///
            if (weightedList.Count == 0)
            {
                throw new System.ArgumentException();
            }

            ///
            float weightSum = 0;
            for (int i = 0; i < weightedList.Count; i++)
            {
                ///
                if (i == ignoredIndex)
                {
                    continue;
                }

                ///
                weightSum += weightedList[i].Weight;
            }

            ///
            float randomValue = random.Range(0, weightSum);

            ///           
            float currentWeight = 0;
            T result;
            for (int i = 0; i < weightedList.Count; i++)
            {
                ///
                if (i == ignoredIndex)
                {
                    continue;
                }

                ///
                currentWeight += weightedList[i].Weight;
                if (currentWeight >= randomValue || i == (weightedList.Count - 1))
                {
                    ///
                    result = weightedList[i];

                    ///
                    if (willRemove)
                    {
                        weightedList.RemoveAt(i);
                        itemIndex = -1;
                    }
                    else
                    {
                        itemIndex = i;
                    }

                    ///
                    return result;
                }
            }

            ///
            throw new System.Exception();
        }
    }

}