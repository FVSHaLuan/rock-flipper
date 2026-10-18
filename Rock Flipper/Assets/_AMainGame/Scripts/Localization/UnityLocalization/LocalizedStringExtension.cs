using System.Globalization;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Agame.Localization
{
    public static class LocalizedStringExtension
    {
        public static bool IsValidAndNotEmpty(this UnityEngine.Localization.LocalizedString localizedString)
        {
            if (localizedString == null)
            {
                return false;
            }

            ///
            if (localizedString.IsEmpty)
            {
                return false;
            }

            ///
            return true;
        }

        /// <summary>
        /// The localized string in upper case, following the selected locale's casing rules (e.g. Turkish dotted İ).
        /// </summary>
        public static string GetLocalizedStringUpper(this UnityEngine.Localization.LocalizedString localizedString)
        {
            var culture = LocalizationSettings.SelectedLocale?.Identifier.CultureInfo ?? CultureInfo.InvariantCulture;
            return localizedString.GetLocalizedString().ToUpper(culture);
        }

        /// <summary>
        /// The localized string if <paramref name="localizedString"/> is set, else <paramref name="plainString"/>.
        /// </summary>
        public static string GetLocalizedStringOr(this UnityEngine.Localization.LocalizedString localizedString, string plainString)
        {
            return localizedString.IsValidAndNotEmpty() ? localizedString.GetLocalizedString() : plainString;
        }

#if UNITY_EDITOR
        /// <summary>
        /// The entry's English text, read from the table collection in the editor. Null if the entry doesn't exist.
        /// </summary>
        public static string Editor_GetEnglishString(this UnityEngine.Localization.LocalizedString localizedString)
        {
            if (!localizedString.IsValidAndNotEmpty())
            {
                return null;
            }

            ///
            var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(localizedString.TableReference);
            var englishTable = collection != null ? collection.GetTable("en") as UnityEngine.Localization.Tables.StringTable : null;
            var entry = englishTable != null ? englishTable.GetEntryFromReference(localizedString.TableEntryReference) : null;
            return entry?.Value;
        }
#endif
    }

}
