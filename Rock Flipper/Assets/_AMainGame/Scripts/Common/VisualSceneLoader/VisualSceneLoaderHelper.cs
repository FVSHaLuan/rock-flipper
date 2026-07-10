using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Run
{
    public class VisualSceneLoaderHelper : ExtendedMonoBehaviour
    {
        [SerializeField]
        private GameScene gameScene;

        public void Load()
        {
            entry.visualSceneLoader.Load(gameScene);
        }
    }

}