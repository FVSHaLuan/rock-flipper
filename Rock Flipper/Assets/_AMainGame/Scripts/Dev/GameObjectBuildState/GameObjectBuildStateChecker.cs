using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public class GameObjectBuildStateChecker : MonoBehaviour
    {
#if UNITY_EDITOR
        private List<GameObjectBuildState> toCheckObjects = new List<GameObjectBuildState>();

        protected void Awake()
        {
            Check();
        }

        private void GetObjects()
        {
            GetComponentsInChildren(true, toCheckObjects);
        }

        private void Check()
        {
            ///
            GetObjects();

            ///
            foreach (var item in toCheckObjects)
            {
                if (item.gameObject.activeSelf != item.DefaultActiveState)
                {
                    Debug.LogErrorFormat(item.gameObject, "GameObject {0} is at wrong active state", item.gameObject.name);
                }
            }
        } 
#endif
    }

}