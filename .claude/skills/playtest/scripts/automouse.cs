// Play mode only. Turns the dev AutoMouse on or off: ON = true | false.
// AutoMouse plays the mouse continuously the way the current build stats allow: random clicks (flipsPerSecond) until
// Mouse Hover is unlocked, then a virtual cursor that hovers toward rocks/chests and flips what it touches.
// While it is on, the real cursor can't flip. It is a dev UI toggle: if dev UIs are hidden this switches its
// GameObject on for this Play session only (nothing is saved).
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode";
var auto = UnityEngine.Object.FindAnyObjectByType<Agame.Run.Dev.AutoMouse>(UnityEngine.FindObjectsInactive.Include);
if (auto == null) return "no AutoMouse in the scene";
var on = bool.Parse("{{ON}}");
if (on && !auto.isActiveAndEnabled)
{
    for (var t = auto.transform; t != null; t = t.parent) t.gameObject.SetActive(true);
}
var toggle = auto.GetComponent<UnityEngine.UI.Toggle>();
toggle.isOn = on;
return "AutoMouse " + (toggle.isOn ? "ON" : "OFF") + (auto.isActiveAndEnabled ? "" : " (its GameObject only starts next frame)");
