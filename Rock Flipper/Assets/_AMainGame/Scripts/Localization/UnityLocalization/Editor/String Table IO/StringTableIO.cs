using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Localization
{
    public static class StringTableIO
    {
        [System.Serializable]
        private class SerializableLocalizedStringList
        {
            public List<SerializableLocalizedString> list;
        }

        public static string ExportToJSON(IEnumerable<LocalizedString> localizedStrings, LocaleExportOption localeExportOption)
        {
            ///
            List<SerializableLocalizedString> sls = new List<SerializableLocalizedString>();
            foreach (var localizedString in localizedStrings)
            {
                var sl = new SerializableLocalizedString(localizedString, localeExportOption);
                sls.Add(sl);
            }

            ///
            return JsonUtility.ToJson(new SerializableLocalizedStringList() { list = sls }, true);
        }

        public static void ImportFromJSON(string json)
        {
            var sls = JsonUtility.FromJson<SerializableLocalizedStringList>(json);

            ///
            foreach (var sl in sls.list)
            {
                sl.Import();
            }

            ///
            Debug.Log($"Imported {sls.list.Count} localized strings.");
        }

    }
}