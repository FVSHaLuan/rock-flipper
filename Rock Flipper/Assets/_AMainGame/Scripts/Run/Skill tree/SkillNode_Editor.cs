using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Agame.Run
{
    public partial class SkillNode
    {

#if UNITY_EDITOR
        public void Editor_ImportFromGraphNode(SkillGraphNode skillGraphNode)
        {
            ///
            graphNode = skillGraphNode;

            ///
            skillGraphNode.Editor_GetOutputNodes(outputNodes, GetComponentInParent<SkillTree>(true));

            ///
            iconImage.sprite = graphNode.Icon;
            iconImage.color = skillGraphNode.Editor_GetCashTierColor();
            upgradeFxImage.sprite = graphNode.Icon;
            costs_1 = new List<CurrencyAmount>(graphNode.costs_1);
            costs_2 = new List<CurrencyAmount>(graphNode.costs_2);
            costs_3 = new List<CurrencyAmount>(graphNode.costs_3);

            ///
            editor_AttentionObject.SetActive(graphNode.AttentionFlag);
        }

        public void Editor_ResetParents()
        {
            parents.Clear();
            DepthMax = -1;
        }

        [ContextMenu("Editor_SelectParents")]
        private void Editor_SelectParents()
        {
            ///
            if (parents == null || parents.Count == 0)
            {
                ///
                Debug.Log("No parents!");

                ///
                return;
            }

            ///
            var s = new GameObject[parents.Count];
            for (int i = 0; i < parents.Count; i++)
            {
                s[i] = parents[i].gameObject;
            }

            ///
            Selection.objects = s;

            ///
            EditorGUIUtility.PingObject(gameObject);
            Debug.LogFormat(gameObject, "Selected parents for {0}", gameObject.name);
        }

        [ContextMenu("Editor_MatchOutputNodesToConnectors")]
        public void Editor_MatchOutputNodesToConnectors()
        {
            ///
            foreach (var item in allConnectors)
            {
                item.gameObject.SetActive(false);
            }

            ///
            if (connectors == null)
            {
                connectors = new List<SkillNodeConnector>();
            }
            connectors.Clear();

            ///
            for (int i = 0; i < outputNodes.Count; i++)
            {
                var connector = Editor_GetConnector(GetOutputNode(i));
                connector?.gameObject.SetActive(true);
                connectors.Add(connector);
            }
        }

        [ContextMenu("Editor_SelectGraphNode")]
        public void Editor_SelectGraphNode()
        {
            Selection.activeObject = graphNode;
            XNodeEditor.NodeEditorWindow.Open(graphNode.graph);
        }

        [ContextMenu("Editor_RefundOneLevel")]
        public void Editor_RefundOneLevel()
        {
            Editor_Refund(1);
        }

        public void Editor_SetGradeText(string grade)
        {
            gradeText.Text = grade;
        }

        public void Editor_RefundAll()
        {
            Editor_Refund(state.level);
        }

        public void Editor_SetSkillTree(SkillTree skillTree)
        {
            this.skillTree = skillTree;
        }

        public void Editor_SetAsSpecialEntry()
        {
            isSpecialEntry = true;
        }

        private void Editor_Refund(int levelCount)
        {
            ///
            if (state.level < levelCount)
            {
                Debug.LogWarning("No level to refund!");

                ///
                return;
            }

            ///
            var savedLevel = state.level;

            ///
            state.level -= levelCount;

            ///
            RunData.UpdateSkillNodeState(NodeId, state);

            ///
            Debug.LogFormat("New level: {0}", state.level);

            ///
            OnStateChanged?.Invoke();

            ///
            ReleaseToolTip();
            RunEntry.skillTree.DrawTreeWithoutApplyingToBuildStats();

            ///
            RunEntry.MarkBuildStatsAsInvalid();
        }

        private SkillNodeConnector Editor_GetConnector(SkillNode outputNode)
        {
            var o = graphNode.position;
            var p = outputNode.graphNode.position;

            ///
            Vector2Int d = new Vector2Int();

            ///
            if (p.x - o.x >= SnapThreshold)
            {
                d.x = 1;
            }
            else if (o.x - p.x >= SnapThreshold)
            {
                d.x = -1;
            }

            ///
            if (p.y - o.y >= SnapThreshold)
            {
                d.y = -1;
            }
            else if (o.y - p.y >= SnapThreshold)
            {
                d.y = 1;
            }

            ///
            return GetConnector(d.x, d.y);
        }

        public void Editor_SnapOutputNodes(float distance)
        {
            for (int i = 0; i < outputNodes.Count; i++)
            {
                var node = GetOutputNode(i);
                var connector = connectors[i];

                ///
                if (connector == null)
                {
                    continue;
                }

                ///
                connector.Direction.GetVector(out int x, out int y);
                Vector2 d = new Vector2(x * distance, y * distance);
                node.transform.position = transform.position + (Vector3)d;

                ///
                var calculatedNodePosition = nodePosition + new Vector2Int(x, y);
                if (node.nodePosition.x == 0 && node.nodePosition.y == 0)
                {
                    node.nodePosition = calculatedNodePosition;
                }
                else if (node.nodePosition != calculatedNodePosition)
                {
                    Debug.LogError("Inconsistent nodePosition", node);
                }
            }
        }

        public void Editor_UpdateGameObjectName()
        {
            gameObject.name = string.Format("{0}_{1}", DepthMax, string.IsNullOrWhiteSpace(NodeId) ? "<NoId>" : NodeId);
        }

        public void Editor_LevelUp()
        {
            LevelUp();
        }

        public void Editor_LevelUpMax()
        {
            ///
            if (!IsMaxable)
            {
                Debug.LogError($"Skill node {gameObject.name} is not maxable");

                ///
                return;
            }

            ///
            while (!IsMaxed)
            {
                LevelUp();
            }
        }
#endif
    }

}