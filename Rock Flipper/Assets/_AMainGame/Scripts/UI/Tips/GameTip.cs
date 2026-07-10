using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.Tips
{
    public class GameTip : MonoBehaviour
    {
        [SerializeField]
        private bool showAtLoadingScreen;
        [SerializeField]
        private bool showOnGameTipsScreen = true;

        public bool ShowAtLoadingScreen => showAtLoadingScreen;
    }

}