using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Agame.Localization
{
    public class LanguageSetterButton : MonoBehaviour
    {
        [SerializeField]
        private Locale locale;
        [SerializeField]
        private bool isAI;

        [Header("Editor")]
        [SerializeField]
        private TMP_Text buttonText;
        [SerializeField]
        private LocalizedString localizedLanguageName;
        [SerializeField]
        private LocalizedTmpFont localizedTmpFont;
        [SerializeField]
        private LocalizedMaterial localizedTmpFontMaterial;

        public void Set()
        {
            LocalizationSettings.SelectedLocale = locale;
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_SetButtonText")]
        private void Editor_SetButtonText()
        {
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Editor_SetButtonText");

            ///
            localizedLanguageName.LocaleOverride = locale;
            localizedTmpFont.LocaleOverride = locale;
            localizedTmpFontMaterial.LocaleOverride = locale;
            buttonText.text = localizedLanguageName.GetLocalizedString(locale) + (isAI ? " (AI)" : "");
            buttonText.font = localizedTmpFont.LoadAsset();
            buttonText.fontSharedMaterial = localizedTmpFontMaterial.LoadAsset();

            ///
            gameObject.name = $"Button_{locale.name}";

            ///
            buttonText.UpdateMeshPadding();
            buttonText.SetAllDirty();

            ///
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif
    }

}