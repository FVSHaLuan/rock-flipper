using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public class DevStatManager : MonoBehaviour
    {
        public static DevStatManager Instance { get; private set; }

        public GameObject fps;
        public GameObject enemyCount;

        protected void Awake()
        {
            Instance = this;
        }
    }

}