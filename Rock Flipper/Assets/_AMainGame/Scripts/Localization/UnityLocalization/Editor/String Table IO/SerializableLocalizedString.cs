using AutoGroupGenerator;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Tables;

namespace Agame.Localization
{
    [System.Serializable]
    public class SerializableLocalizedString
    {
        [SerializeField]
        private string tableCollectionName;
        [SerializeField]
        private string key;
        [SerializeField]
        private string sharedComment;
        [SerializeField]
        private List<LocaleStringPair> localizedStrings;

        public SerializableLocalizedString(LocalizedString localizedString, LocaleExportOption localeExportOption)
        {
            ///
            if (localizedString.IsEmpty)
            {
                throw new System.ArgumentException("LocalizedString is empty.");
            }

            ///
            var collection = LocalizationEditorSettings.GetStringTableCollection(localizedString.TableReference);
            tableCollectionName = collection.TableCollectionName;
            var englishTable = collection.GetTable("en") as StringTable;
            var entry = englishTable.GetEntryFromReference(localizedString.TableEntryReference);
            key = entry.Key;
            sharedComment = entry.SharedEntry?.Metadata?.GetMetadata<Comment>()?.CommentText;
            localizedStrings = new List<LocaleStringPair>();

            // import English
            var englishComment = entry.GetMetadata<Comment>()?.CommentText;
            var englishLocalizedString = new LocaleStringPair("en", entry.Value, englishComment);
            localizedStrings.Add(englishLocalizedString);

            ///
            HashSet<string> processedLocales = new HashSet<string>();
            processedLocales.Add("en");

            // other languages
            foreach (var table in collection.StringTables)
            {
                ///
                if (table.LocaleIdentifier.Code == "en")
                {
                    continue;
                }

                ///
                processedLocales.Add(table.LocaleIdentifier.Code.ToLower());

                ///
                var otherEntry = table.GetEntryFromReference(localizedString.TableEntryReference);
                if (otherEntry == null)
                {
                    var otherLocalizedStringEmpty = new LocaleStringPair(table.LocaleIdentifier.Code, string.Empty, string.Empty);
                    localizedStrings.Add(otherLocalizedStringEmpty);
                    continue;
                }

                ///
                if (localeExportOption == LocaleExportOption.UntranslatedOnly && !string.IsNullOrWhiteSpace(otherEntry.Value))
                {
                    continue;
                }

                ///
                var otherComment = otherEntry.GetMetadata<Comment>()?.CommentText;
                var otherLocalizedString = new LocaleStringPair(table.LocaleIdentifier.Code, otherEntry.Value, otherComment);
                localizedStrings.Add(otherLocalizedString);
            }

            // iterate through all locales in the project to find any that were not processed
            foreach (var locale in LocalizationEditorSettings.GetLocales())
            {
                ///
                if (processedLocales.Contains(locale.Identifier.Code.ToLower()))
                {
                    continue;
                }
                ///
                var otherLocalizedStringEmpty = new LocaleStringPair(locale.Identifier.Code, string.Empty, string.Empty);
                localizedStrings.Add(otherLocalizedStringEmpty);
            }

        }

        public bool Import()
        {
            var collection = LocalizationEditorSettings.GetStringTableCollection(tableCollectionName);
            foreach (var localizedString in localizedStrings)
            {
                // Ignore English
                if (localizedString.localeCode == "en"
                    || string.IsNullOrWhiteSpace(localizedString.value))
                {
                    continue;
                }

                var table = collection.GetTable(localizedString.localeCode) as StringTable;
                var entry = table.GetEntryFromReference(key);
                if (entry != null && !string.IsNullOrWhiteSpace(entry.Value))
                {
                    continue;
                }

                ///
                if (entry == null)
                {
                    entry = table.AddEntry(key, localizedString.value);
                }
                else
                {
                    ///
                    entry.Value = localizedString.value;
                }

                ///
                EditorUtility.SetDirty(table);
                EditorUtility.SetDirty(collection);
            }

            ///
            return true;
        }
    }
}