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
            if (GUILayout.Button("Fill costs", GUILayout.Height(40)))
            {
                (target as SkillGraphNode).Editor_FillCosts();
            }
        }
    }

}