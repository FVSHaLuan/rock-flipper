using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FH
{
    public class DeleteAllPlayerPrefs
    {
        [MenuItem("Delete All PlayerPrefs", menuItem = "FH/PlayerData/Delete All PlayerPrefs")]
        static void DeleteAllPlayPrefs()
        {
            ///
            if (!EditorUtility.DisplayDialog("Are you sure?", "Delete All PlayerPrefs", "ok", "cancel"))
            {
                return;
            }

            ///
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("Delete All Player Data", menuItem = "FH/PlayerData/Delete All Player Data", priority = 0)]
        static void DeleteAllPlayData()
        {
            ///
            if (!EditorUtility.DisplayDialog("Are you sure?", "Delete All Player Data", "ok", "cancel"))
            {
                return;
            }

            ///
            DeleteAllPlayPrefs();
            Core.Architecture.WritableData.WritableDataDeleteAll.DeleteAll();
        }
    }

}