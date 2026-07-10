using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FH.Core.Architecture.WritableData
{
    public class WritableDataDeleteAll
    {
        [MenuItem("Delete All WritableData", menuItem = "FH/PlayerData/Delete All WritableData")]
        public static void DeleteAll()
        {
            ///
            if (!EditorUtility.DisplayDialog("Are you sure?", "Delete All WritableData", "ok", "cancel"))
            {
                return;
            }

            ///
            DeleteAllForced();
        }

        [MenuItem("Reveal data folder", menuItem = "FH/PlayerData/Reveal data folder", priority = 999)]
        public static void RevealDataFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        public static void DeleteAllForced()
        {
            Debug.Log(string.Format("Deleted all files from {0}", Application.persistentDataPath));
            // FileUtil.DeleteFileOrDirectory(Application.persistentDataPath);
            DeleteFilesInDirectory(Application.persistentDataPath);
        }

        public static void DeleteFilesInDirectory(string directoryPath)
        {
            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Debug.Log($"Directory not found: {directoryPath}");
                return;
            }

            // Get all file paths in the directory
            string[] files = Directory.GetFiles(directoryPath);

            // Loop through each file and delete it
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Deleted file: {file}");
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Error deleting file {file}: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.LogError($"Unauthorized access when deleting file {file}: {ex.Message}");
                }
            }
        }
    }

}