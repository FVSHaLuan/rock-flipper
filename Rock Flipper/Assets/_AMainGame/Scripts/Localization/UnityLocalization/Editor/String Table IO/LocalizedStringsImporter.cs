using UnityEngine;

namespace Agame.Localization
{
    public class LocalizedStringsImporter : ScriptableObjectWithInit
    {
        [Space]
        [SerializeField, TextArea(5, 10), ReadOnly]
        private string intput;

        [ContextMenu("Import")]
        private void Import()
        {
            StringTableIO.ImportFromJSON(intput);
        }
    }

}