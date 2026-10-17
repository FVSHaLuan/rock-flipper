// Play mode only. Sets the dev gameplay time scale (the "Dev Gameplay Time Scale Controller" slider) to {{SCALE}}.
// The slider's range is set in the scene (0.2x-20x as of 2026-10; the result prints it) and it also fast-forwards play-time timers in the editor (RunData.PlayTimeAdjustment).
// The slider is a dev UI: if dev UIs are hidden it is inactive and has no effect, so this switches it on for this
// Play session only (nothing is saved) and asks to be run again once it has started.
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode";
var slider = UnityEngine.Object.FindAnyObjectByType<Agame.Run.Dev.GameplayTimeScaleSlider>(UnityEngine.FindObjectsInactive.Include);
if (slider == null) return "no GameplayTimeScaleSlider in the scene";
if (!slider.isActiveAndEnabled)
{
    for (var t = slider.transform; t != null; t = t.parent) t.gameObject.SetActive(true);
    return "time scale slider was hidden (dev UIs off); switched it on. Run this again to set the scale.";
}
var ui = (UnityEngine.UI.Slider)typeof(Agame.Run.Dev.GameplayTimeScaleSlider)
    .GetField("slider", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
    .GetValue(slider);
var min = (float)typeof(Agame.Run.Dev.GameplayTimeScaleSlider).GetField("minScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(slider);
var max = (float)typeof(Agame.Run.Dev.GameplayTimeScaleSlider).GetField("maxScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(slider);
ui.normalizedValue = UnityEngine.Mathf.InverseLerp(min, max, float.Parse("{{SCALE}}", System.Globalization.CultureInfo.InvariantCulture));
return "gameplay time scale control = " + slider.ControlValue.ToString("0.00") + " (range " + min + "-" + max + "), Time.timeScale = " + UnityEngine.Time.timeScale.ToString("0.00");
