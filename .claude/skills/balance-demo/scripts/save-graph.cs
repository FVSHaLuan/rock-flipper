// Edit mode only. Writes the skill graph's pending changes (from node-set.cs) to MainSkillTree.asset on disk.
// Graph edits made during Play mode stay in memory after Play stops; this is what makes them stick.
if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return "REFUSED: stop Play mode first";
var graph = UnityEditor.AssetDatabase.LoadAssetAtPath<Agame.Run.SkillTreeGraph>("Assets/_AMainGame/Data/Skill Trees/MainSkillTree.asset");
if (graph == null) return "MainSkillTree.asset not found";
// Re-mark in case the dirty flag was lost across the Play-mode transition; the in-memory values are what get written
UnityEditor.EditorUtility.SetDirty(graph);
foreach (var n in graph.nodes) if (n != null) UnityEditor.EditorUtility.SetDirty(n);
UnityEditor.AssetDatabase.SaveAssetIfDirty(graph);
return "saved MainSkillTree.asset";
