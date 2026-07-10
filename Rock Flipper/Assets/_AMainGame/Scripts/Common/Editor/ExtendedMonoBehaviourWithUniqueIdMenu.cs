using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ExtendedMonoBehaviourWithUniqueIdMenu
{
    [MenuItem("CONTEXT/ExtendedMonoBehaviourWithUniqueId/Assign new unique Id")]
    private static void ContextMenu_CreatePreviewInstance(MenuCommand menuCommand)
    {
        (menuCommand.context as ExtendedMonoBehaviourWithUniqueId).Editor_AssignNewUniqueId();
    }
}
