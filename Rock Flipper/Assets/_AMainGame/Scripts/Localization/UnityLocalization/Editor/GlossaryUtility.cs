using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Agame.Localization
{
    public class GlossaryUtility : ScriptableObjectWithInit
    {
        [SerializeField, TextArea(10, 20)]
        private string outputText;

        [Space()]
        [SerializeField]
        private List<StringTableCollection> stringTableCollections;

        [ContextMenu("Print All English Terms")]
        private void PrintAllEnglishTerms()
        {
            outputText = "";
            //var collections = LocalizationEditorSettings.GetStringTableCollections();
            foreach (var collection in stringTableCollections)
            {
                var englishTable = collection.GetTable("en") as StringTable;
                if (englishTable == null)
                    continue;
                foreach (var entry in englishTable.Values)
                {
                    outputText += $"{entry.Value}\n";
                }
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);

            ///
            Debug.Log($"All English Terms:\n{outputText}");
        }
    }

}