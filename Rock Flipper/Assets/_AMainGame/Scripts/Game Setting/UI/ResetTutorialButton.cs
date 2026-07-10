using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BT.UI.GameSettings
{
    [RequireComponent(typeof(Button), typeof(SelectedObjectFallback))]
    public class ResetTutorialButton : ExtendedMonoBehaviour
    {
        protected void Start()
        {
            var button = GetComponent<Button>();

            ///
            button.onClick.AddListener(OnButtonClicked);
        }

        protected void OnEnable()
        {
            GetComponent<Button>().interactable = gameSetting.PassedTutorialCount > 0;
        }

        private void OnButtonClicked()
        {
            var button = GetComponent<Button>();
            button.interactable = false;

            ///
            gameSetting.ResetTutorial();
            entry.gameSettingObject.SaveData();

            ///
            GetComponent<SelectedObjectFallback>().Fallback();
        }
    }

}