// Edit mode. Writes translations from a UTF-8 TSV file into one string table collection and saves the changed tables.
//   FILE       absolute path to the TSV file (forward slashes)
//   OVERWRITE  "false" skips entries that already have text in that locale, "true" replaces them
//   DRY        "true" only reports what would change
// TSV (tab-separated, one entry per row, a newline inside a text written as \n and a zero-width space (U+200B) as \u200B, lines starting with # ignored):
//   table	Rock Flipper Strings
//   key	zh	ja	...            <- locale codes, any subset, any order
//   Preferred	首选	お気に入り	...  <- an empty cell leaves that locale alone
// Keys are the entry keys as strings.cs prints them. English ("en") is refused: the owner manages the source text.
// Each written entry gets the English entry's Smart flag.
// Refuses the whole file, writing nothing, on an unknown table, key or locale.
if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return "REFUSED: stop Play mode first";
var path = "{{FILE}}";
var overwrite = "{{OVERWRITE}}" == "true";
var dry = "{{DRY}}" == "true";
if (!System.IO.File.Exists(path)) return "REFUSED: no file " + path;

var lines = new System.Collections.Generic.List<string[]>();
foreach (var raw in System.IO.File.ReadAllLines(path, System.Text.Encoding.UTF8))
{
    var line = raw.TrimEnd('\r');
    if (line.Trim().Length == 0 || line.StartsWith("#")) continue;
    lines.Add(line.Split('\t'));
}
if (lines.Count < 2 || lines[0][0] != "table" || lines[0].Length < 2 || lines[1][0] != "key")
    return "REFUSED: the file must start with a 'table<TAB>name' line and a 'key<TAB>locale...' header";
var tableName = lines[0][1].Trim();
var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(tableName);
if (collection == null) return "REFUSED: no string table collection named '" + tableName + "'";
var en = collection.GetTable("en") as UnityEngine.Localization.Tables.StringTable;
var codes = lines[1];

// validate everything first, so a bad file changes nothing
var errors = new System.Collections.Generic.List<string>();
for (int c = 1; c < codes.Length; c++)
{
    if (codes[c] == "en") errors.Add("column en: English is the source, not written here");
    else if (!(collection.GetTable(codes[c]) is UnityEngine.Localization.Tables.StringTable)) errors.Add("column " + c + ": no '" + codes[c] + "' table");
}
for (int r = 2; r < lines.Count; r++)
{
    if (collection.SharedData.GetEntry(lines[r][0]) == null) errors.Add("unknown key '" + lines[r][0] + "'");
    if (lines[r].Length > codes.Length) errors.Add(lines[r][0] + ": more cells than header columns");
}
if (errors.Count > 0) return "REFUSED, nothing written:\n" + string.Join("\n", errors);

var changed = new System.Collections.Generic.HashSet<UnityEngine.Localization.Tables.StringTable>();
int written = 0, same = 0;
var skipped = new System.Collections.Generic.List<string>();
for (int r = 2; r < lines.Count; r++)
{
    var shared = collection.SharedData.GetEntry(lines[r][0]);
    var enEntry = en.GetEntry(shared.Id);
    var smart = enEntry != null && enEntry.IsSmart;
    for (int c = 1; c < lines[r].Length; c++)
    {
        var text = lines[r][c].Replace("\\n", "\n").Replace("\\u200B", "\u200B");
        if (text.Trim().Length == 0) continue;
        var table = (UnityEngine.Localization.Tables.StringTable)collection.GetTable(codes[c]);
        var entry = table.GetEntry(shared.Id);
        if (entry != null && entry.Value == text && entry.IsSmart == smart) { same++; continue; }
        if (entry != null && !string.IsNullOrEmpty(entry.Value) && !overwrite) { skipped.Add(shared.Key + "/" + codes[c]); continue; }
        written++;
        if (dry) continue;
        entry = table.AddEntry(shared.Id, text);
        entry.IsSmart = smart;
        changed.Add(table);
    }
}
foreach (var t in changed)
{
    UnityEditor.EditorUtility.SetDirty(t);
    UnityEditor.AssetDatabase.SaveAssetIfDirty(t);
}
return (dry ? "DRY RUN, would write " : "written ") + written + ", unchanged " + same + ", tables saved " + changed.Count
    + (skipped.Count > 0 ? "\nskipped (already translated, OVERWRITE=false): " + string.Join(", ", skipped) : "");
