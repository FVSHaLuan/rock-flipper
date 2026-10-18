using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Agame.Home
{
    public class LanguageSelectScreen : ExtendedMonoBehaviourHome
    {
        private Locale savedLocale;

        protected void OnEnable()
        {
            savedLocale = LocalizationSettings.SelectedLocale;
        }

        private void OnDisable()
        {
            if (savedLocale != LocalizationSettings.SelectedLocale)
            {
                entry.visualSceneLoader.Load(Entry.ActiveGameScene);
            }
        }
    }

}