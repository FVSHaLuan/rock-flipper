using Mono.Cecil;
using RectEx;
using Agame.FeatureBranching;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Agame.Run
{
    [CustomNodeEditor(typeof(SkillGraphNode))]
    public class SkillGraphNodeEditor : NodeEditor
    {
        private HashSet<string> excludedFields;

        public override HashSet<string> ExcludedFields
        {
            get
            {
                if (excludedFields == null)
                {
                    excludedFields = new HashSet<string>(base.ExcludedFields)
                    {
                        "attentionFlag",
                        "icon",
                        "cashTier",
                        "buildAgent",
                        "buildValue",
                        "unlockingRequirement",
                        "minParentLevelEach",
                        "costs_1",
                        "costs_2",
                        "costs_3",
                        "costFormulas",
                        "demoLimit",
                        "skillNodePrototype"
                    };
                }

                ///
                return excludedFields;
            }
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
        }

        protected override void DrawFields()
        {
            ///
            base.DrawFields();

            ///
            var node = target as SkillGraphNode;

            ///
            if (node == null)
            {
                return;
            }

            ///
            var skillTree = node.graph as SkillTreeGraph;

            // Error
            string error = "";
            if (node.Icon == null)
            {
                error += "No icon\r\n";
            }
            if (node.BuildAgent == null)
            {
                error += "No agent\r\n";
            }
            if (node.name == "Skill Graph")
            {
                error += "No unique name\r\n";
            }
            if (node.AttentionFlag)
            {
                error += "Attention Flag\r\n";
            }

            ///
            if (error != "")
            {
                if (skillTree == null || !skillTree.CompactMode)
                {
                    EditorGUILayout.HelpBox(error, MessageType.Error);
                }
                else
                {
                    EditorGUILayout.HelpBox("", MessageType.Error);
                }
            }

            // Warning
            if (VersionBranchInfo.IsPlaytestOrDemo)
            {
                string warning = "";
                if (node.DemoLimit >= 0)
                {
                    warning += $"{node.DemoLimit}";
                }

                ///
                if (warning != "")
                {
                    EditorGUILayout.HelpBox(warning, MessageType.Warning);
                }
            }

            ///
            if (skillTree == null || !skillTree.CompactMode)
            {
                string info = "";
                info += "Agent: " + (node.BuildAgent != null ? node.BuildAgent.name : "<NONE>") + "\r\n";
                info += "Build value: " + node.BuildValue + "\r\n";
                info += "Level: " + node.LevelCount + "\r\n";
                info += "2nd + 3rd currency: " + node.SecondaryCurrency + "-" + node.ThirdCurrency + "\r\n";

                ///
                EditorGUILayout.HelpBox(info, MessageType.Info);
            }

            ///
            if (node.Icon != null)
            {
                var rect = EditorGUILayout.GetControlRect(false, 50);
                var sprite = node.Icon;
                Rect spriteRect = sprite.textureRect;
                Texture2D tex = sprite.texture;
                GUI.color = node.Editor_GetCashTierColor();
                GUI.DrawTextureWithTexCoords(spriteRect.FitCompletelyInside(rect), tex, new Rect(spriteRect.x / tex.width, spriteRect.y / tex.height, spriteRect.width / tex.width, spriteRect.height / tex.height));
            }
        }

        public override int GetWidth()
        {
            ///
            var skillTree = target.graph as SkillTreeGraph;
            if ((skillTree != null) && skillTree.CompactMode)
            {
                return skillTree.CompactModeWidth;
            }

            ///
            return base.GetWidth();
        }
    }

}