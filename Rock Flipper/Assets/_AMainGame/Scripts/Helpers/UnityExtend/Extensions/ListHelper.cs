using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GD
{
    public static class ListHelper
    {
        public delegate T GetObjectUniqueIdFunction<T, U>(U obj);

        public static Dictionary<T, U> ToDictionary<T, U>(this List<U> list, GetObjectUniqueIdFunction<T, U> getObjectUniqueIdFunction)
        {
            Dictionary<T, U> dictionary = new Dictionary<T, U>();

            foreach (var item in list)
            {
                dictionary.Add(getObjectUniqueIdFunction(item), item);
            }

            return dictionary;
        }

        public static Dictionary<T, U> ToDictionary<T, U>(this List<U> list) where U : IUnique<T>
        {
            return list.ToDictionary((U u) => { return u.UniqueId; });
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable == null) || (enumerable.Count() == 0);
        }

        /// <summary>
        ///Null list to an empty list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void CorrectNullList<T>(ref List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }
        }

        /// <summary>
        /// Get random item, delete it in the list and return value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T TakeRandomItem<T>(this List<T> list)
        {
            ///
            if (list.Count <= 0)
            {
                throw new System.Exception();
            }

            ///
            int index = Random.Range(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);

            ///
            return item;
        }

        /// <summary>
        /// Get random item across lists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="remove">should remove the item after picked?</param>
        /// <param name="lists"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static T GetRandomItem<T>(bool remove, params List<T>[] lists)
        {
            ///
            var item = GetRandomItem(out var list, out var index, lists);

            ///
            if (remove && index >= 0)
            {
                list.RemoveAt(index);
            }

            ///
            return item;
        }

        /// <summary>
        /// Get a random item without removing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this HashSet<T> set)
        {
            return set.ElementAt(Random.Range(0, set.Count));
        }

        /// <summary>
        /// Get random item across lists without removing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lists"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static T GetRandomItem<T>(params List<T>[] lists)
        {
            return GetRandomItem(out _, out _, lists);
        }

        /// <summary>
        /// Get random item across lists without removing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pickedList"></param>
        /// <param name="pickedIndex"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static T GetRandomItem<T>(out List<T> pickedList, out int pickedIndex, params List<T>[] lists)
        {
            ///
            int totalItemCount = 0;

            ///
            foreach (var item in lists)
            {
                totalItemCount += item.Count;
            }

            ///
            if (totalItemCount <= 0)
            {
                pickedList = null;
                pickedIndex = -1;
                return default(T);
            }

            ///
            var index = Random.Range(0, totalItemCount);
            var currentBound = 0;

            ///
            for (int i = 0; i < lists.Length; i++)
            {
                ///
                var effectiveIndex = index - currentBound;

                ///
                currentBound += lists[i].Count;

                ///
                if (index < currentBound)
                {
                    pickedList = lists[i];
                    pickedIndex = effectiveIndex;
                    return pickedList[pickedIndex];
                }
            }

            ///
            pickedList = null;
            pickedIndex = -1;
            return default(T);
        }

        /// <summary>
        /// Get random item without remove it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this T[] ts)
        {
            ///
            if (ts.Length == 0)
            {
                throw new System.Exception();
            }

            ///
            return ts[Random.Range(0, ts.Length)];
        }

        /// <summary>
        /// Get random item without removing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> ts)
        {
            ///
            if (ts.Count == 0)
            {
                throw new System.Exception();
            }

            ///
            return ts[Random.Range(0, ts.Count)];
        }

        /// <summary>
        /// Get random item without removing it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> ts, IRandomGenerator random)
        {
            ///
            if (ts.Count == 0)
            {
                throw new System.Exception();
            }

            ///
            return ts[random.Range(0, ts.Count)];
        }

        /// <summary>
        /// If count < srcList.Count -> All items are unique. Otherwise, there will be duplication
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="srclist"></param>
        /// <param name="count"></param>
        /// <param name="desList"></param>
        public static void GetRandomItemsDuplicatable<T>(this IList<T> srcList, int count, IList<T> desList)
        {
            ///
            desList.Clear();

            ///
            if (count == 0
                || srcList == null
                || srcList.Count == 0)
            {
                return;
            }

            ///
            var wholeDuplication = count / srcList.Count;
            var uniqueCount = count % srcList.Count;

            ///
            for (int i = 0; i < wholeDuplication; i++)
            {
                for (int j = 0; j < srcList.Count; j++)
                {
                    desList.Add(srcList[j]);
                }
            }

            ///
            desList.Shuffle(UnityRandom.Default);

            ///
            srcList.GetRandomItems(UnityRandom.Default, desList, uniqueCount);
        }

        /// <summary>
        /// divides original list into segments and picks a random item in each segments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static IEnumerable<T> GetRandomItems<T>(this IList<T> ts, int count)
        {
            ///
            if (ts == null)
            {
                throw new System.Exception();
            }

            ///
            if (count == 0)
            {
                yield break;
            }

            ///
            if (ts.Count < count)
            {
                throw new System.Exception();
            }

            ///
            int interval = ts.Count / count;

            ///
            int startId = 0;
            for (int i = 0; i < count; i++)
            {
                ///
                int endId = (i == count - 1) ? ts.Count - 1 : startId + interval - 1;
                var pickedId = Random.Range(startId, endId + 1);

                ///
                yield return ts[pickedId];

                ///
                startId = endId + 1;
            }
        }

        /// <summary>
        /// Using recursive algorithm, suitable for small count.
        /// This won't clear targetList, only adds new items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="targetList"></param>
        /// <param name="count"></param>
        public static void GetRandomItems<T>(this IList<T> ts, IRandomGenerator random, IList<T> targetList, int count)
        {
            ts.GetRandomItems(0, ts.Count - 1, random, targetList, count);
        }

        /// <summary>
        /// Using recursive algorithm, suitable for small count
        /// This won't clear targetList, only adds new items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="targetList"></param>
        /// <param name="count"></param>
        public static void GetRandomItems<T>(this IList<T> ts, int startIndex, int endIndex, IRandomGenerator random, IList<T> targetList, int count)
        {
            ///
            if (count == 0)
            {
                return;
            }

            ///
            if (ts == null)
            {
                throw new System.ArgumentNullException(nameof(ts));
            }

            ///
            if (targetList == null)
            {
                throw new System.ArgumentNullException(nameof(targetList));
            }

            ///
            if (endIndex < startIndex)
            {
                throw new System.ArgumentNullException("startIndex and endIndex", "endIndex must < startIndex");
            }

            ///
            int sourceCount = endIndex - startIndex + 1;

            ///
            if (sourceCount <= count)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    targetList.Add(ts[i]);
                }
            }
            else
            {
                ///
                int randomIndex = random.Range(startIndex, endIndex + 1);
                targetList.Add(ts[randomIndex]);

                ///
                if (count == 1)
                {
                    return;
                }

                ///
                var leftCountMax = randomIndex - startIndex;
                var rightCountMax = endIndex - randomIndex;

                ///
                Mathg.DivideValueRandomly(count - 1, leftCountMax, rightCountMax, out var leftCount, out var rightCount, random);

                ///
                if (startIndex < randomIndex)
                {
                    ts.GetRandomItems(startIndex, randomIndex - 1, random, targetList, leftCount);
                }
                if (randomIndex < endIndex)
                {
                    ts.GetRandomItems(randomIndex + 1, endIndex, random, targetList, rightCount);
                }
            }
        }

        /// <summary>
        /// Move data from srcList to desList in a random order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="srcList"></param>
        /// <param name="desList"></param>
        /// <param name="forceNull"></param>
        public static void ShuffleMoveList<T>(ref List<T> srcList, ref List<T> desList, bool forceNull = false)
        {
            ///
            if (srcList == null)
            {
                ///
                if (forceNull)
                {
                    desList = null;
                }
                else
                {
                    desList?.Clear();
                }

                ///
                return;
            }

            ///
            if (desList == null)
            {
                desList = new List<T>();
            }
            else
            {
                desList.Clear();
            }

            ///
            while (srcList.Count > 0)
            {
                ///
                var value = srcList.TakeRandomItem();

                ///
                desList.Add(value);
            }
        }

        public static void CopyList<T>(ref List<T> srcList, ref List<T> desList, bool forceNull = false)
        {
            ///
            if (srcList == null)
            {
                ///
                if (forceNull)
                {
                    desList = null;
                }
                else
                {
                    desList?.Clear();
                }

                ///
                return;
            }

            ///
            if (desList == null)
            {
                desList = new List<T>();
            }
            else
            {
                desList.Clear();
            }

            ///
            for (int i = 0; i < srcList.Count; i++)
            {
                desList.Add(srcList[i]);
            }
        }

        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            ///
            if (list == null)
            {
                return true;
            }

            ///
            if (list.Count == 0)
            {
                return true;
            }

            ///
            return false;
        }

        public static void Shuffle<T>(this IList<T> list, IRandomGenerator random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle<T>(this IList<T> list, int startId, int endId)
        {
            int n = endId + 1;
            while (n > (startId + 1))
            {
                n--;
                int k = Random.Range(startId, endId + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetItemWithClampedIndex<T>(this List<T> ts, int index)
        {
            ///
            return ts[Mathf.Clamp(index, 0, ts.Count - 1)];
        }

        public static T GetItemWithClampedIndex<T>(this T[] ts, int index)
        {
            ///
            return ts[Mathf.Clamp(index, 0, ts.Length - 1)];
        }

        public static bool HasDuplicate<T>(this List<T> list)
        {
            ///
            if (list == null)
            {
                return false;
            }

            ///
            return list.Count != list.Distinct().Count();
        }
    }
}
