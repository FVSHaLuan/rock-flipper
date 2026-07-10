using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public static class UniqueIdValidator
    {
        private static Dictionary<int, List<Object>> intDictionary = new Dictionary<int, List<Object>>();
        private static Dictionary<string, List<Object>> stringDictionary = new Dictionary<string, List<Object>>();

        public static bool Validate(int id, Object unityObject)
        {
            return Validate(intDictionary, id, unityObject);
        }

        public static bool Validate(string id, Object unityObject)
        {
            return Validate(stringDictionary, id, unityObject);
        }

        public static bool RemoveEntry(string id, Object unityObject)
        {
            return RemoveEntry(stringDictionary, id, unityObject);
        }

        public static bool RemoveEntry(int id, Object unityObject)
        {
            return RemoveEntry(intDictionary, id, unityObject);
        }

        private static bool Validate<T>(Dictionary<T, List<Object>> dictionary, T id, Object unityObject)
        {
            ///
            if (unityObject == null)
            {
                return true;
            }

            ///
            List<Object> list = null;

            ///
            if (dictionary.ContainsKey(id))
            {
                list = dictionary[id];
            }
            else
            {
                dictionary[id] = list = new List<Object>();
            }

            // Trim null items, item with different id, itself
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null || list[i] == unityObject)
                {
                    list.RemoveAt(i);
                }
            }

            ///
            if (list.Count != 0)
            {
                ///
                if (list.Count == 1)
                {
                    Debug.LogErrorFormat(list[0], "{0} has duplicated Id", list[0].name);
                }

                ///
                Debug.LogErrorFormat(unityObject, "{0} has duplicated Id", unityObject.name);

                ///
                list.Add(unityObject);

                ///
                return false;
            }

            ///
            list.Add(unityObject);

            ///
            return true;
        }

        private static bool RemoveEntry<T>(Dictionary<T, List<Object>> dictionary, T id, Object unityObject)
        {
            ///
            if (unityObject == null)
            {
                return true;
            }

            ///
            List<Object> list = null;

            ///
            if (dictionary.ContainsKey(id))
            {
                list = dictionary[id];
            }
            else
            {
                return false;
            }

            ///
            return list.Remove(unityObject);
        }
    }
}
