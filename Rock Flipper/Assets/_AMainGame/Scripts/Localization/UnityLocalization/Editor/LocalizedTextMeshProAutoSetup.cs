using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.Localization.PropertyVariants.TrackedObjects;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Tables;

namespace Agame.Localization
{
    public static class LocalizedTextMeshProAutoSetup
    {
#if UNITY_EDITOR

        [MenuItem("CONTEXT/TMP_Text/Setup Localization")]
        private static void Localize(MenuCommand command)
        {
            var tmpText = command.context as TMP_Text;
            var originalText = tmpText.text;
            var localizer = tmpText.GetComponent<GameObjectLocalizer>();
            if (localizer == null)
            {
                localizer = Undo.AddComponent<GameObjectLocalizer>(tmpText.gameObject);
            }

            ///
            Undo.RecordObject(localizer, "Setup Localization");

            ///
            var trackedObject = localizer.GetTrackedObject<TrackedUGuiGraphic>(tmpText, true);
            trackedObject.GetTrackedProperty<LocalizedStringProperty>("m_text", true);
            trackedObject.GetTrackedProperty<LocalizedAssetProperty>("m_fontAsset", true).LocalizedObject = UnityLocalizationHelperSettings.Instance.DefaultLocalizedTmpFont;
            trackedObject.GetTrackedProperty<LocalizedAssetProperty>("m_sharedMaterial", true).LocalizedObject = UnityLocalizationHelperSettings.Instance.DefaultLocalizedTmpFontMaterial;

            ///
            EditorUtility.SetDirty(localizer);

            ///
            Debug.Log($"Setup Localization for {tmpText.name} with original text: {originalText}");
            GUIUtility.systemCopyBuffer = originalText;
            Debug.Log($"Copied original text to clipboard!");

            ///
            SearchForLocalizedTerms(localizer);
        }

        [MenuItem("CONTEXT/GameObjectLocalizer/Search For Localized Terms")]
        private static void SearchForLocalizedTerms(MenuCommand command)
        {
            ///
            var localizer = command.context as GameObjectLocalizer;

            ///
            SearchForLocalizedTerms(localizer);
        }

        [MenuItem("CONTEXT/GameObjectLocalizer/Create Localized String Entry")]
        private static void CreateLocalizedStringEntry(MenuCommand command)
        {
            ///
            var localizer = command.context as GameObjectLocalizer;

            ///
            var tmpText = localizer.GetComponent<TMP_Text>();

            ///
            if (tmpText == null)
            {
                Debug.LogError("The selected object is not a TMP_Text component.");
                return;
            }

            ///
            var originalText = tmpText.text;

            ///
            var trackedUGuiGraphic = localizer.GetTrackedObject<TrackedUGuiGraphic>(tmpText, true);
            var trackedProperty = trackedUGuiGraphic.GetTrackedProperty<LocalizedStringProperty>("m_text", true);

            ///
            if (trackedProperty.LocalizedString == null
                || trackedProperty.LocalizedString.TableReference.ReferenceType == TableReference.Type.Empty)
            {
                Debug.LogError("Specify Table Collection first");
                return;
            }

            ///
            if (!trackedProperty.LocalizedString.IsEmpty)
            {
                Debug.LogError($"Localized String Entry already exists: {trackedProperty.LocalizedString.TableReference.TableCollectionName}/{trackedProperty.LocalizedString.TableEntryReference.KeyId} for {tmpText.name}");
                return;
            }

            ///
            var key = LocalizationUtility.GenerateKey(originalText);
            if (LocalizationUtility.CreateEnglishEntry(trackedProperty.LocalizedString.TableReference.TableCollectionName, key, originalText, out var keyId))
            {
                Undo.RecordObject(localizer, "Create Localized String Entry");
                trackedProperty.LocalizedString.TableEntryReference = keyId;
                EditorUtility.SetDirty(localizer);
                Debug.Log($"Created Localized String Entry: {trackedProperty.LocalizedString.TableReference.TableCollectionName}/{key} for {tmpText.name}");
            }
            else
            {
                Debug.LogError($"Failed to create Localized String Entry: {trackedProperty.LocalizedString.TableReference.TableCollectionName}/{key} for {tmpText.name}. Entry already exists.");
            }
        }

        private static void SearchForLocalizedTerms(GameObjectLocalizer localizer)
        {
            var tmpText = localizer.GetComponent<TMP_Text>();

            ///
            if (tmpText == null)
            {
                Debug.LogError("The selected object is not a TMP_Text component.");
                return;
            }

            ///
            var originalText = tmpText.text;

            ///
            LocalizedStringPicker.ShowPicker(originalText,
                (LocalizedString localizedString) =>
                {
                    Undo.RecordObject(localizer, "Assign Localized String");

                    ///
                    var trackedUGuiGraphic = localizer.GetTrackedObject<TrackedUGuiGraphic>(tmpText, true);
                    UpdateLocalizedStringProperty(trackedUGuiGraphic, localizedString);

                    ///
                    Debug.Log($"Assigned Localized String: {localizedString.TableReference.TableCollectionName}/{localizedString.TableEntryReference.KeyId} to {tmpText.name}");

                    ///
                    EditorUtility.SetDirty(localizer);
                });
        }


        private static void UpdateLocalizedStringProperty(TrackedUGuiGraphic trackedUGuiGraphic, LocalizedString newLocalizedString)
        {
            var trackedProperty = trackedUGuiGraphic.GetTrackedProperty<LocalizedStringProperty>("m_text", true);
            if (trackedProperty.LocalizedString == null)
            {
                trackedProperty.LocalizedString = newLocalizedString;
            }
            else
            {
                trackedProperty.LocalizedString.TableReference = newLocalizedString.TableReference;
                trackedProperty.LocalizedString.TableEntryReference = newLocalizedString.TableEntryReference;
            }
        }
#endif
    }

}