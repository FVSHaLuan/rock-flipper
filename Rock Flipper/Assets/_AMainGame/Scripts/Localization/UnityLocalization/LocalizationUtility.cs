#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;
#endif

namespace Agame.Localization
{
    public static class LocalizationUtility
    {
        private const int MaxKeyLength = 70;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="entryName"></param>
        /// <param name="englishText"></param>
        /// <returns>true if the entry was newly successfully created, false if entry already exists or failed to create</returns>
        public static bool CreateEnglishEntry(string collectionName, string entryName, string englishText, out long keyId)
        {
#if UNITY_EDITOR
            var collection = LocalizationEditorSettings.GetStringTableCollection(collectionName);
            var englishTable = collection.GetTable("en") as StringTable;
            var entry = englishTable.GetEntry(entryName);
            if (entry != null)
            {
                keyId = entry.KeyId;
                return false;
            }
            else
            {
                entry = englishTable.AddEntry(entryName, englishText);
                keyId = entry.KeyId;
            }

            ///
            EditorUtility.SetDirty(englishTable);
            EditorUtility.SetDirty(englishTable.SharedData);

            ///
            return true;
#else
            ///
            keyId = 0;
            return false; 
#endif
        }

        public static string GenerateKey(string originalText)
        {
            if (string.IsNullOrWhiteSpace(originalText))
                throw new System.Exception("Cannot generate key from empty or whitespace text.");
            var key = originalText.StripFormatAll().RemoveAllNonalphanumericCharacters();
            if (key.Length > MaxKeyLength)
            {
                key = key.Substring(0, MaxKeyLength);
            }
            return key;
        }
    }

}