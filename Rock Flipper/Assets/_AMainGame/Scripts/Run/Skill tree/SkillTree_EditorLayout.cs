using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run
{
    public partial class SkillTree
    {
        [Header("Editor")]
        [SerializeField]
        private SkillNode nodePrototype;
        [SerializeField]
        private SkillNode_Debug debugNodePrototype;
        [SerializeField]
        private float nodeDistanceInUnit = 240 / 100f;
        [SerializeField]
        private float treePaddingScale = 10;
        [SerializeField]
        private Transform debugRoot;

        private List<SkillNode> editor_SkillNodes = new List<SkillNode>();
        private Dictionary<SkillGraphNode, SkillNode> editor_SkillGraphNodeDictionary = new Dictionary<SkillGraphNode, SkillNode>();
        private Dictionary<Sprite, List<SkillNode>> editor_SkillIconDictionary = new Dictionary<Sprite, List<SkillNode>>();
#if UNITY_EDITOR
        [ContextMenu("Editor_ImportFromGraph (Dirty)"), EditorModeOnly]
        private void Editor_ImportFromGraph()
        {
            ///
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Import skill tree from graph");

            // Destroy all nodes
            GetComponentsInChildren(true, editor_SkillNodes);
            foreach (var item in editor_SkillNodes)
            {
                DestroyImmediate(item.gameObject);
            }

            // Prepare to spawn nodes
            editor_SkillGraphNodeDictionary.Clear();

            // Spawn nodes
            Editor_SpawnNodes(mainSkillTreeGraph, ref mainSkillNodes, ref mainRootNode, mainTreeRoot);
            Editor_FindSpecialEntryNode();
            Editor_SpawnNodes(laserSkillTreeGraph, ref laserSkillNodes, ref laserRootNode, laserTreeRoot);
            Editor_SpawnNodes(lightningSkillTreeGraph, ref lightningSkillNodes, ref lightningRootNode, lightningTreeRoot);

            // Import
            foreach (var item in editor_SkillGraphNodeDictionary)
            {
                ///
                var node = item.Value;

                ///
                node.Editor_ImportFromGraphNode(item.Key);
            }

            ///
            if (mainRootNode == null)
            {
                Debug.LogError("No RootNode found!");
                return;
            }

            ///
            var specialCrusherIds = new SpecialCrusherId[]
            {
                SpecialCrusherId.Laser,
                SpecialCrusherId.Lightning
            };

            ///            
            foreach (var item in specialCrusherIds)
            {
                ///
                SetActiveSpecialTree(item);

                ///
                Editor_Snap();

                ///
                Editor_SetNodeGrades();
                Editor_CheckForDuplicates();
            }

            ///
            Editor_UpdateSize();
            Editor_SpawnDebugNodes();

            ///
            Debug.Log("Done!");
        }

        private void Editor_SpawnNodes(SkillTreeGraph treeGraph, ref List<SkillNode> nodeList, ref SkillNode rootNode, Transform rootTransform)
        {
            if (nodeList == null)
            {
                nodeList = new List<SkillNode>();
            }
            nodeList.Clear();

            ///
            foreach (var item in treeGraph.nodes)
            {
                var graphNode = item as SkillGraphNode;

                ///
                if (graphNode == null)
                {
                    continue;
                }

                ///
                var skillNodePrototype = graphNode.SkillNodePrototype == null ? nodePrototype : graphNode.SkillNodePrototype;
                var skillNode = UnityEditor.PrefabUtility.InstantiatePrefab(skillNodePrototype, rootTransform) as SkillNode;
                skillNode.Editor_SetSkillTree(this);
                nodeList.Add(skillNode);

                ///
                editor_SkillGraphNodeDictionary[graphNode] = skillNode;

                // Find root
                if (treeGraph.RootNode == graphNode)
                {
                    if (rootNode != null)
                    {
                        Debug.LogError("There are more than 1 RootNode", skillNode);
                    }

                    ///
                    rootNode = skillNode;

                    ///
                    rootNode.transform.localPosition = Vector3.zero;
                }
            }
        }

        private void Editor_FindSpecialEntryNode()
        {
            specialEntryNode = null;
            foreach (var item in mainSkillTreeGraph.nodes)
            {
                if (item is SkillGraphNode && item.name == "SpecialEntry")
                {
                    if (specialEntryNode != null)
                    {
                        Debug.LogError("There are more than 1 SpecialEntryNode", item);
                        throw new System.Exception("There are more than 1 SpecialEntryNode in the graph!");
                    }

                    ///
                    specialEntryNode = editor_SkillGraphNodeDictionary[item as SkillGraphNode];
                    specialEntryNode.Editor_SetAsSpecialEntry();
                    specialEntryNode.gameObject.SetActive(false);
                }
            }

            ///
            if (specialEntryNode == null)
            {
                throw new System.Exception("SpecialEntry node not found!");
            }
        }

        private void Editor_SetNodeGrades()
        {
            ///
            GetComponentsInChildren<SkillNode>(editor_SkillNodes);

            ///
            if (editor_SkillIconDictionary == null)
            {
                editor_SkillIconDictionary = new Dictionary<Sprite, List<SkillNode>>();
            }
            editor_SkillIconDictionary.Clear();

            ///
            foreach (var item in editor_SkillNodes)
            {
                List<SkillNode> list;
                if (!editor_SkillIconDictionary.TryGetValue(item.GraphNode.Icon, out list))
                {
                    list = new List<SkillNode>();
                    editor_SkillIconDictionary[item.GraphNode.Icon] = list;
                }

                ///
                list.Add(item);
            }

            ///
            foreach (var item in editor_SkillIconDictionary.Values)
            {
                if (item.Count == 0)
                {
                    continue;
                }

                ///
                if (item.Count == 1)
                {
                    item[0].Editor_SetGradeText("");
                    continue;
                }

                ///
                item.Sort((x, y) => (x.DepthMax - y.DepthMax));

                ///
                for (int i = 0; i < item.Count; i++)
                {
                    item[i].Editor_SetGradeText((i + 1).ToRomanian());
                }
            }
        }

        private void Editor_Snap()
        {
            ///
            Editor_Outline();
            Editor_NameNodes();
            // Editor_SortNodesByName();

            ///
            Editor_SnapRecursively(mainRootNode);
        }

        private void Editor_ValidateRelationship()
        {
            ///
            GetComponentsInChildren<SkillNode>(editor_SkillNodes);
            foreach (var item in editor_SkillNodes)
            {
                item.Editor_ResetParents();
            }

            ///
            try
            {
                AssignParents();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        private void Editor_Outline()
        {
            ///
            Editor_ValidateRelationship();

            ///
            GetComponentsInChildren<SkillNode>(editor_SkillNodes);

            ///
            foreach (var item in editor_SkillNodes)
            {
                item.Editor_MatchOutputNodesToConnectors();
            }
        }

        private void Editor_SnapRecursively(SkillNode skillNode)
        {
            ///
            // Debug.LogFormat("Snapping {0} - {1}", skillNode.gameObject.name, skillNode.NodeId);

            ///
            skillNode.Editor_SnapOutputNodes(nodeDistanceInUnit * transform.localScale.x);

            ///
            for (int i = 0; i < skillNode.OutputNodeCount; i++)
            {
                var outputNode = skillNode.GetOutputNode(i);
                if (outputNode.GetParentNode(0) == skillNode)
                {
                    Editor_SnapRecursively(outputNode);
                }
            }
        }

        private void Editor_NameNodes()
        {
            ///
            GetComponentsInChildren(editor_SkillNodes);

            ///
            foreach (var item in editor_SkillNodes)
            {
                item.Editor_UpdateGameObjectName();
            }
        }

        [System.Obsolete("Obsolete because now there multiple tree roots")]
        private void Editor_SortNodesByName()
        {
            ///
            GetComponentsInChildren(editor_SkillNodes);

            ///
            editor_SkillNodes.Sort((SkillNode t1, SkillNode t2) => { return t1.gameObject.name.CompareTo(t2.gameObject.name); });

            ///
            for (int i = 0; i < editor_SkillNodes.Count; i++)
            {
                editor_SkillNodes[i].transform.SetSiblingIndex(i);
            }
        }

        public SkillNode Editor_GetSkillNode(SkillGraphNode node)
        {
            return editor_SkillGraphNodeDictionary[node];
        }

        private void Editor_UpdateSize()
        {
            ///
            minPosition = Vector2.zero;
            maxPosition = Vector2.zero;

            ///
            Vector2 padding = Vector2.zero;
            bool setPad = false;

            ///
            GetComponentsInChildren(true, editor_SkillNodes);
            foreach (var node in editor_SkillNodes)
            {
                var rt = node.GetComponent<RectTransform>();
                var localMinPos = rt.anchoredPosition - rt.sizeDelta / 2;
                var localMaxPos = rt.anchoredPosition + rt.sizeDelta / 2;

                ///
                minPosition.x = Mathf.Min(localMinPos.x, minPosition.x);
                minPosition.y = Mathf.Min(localMinPos.y, minPosition.y);
                maxPosition.x = Mathf.Max(localMaxPos.x, maxPosition.x);
                maxPosition.y = Mathf.Max(localMaxPos.y, maxPosition.y);

                ///
                if (!setPad)
                {
                    setPad = true;
                    padding = rt.sizeDelta * treePaddingScale;
                }
            }

            ///
            GetComponent<RectTransform>().sizeDelta = new Vector2()
            {
                x = Mathf.Max(Mathf.Abs(minPosition.x), Mathf.Abs(maxPosition.x)) * 2,
                y = Mathf.Max(Mathf.Abs(minPosition.y), Mathf.Abs(maxPosition.y)) * 2,
            }
            + padding * 2;
        }

        private void Editor_CheckForDuplicates()
        {
            for (int i = 0; i < editor_SkillNodes.Count - 1; i++)
            {
                var node = editor_SkillNodes[i];

                ///
                for (int j = i + 1; j < editor_SkillNodes.Count; j++)
                {
                    var otherNode = editor_SkillNodes[j];

                    // Positions
                    if (node.NodePosition == otherNode.NodePosition)
                    {
                        Debug.LogError("Those nodes have the same position:");
                        Debug.LogError(node.name, node);
                        Debug.LogError(otherNode.name, otherNode);
                    }

                    // Id
                    if (node.NodeId == otherNode.NodeId)
                    {
                        Debug.LogError("Those nodes have the same id:");
                        Debug.LogError(node.name, node);
                        Debug.LogError(otherNode.name, otherNode);
                    }
                }
            }
        }

        private void Editor_SpawnDebugNodes()
        {
            ///
            debugRoot.SetAsLastSibling();

            ///
            var children = debugRoot.GetComponentsInChildren<Transform>(true);
            foreach (var item in children)
            {
                if (item == null
                    || item == debugRoot
                    || item.parent != debugRoot)
                {
                    continue;
                }
                DestroyImmediate(item.gameObject);
            }

            ///
            foreach (var node in mainSkillNodes)
            {
                var debugNode = UnityEditor.PrefabUtility.InstantiatePrefab(debugNodePrototype, debugRoot) as SkillNode_Debug;
                debugNode.transform.position = node.transform.position;
                debugNode.SetNode(node.GraphNode);
            }
        }
#endif
    }

}