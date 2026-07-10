using OneLine;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace BSB.UISystems
{
    public class MiniConsolePlayerContext : ScriptableObjectWithInit
    {
        [SerializeField, OneLineWithHeader]
        private List<ExternalAssets> externalAssets = new List<ExternalAssets>();

        [System.Serializable]
        private struct ExternalAssets
        {
            public string key;
            public UnityEngine.TextAsset textAsset;
        }

        public virtual void GetImportingLines(string key, List<string> lines)
        {
            lines.Clear();

            ///
            if (externalAssets == null || externalAssets.Count == 0)
            {
                return;
            }

            ///
            var ea = externalAssets.Find(x => x.key == key);
            if (ea.textAsset == null)
            {
                return;
            }

            ///
            using (StringReader stringReader = new StringReader(ea.textAsset.text))
            {
                string line = null;
                do
                {
                    line = stringReader.ReadLine();
                    if (line != null)
                    {
                        lines.Add(line);
                    }
                } while (line != null);
            }
        }
    }

}