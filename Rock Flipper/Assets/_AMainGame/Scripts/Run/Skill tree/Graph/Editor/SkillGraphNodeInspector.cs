using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace Agame.Run
{
    [CustomEditor(typeof(SkillGraphNode))]
    public class SkillGraphNodeInspector : GlobalNodeEditor
    {
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
                    EditorUtility.OpenPropertyEditor(skillGraphNode.BuildAgent.gameObject);
                }
            }
        }
    }

}