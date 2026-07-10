using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Agame.Dev
{
    public static class EditorExtensions
    {
        [MenuItem("FH/Print Object Names")]
        private static void PrintObjectNames()
        {
            ///
            StringBuilder sb = new StringBuilder();

            ///
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                sb.AppendLine(Selection.gameObjects[i].name);
            }

            ///
            Debug.Log(sb.ToString());
        }
    }

}