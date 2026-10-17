// Play mode only. Clicks the UI Button whose hierarchy path ends with PATH (e.g. "Shop/Rock Count P0/Stat Builder Button Variant",
// "Side bar/Skill tree button", "Skill Tree/Main/0_g0_P0 Landing Cash"). FORCE = false refuses a button a player couldn't
// click (hidden, not interactable, or under a non-interactable CanvasGroup); true clicks it anyway, for checking guards.
// Runs the button's onClick exactly as a real click would.
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode";
var suffix = "{{PATH}}";
var force = bool.Parse("{{FORCE}}");
var matches = new System.Collections.Generic.List<(UnityEngine.UI.Button button, string path)>();
foreach (var b in UnityEngine.Object.FindObjectsByType<UnityEngine.UI.Button>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var path = b.name;
    for (var t = b.transform.parent; t != null; t = t.parent) path = t.name + "/" + path;
    if (path == suffix || path.EndsWith("/" + suffix)) matches.Add((b, path));
}
if (matches.Count == 0) return "no button path ends with '" + suffix + "'";
if (matches.Count > 1) return "ambiguous, " + matches.Count + " buttons match:\n" + string.Join("\n", System.Linq.Enumerable.Select(matches, m => "  " + m.path));

var (button, fullPath) = matches[0];

// buttons that leave the game (browser, Steam page, Home scene, quit) or reload it: never during a playtest
foreach (var bad in new[] { "Wishlist Box", "Feedback Button", "Quit Button", "Home Button", "Reload Scene Button", "Reset Tutorial Button" })
{
    if (fullPath.Contains(bad)) return "REFUSED: " + fullPath + " leaves or reloads the game. Do this by hand if it's really part of the test.";
}
// a demo-limited skill node opens the Steam store page instead of upgrading
var node = button.GetComponent<Agame.Run.SkillNode>();
if (node != null && node.IsDemoLimited) return "REFUSED: " + fullPath + " is demo-limited; clicking it opens the Steam page.";

var clickable = button.isActiveAndEnabled && button.IsInteractable();
if (!clickable && !force)
{
    return "NOT CLICKABLE: " + fullPath + " (activeInHierarchy " + button.gameObject.activeInHierarchy + ", interactable " + button.interactable + ", IsInteractable " + button.IsInteractable() + ")";
}
button.onClick.Invoke();
return "clicked " + fullPath + (clickable ? "" : " (FORCED: a player couldn't have)");
