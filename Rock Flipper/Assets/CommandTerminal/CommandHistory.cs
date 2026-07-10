using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CommandTerminal
{
    public class CommandHistory
    {
        List<string> history = new List<string>();
        int position;

        public CommandHistory()
        {
            LoadHistory();
        }

        public void Push(string command_string)
        {
            if (command_string == "")
            {
                return;
            }

            ///
            if (history != null
                && history.Count > 0
                && history.Last().ToUpper().Trim() == command_string.ToUpper().Trim())
            {
                return;
            }

            ///
            history.Add(command_string);
            position = history.Count;

            ///
            SaveHistory();
        }

        public string Next()
        {
            position++;

            if (position >= history.Count)
            {
                position = history.Count;
                return "";
            }

            return history[position];
        }

        public string Previous()
        {
            if (history.Count == 0)
            {
                return "";
            }

            position--;

            if (position < 0)
            {
                position = 0;
            }

            return history[position];
        }

        public void Clear()
        {
            history.Clear();
            position = 0;
            SaveHistory();
        }

        private void SaveHistory()
        {
            var key = GetHistoryKey();
            string serialized = string.Join("\n", history.ToArray());
#if UNITY_EDITOR
            EditorPrefs.SetString(key, serialized);
#else
            PlayerPrefs.SetString(key, serialized);
#endif
        }

        private void LoadHistory()
        {
            var key = GetHistoryKey();
            string serialized = null;

            ///
#if UNITY_EDITOR
            if (EditorPrefs.HasKey(key))
            {
                serialized = EditorPrefs.GetString(key);
            }
#else
            if (PlayerPrefs.HasKey(key))
            {
                serialized = PlayerPrefs.GetString(key);
            }
#endif

            ///
            if (serialized != null)
            {
                history = new List<string>(serialized.Split('\n'));
                position = history.Count;
            }
        }

        private string GetHistoryKey()
        {
#if UNITY_EDITOR
            return PlayerSettings.productGUID + ".Terminal.History";
#else
            return "Terminal.History";
#endif
        }
    }
}
