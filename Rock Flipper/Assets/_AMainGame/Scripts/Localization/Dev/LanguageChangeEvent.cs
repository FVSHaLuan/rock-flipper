using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Localization
{
    public class LanguageChangeEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onChangedToEnglish;
        [SerializeField]
        private UnityEvent onChangedToOtherLanguage;
        [SerializeField]
        private UnityEvent onChangedLanguage;

        protected void OnDisable()
        {
            ///
            LocalizationManager.OnLocalizeEvent -= LocalizationManager_OnLocalizeEvent;
        }

        protected void OnEnable()
        {
            ///
            InvokeEvents();

            ///
            LocalizationManager.OnLocalizeEvent += LocalizationManager_OnLocalizeEvent;
        }

        private void LocalizationManager_OnLocalizeEvent()
        {
            InvokeEvents();
        }

        private void InvokeEvents()
        {
            ///
            onChangedLanguage?.Invoke();

            ///
            if (LocalizationManager.CurrentLanguage.ToLower().Contains("english"))
            {
                onChangedToEnglish?.Invoke();
            }
            else
            {
                onChangedToOtherLanguage?.Invoke();
            }
        }
    }

}