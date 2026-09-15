#if UNITY_EDITOR
using System;
using System.Reflection;
using Unity.Hierarchy;
using UnityEditor;
using UnityEngine;

public static class EditorMenu
{
    [UnityEditor.MenuItem("FH/Agame/Select Skill Tree _1")]
    private static void SelectSkillTree()
    {
        // select the skill tree in the hierarchy
        var skillTree = GameObject.FindAnyObjectByType<Agame.Run.SkillTree>();
        if (skillTree != null)
        {
            Selection.activeGameObject = skillTree.gameObject;
        }
        else
        {
            Debug.LogWarning("No Skill Tree found in the scene.");
        }
    }
}

#endif