using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    [DisallowMultipleComponent]
    public class GameObjectBuildState : MonoBehaviour, IGameObjectBuildStateSetter
    {
        [SerializeField]
        private bool defaultActiveState;
        [SerializeField]
        private bool includedInConfiguration = true;
        [SerializeField]
        private string gameObjectName;

        public bool DefaultActiveState => defaultActiveState;
        public bool HasCorrectGameObjectName => gameObjectName == gameObject.name;

        public bool IncludedInConfiguration => includedInConfiguration;

        public void Reset()
        {
            defaultActiveState = gameObject.activeSelf;
            gameObjectName = gameObject.name;
        }

        public bool SetBuildState()
        {
            if (HasCorrectGameObjectName)
            {
                ///
                gameObject.SetActive(DefaultActiveState);

                ///
                return true;
            }
            else
            {
                ///
                Debug.LogErrorFormat(gameObject, "Incorrect Object - {0}", gameObject.name);

                ///
                return false;
            }
        }

#if UNITY_EDITOR
        public void Editor_CorrectName()
        {
            UnityEditor.Undo.RecordObject(this, "Correct Name");
            gameObjectName = gameObject.name;
        }
#endif
    }
}