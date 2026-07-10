using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public class TellADevPopup : MonoBehaviour
    {
        [SerializeField]
        private UnifiedText messageText;

        public void Show(string errorMessage)
        {
            ///
            messageText.Text = errorMessage;

            ///
            gameObject.SetActive(true);
        }
    }

}