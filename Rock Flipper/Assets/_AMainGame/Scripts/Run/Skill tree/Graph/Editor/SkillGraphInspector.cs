using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Agame.Run
{
    [CustomEditor(typeof(SkillTreeGraph))]
    public class SkillGraphInspector : GlobalGraphEditor
    {
        protected override void DrawTopButtons()
        {
            ///
            base.DrawTopButtons();

            ///
            if (GUILayout.Button("Select Attentions", GUILayout.Height(TopButtonHeight)))
            {
                SelectAttentions();
            }
        }


        private void SelectAttentions()
        {
            var nodeGraph = target as XNode.NodeGraph;
            List<Object> selectObjects = new List<Object>();
            foreach (var item in nodeGraph.nodes)
            {
                var skillNode = item as SkillGraphNode;
                if (skillNode != null && skillNode.AttentionFlag)
                {
                    selectObjects.Add(skillNode);
                }
            }

            ///
            UnityEditor.Selection.objects = selectObjects.ToArray();
        }
    }

}