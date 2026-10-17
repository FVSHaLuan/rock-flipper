// Play mode only. Plays {{COUNT}} mouse clicks: each flips one random rock or chest the player cursor could flip now
// (same candidate rules as AutoMouse: active, landing cooldown over, not airborne). TARGET = any | rocks | chests.
// Goes through Flippable.TryFlipping(FlipSource.Mouse), so landing cash, HP, exp and abilities run for real;
// it skips the cursor's hit test and the click/hover input mode, which is what AutoMouse/real input are for.
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode";
var re = Agame.Run.RunEntry.Instance;
if (re == null) return "no RunEntry yet";
var target = "{{TARGET}}";
var count = int.Parse("{{COUNT}}");

var candidates = new System.Collections.Generic.List<Agame.Run.Combat.FlippableByPlayerCursor>();
void TryAdd(Agame.Run.Combat.FlippableByPlayerCursor c)
{
    if (c != null && c.isActiveAndEnabled && c.CooledDown && !c.Flippable.IsFlipping) candidates.Add(c);
}
if (target != "chests") foreach (var r in re.rockInstanceManager.ActiveRocks) TryAdd(r.FlippableByPlayerCursor);
if (target != "rocks") foreach (var c in re.chestInstanceManager.ActiveChests) TryAdd(c.FlippableByPlayerCursor);

var before = re.RunData.GetCurrencyValue(Agame.Run.Currency.CASH);
int flipped = 0;
for (int i = 0; i < count && candidates.Count > 0; i++)
{
    var index = UnityEngine.Random.Range(0, candidates.Count);
    if (candidates[index].Flippable.TryFlipping(Agame.Run.Combat.FlipSource.Mouse)) flipped++;
    candidates.RemoveAt(index);
}
return "flipped " + flipped + "/" + count + " (" + candidates.Count + " more could flip now). Cash lands when they do: " + before.ToString("0.##") + " now.";
