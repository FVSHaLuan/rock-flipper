using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    public class SaveGameOnSystemMenuScreen : ExtendedMonoBehaviour
    {
        [SerializeField]
        private GameObject savableViewObject;
        [SerializeField]
        private GameObject unsavableViewObject;

        protected void OnEnable()
        {
            ///
            var playerDataSaver = entry.playerDataSaver;

            ///
            if (playerDataSaver.IsSavable)
            {
                ///
                savableViewObject.SetActive(true);
                unsavableViewObject.SetActive(false);

                ///
                playerDataSaver.SetSaveThisFrame();
            }
            else
            {
                ///
                savableViewObject.SetActive(false);
                unsavableViewObject.SetActive(true);
            }
        }
    }
}