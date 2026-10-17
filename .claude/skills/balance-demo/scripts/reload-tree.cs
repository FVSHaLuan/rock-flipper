// Play mode only. Same as the dev "Reload Skill Tree Button" (SkillTreeReloader.ReloadSkillTree): redraws the
// Run scene's tree from the graph asset as it is now and rebuilds BuildStats. Picks up buildValue, levelCount,
// price and attentionFlag changes without a re-import or a restart. It doesn't pick up new/removed nodes, rewiring,
// icons or titles: those need SkillTree -> Editor_ImportFromGraph, a scene edit.
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode (nothing to reload; the scene reads the graph when Play starts)";
var reloader = UnityEngine.Object.FindFirstObjectByType<Agame.Run.Dev.SkillTreeReloader>(UnityEngine.FindObjectsInactive.Include);
if (reloader == null) return "no SkillTreeReloader in the scene";
reloader.ReloadSkillTree();
return "reloaded skill tree and rebuilt BuildStats";
