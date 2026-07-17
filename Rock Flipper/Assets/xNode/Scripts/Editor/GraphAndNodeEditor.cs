using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
#endif

namespace XNodeEditor
{
    /// <summary> Override graph inspector to show an 'Open Graph' button at the top </summary>
    [CustomEditor(typeof(XNode.NodeGraph), true)]
#if ODIN_INSPECTOR
    public class GlobalGraphEditor : OdinEditor {
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Edit graph", GUILayout.Height(40))) {
                NodeEditorWindow.Open(serializedObject.targetObject as XNode.NodeGraph);
            }
            base.OnInspectorGUI();
        }
    }
#else
    public class GlobalGraphEditor : Editor
    {
        protected const float TopButtonHeight = 24;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ///
            DrawTopButtons();

            ///
            var nodeGraph = target as XNode.NodeGraph;

            ///
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("[Render config]", "BoldLabel");
            nodeGraph.CompactMode = GUILayout.Toggle(nodeGraph.CompactMode, "Compact Mode");
            nodeGraph.DrawStraightConnection = GUILayout.Toggle(nodeGraph.DrawStraightConnection, "DrawStraightConnection");
            nodeGraph.HideInputOutputHandles = GUILayout.Toggle(nodeGraph.HideInputOutputHandles, "HideInputOutputHandles");

            ///
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("[Raw data]", "BoldLabel");

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawTopButtons()
        {
            if (GUILayout.Button("Edit graph", GUILayout.Height(TopButtonHeight)))
            {
                NodeEditorWindow.Open(serializedObject.targetObject as XNode.NodeGraph);
            }
            if (GUILayout.Button("Check overlap", GUILayout.Height(TopButtonHeight)))
            {
                (serializedObject.targetObject as XNode.NodeGraph).Editor_CheckOverlapNodes();
            }
        }
    }
#endif

    [CustomEditor(typeof(XNode.Node), true)]
#if ODIN_INSPECTOR
    public class GlobalNodeEditor : OdinEditor {
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Edit graph", GUILayout.Height(40))) {
                SerializedProperty graphProp = serializedObject.FindProperty("graph");
                NodeEditorWindow w = NodeEditorWindow.Open(graphProp.objectReferenceValue as XNode.NodeGraph);
                w.Home(); // Focus selected node
            }
            base.OnInspectorGUI();
        }
    }
#else
    public class GlobalNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ///
            serializedObject.Update();

            ///
            DrawTopButtons();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("Raw data", "BoldLabel");

            // Now draw the node itself.
            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawTopButtons()
        {
            if (GUILayout.Button("Edit graph", GUILayout.Height(40)))
            {
                SerializedProperty graphProp = serializedObject.FindProperty("graph");
                NodeEditorWindow w = NodeEditorWindow.Open(graphProp.objectReferenceValue as XNode.NodeGraph);
                w.Home(); // Focus selected node
            }

            ///
            if (GUILayout.Button("Select graph", GUILayout.Height(40)))
            {
                SerializedProperty graphProp = serializedObject.FindProperty("graph");
                Selection.activeObject = graphProp.objectReferenceValue as XNode.NodeGraph;
                EditorGUIUtility.PingObject(graphProp.objectReferenceValue);
            }
        }
    }
#endif
}