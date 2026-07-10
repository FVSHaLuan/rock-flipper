using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.GameplayHelpers
{
    public class UIScreenToggle : MonoBehaviour
    {
        [SerializeField]
        private UIScreen uiScreen;

        public void Toggle()
        {
            if (uiScreen.IsScreenActive)
            {
                uiScreen.gameObject.SetActive(false);
            }
            else
            {
                uiScreen.ShowPopup();
            }
        }
    }

}