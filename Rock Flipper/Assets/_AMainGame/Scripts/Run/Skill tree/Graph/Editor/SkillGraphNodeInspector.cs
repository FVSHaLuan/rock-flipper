using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace Agame.Run
{
    [CustomEditor(typeof(SkillGraphNode))]
    public class SkillGraphNodeInspector : GlobalNodeEditor
    {
        // EditorUtility.OpenPropertyEditor doesn't return the window it creates and always
        // spawns a new one, so we find any already-open one by title to focus instead of duplicating it.
        private static EditorWindow FindOpenPropertyEditorWindow(string title)
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            foreach (var window in windows)
            {
                if (window.GetType().Name == "PropertyEditor" && window.titleContent.text == title)
                {
                    return window;
                }
            }

            return null;
        }

        protected override void DrawTopButtons()
        {
            ///
            base.DrawTopButtons();

            ///
            var skillGraphNode = target as SkillGraphNode;

            ///
            if (skillGraphNode == null)
            {
                return;
            }

            ///
            if (GUILayout.Button("Fill costs", GUILayout.Height(40)))
            {
                skillGraphNode.Editor_FillCosts();
            }

            ///
            using (new EditorGUI.DisabledGroupScope(skillGraphNode.BuildAgent == null))
            {
                if (GUILayout.Button("View Build Agent", GUILayout.Height(40)))
                {
                    var buildAgentObject = skillGraphNode.BuildAgent.gameObject;
                    var existingWindow = FindOpenPropertyEditorWindow(buildAgentObject.name);

                    if (existingWindow != null)
                    {
                        existingWindow.Focus();
                    }
                    else
                    {
                        EditorUtility.OpenPropertyEditor(buildAgentObject);
                    }
                }
            }
        }
    }

}