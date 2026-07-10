using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BT.Dev
{
    public static class GameObjectBuildStateMenu
    {
        [MenuItem("CONTEXT/GameObjectBuildState/Correct Name")]
        private static void CorrectName(MenuCommand menuCommand)
        {
            (menuCommand.context as GameObjectBuildState).Editor_CorrectName();
        }
    }
}