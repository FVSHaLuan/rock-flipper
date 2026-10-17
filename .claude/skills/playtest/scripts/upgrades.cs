// Play mode only. Every upgrade the player can see, cheapest first, then the ones that can't be bought and why.
// Line 1 (parsed by buy-cheapest.sh): T <gameplay time> CASH <cash> STAR <stars> LEVEL <level> EXP <exp>/<needed>
// Rows:  BUY <cost> <currency> <affordable|short> | <what> L<level>/<max> | PATH=<click.cs path>
//        BLOCKED <reason> | <what> | PATH=<path>     reasons: DEMO-LIMITED, NEEDS-ATTENTION, SKILL-TREE-LOCKED
// Skill-node costs can be in STAR (earned per player level), not only CASH.
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode";
var re = Agame.Run.RunEntry.Instance;
if (re == null) return "no RunEntry yet";
var data = re.RunData;
var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
var rows = new System.Collections.Generic.List<(double cost, string line)>();
var blocked = new System.Collections.Generic.List<string>();
string Row(Agame.Run.CurrencyAmount cost, string what)
{
    var enough = data.GetCurrencyValue(cost.currency) >= cost.amount;
    return "BUY " + cost.amount.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + " " + cost.currency + " " + (enough ? "affordable" : "short") + " | " + what;
}

// Shop buttons on the visible tab content (hidden ones, e.g. Rock Count P1 before its unlock, aren't offered to the player)
foreach (var b in UnityEngine.Object.FindObjectsByType<Agame.Run.Shop.StatBuilderButton>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    if (!b.gameObject.activeInHierarchy) continue;
    var t = typeof(Agame.Run.Shop.StatBuilderButton);
    var id = (string)t.GetField("buttonId", flags).GetValue(b);
    var costConfig = (Agame.Run.Shop.ICostConfig)t.GetField("costConfig", flags).GetValue(b);
    var maxConfig = (Agame.Run.Shop.IMaxLevelConfig)t.GetField("maxLevelConfig", flags).GetValue(b);
    var level = data.GetBuilderButtonLevel(id);
    if (level >= maxConfig.MaxLevel) continue;
    var cost = costConfig.GetNextLevelCost(level);
    rows.Add((cost.amount, Row(cost, "shop " + id + " L" + level + "/" + maxConfig.MaxLevel + " | PATH=Shop/" + b.name + "/Stat Builder Button Variant")));
}

// Activated (visible) skill nodes
var treeUnlocked = re.BuildStats.UnlockedSkillTree;
foreach (var n in UnityEngine.Object.FindObjectsByType<Agame.Run.SkillNode>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    if (n.IsDecorative || !n.IsActivated || n.IsMaxed) continue;
    var what = "node " + n.name + " L" + n.Level + "/" + n.LevelCount + " | PATH=" + n.transform.parent.name + "/" + n.name;
    if (n.IsDemoLimited) { blocked.Add("BLOCKED DEMO-LIMITED | " + what); continue; }
    if (n.GraphNode.AttentionFlag) { blocked.Add("BLOCKED NEEDS-ATTENTION | " + what); continue; }
    if (!treeUnlocked) { blocked.Add("BLOCKED SKILL-TREE-LOCKED | " + what); continue; }
    var cost = n.GraphNode.GetNextLevelCost(n.Level);
    rows.Add((cost.amount, Row(cost, what)));
}

rows.Sort((a, b) => a.cost.CompareTo(b.cost));
var inv = System.Globalization.CultureInfo.InvariantCulture;
var sb = new System.Text.StringBuilder();
sb.AppendLine("T " + UnityEngine.Time.time.ToString("0.0", inv)
    + " CASH " + data.GetCurrencyValue(Agame.Run.Currency.CASH).ToString("0.##", inv)
    + " STAR " + data.GetCurrencyValue(Agame.Run.Currency.STAR).ToString("0.##", inv)
    + " LEVEL " + data.Level + " EXP " + data.LevelExp.ToString("0", inv) + "/" + Entry.Instance.gameBalance.GetRequiredExpForNextLevel(data.Level).ToString("0", inv));
foreach (var r in rows) sb.AppendLine(r.line);
foreach (var x in blocked) sb.AppendLine(x);
return sb.ToString();
