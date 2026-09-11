using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Localization
{
    public class LocalizedStringsExporter : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<LocalizedString> exportingItems;
        [SerializeField]
        private LocaleExportOption localeExportOption;

        [Space]
        [SerializeField, TextArea(5, 10), ReadOnly]
        private string output;

        [Header("Test")]
        [SerializeField]
        private LocalizedString ls;
        [SerializeField]
        private SerializableLocalizedString serializedLocalizedString;

        [ContextMenu("Test")]
        private void Test()
        {
            serializedLocalizedString = new SerializableLocalizedString(ls, localeExportOption);
        }

        [ContextMenu("Export")]
        private void Export()
        {
            output = StringTableIO.ExportToJSON(exportingItems, localeExportOption);
            Debug.Log(output);
        }
    }

}