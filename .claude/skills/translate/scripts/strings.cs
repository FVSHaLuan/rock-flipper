// Edit or Play mode. Read only. Lists string table entries with their English text and what each locale is missing.
//   TABLE    collection name ("Common Strings"), or "all" for every string table collection
//   KEY      substring of the key to match (case-insensitive), "-" for every key
//   MISSING  "any" lists only entries untranslated in at least one locale, a locale code ("ja") only those missing there, "-" every entry
//   SHOW     comma-separated locale codes whose current text to print too ("ja,zh"), "all", or "-"
// Per entry: key, id, [smart], the entry's metadata ("note:" on the shared entry, "en note:" on the English entry),
// the English text, missing locales, then the SHOW texts with their own notes.
// Notes print comment texts, and other metadata types as <TypeName> (Smart flags show as [smart] instead).
// Newlines in texts print as \n. Pseudo-locales are skipped.
var tableArg = "{{TABLE}}";
var keyArg = "{{KEY}}";
var missingArg = "{{MISSING}}";
var showArg = "{{SHOW}}";

var collections = new System.Collections.Generic.List<UnityEditor.Localization.StringTableCollection>();
foreach (var c in UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollections())
    if (tableArg == "all" || c.TableCollectionName == tableArg) collections.Add(c);
if (collections.Count == 0) return "no string table collection named '" + tableArg + "'";

var locales = new System.Collections.Generic.List<UnityEngine.Localization.Locale>();
foreach (var l in UnityEditor.Localization.LocalizationEditorSettings.GetLocales())
    if (!(l is UnityEngine.Localization.Pseudo.PseudoLocale) && l.Identifier.Code != "en") locales.Add(l);
if (missingArg != "-" && missingArg != "any" && !locales.Exists(l => l.Identifier.Code == missingArg))
    return "unknown locale '" + missingArg + "'";

var show = new System.Collections.Generic.List<string>();
if (showArg == "all") locales.ForEach(l => show.Add(l.Identifier.Code));
else if (showArg != "-") show.AddRange(showArg.Split(','));

System.Func<string, string> esc = s => s == null ? "" : s.Replace("\r", "").Replace("\n", "\\n");
// every metadata item: comments as their text, anything else by type name
System.Func<System.Collections.Generic.IList<UnityEngine.Localization.Metadata.IMetadata>, string> meta = items =>
{
    var parts = new System.Collections.Generic.List<string>();
    if (items != null)
        foreach (var m in items)
        {
            if (m is UnityEngine.Localization.Metadata.Comment cm) { if (!string.IsNullOrEmpty(cm.CommentText)) parts.Add(esc(cm.CommentText)); }
            else if (m != null && !(m is UnityEngine.Localization.Metadata.SmartFormatTag)) parts.Add("<" + m.GetType().Name + ">");
        }
    return string.Join(" | ", parts);
};
var sb = new System.Text.StringBuilder();
sb.AppendLine("locales: " + string.Join(", ", locales.ConvertAll(l => l.Identifier.Code)));
int listed = 0;
foreach (var c in collections)
{
    var en = c.GetTable("en") as UnityEngine.Localization.Tables.StringTable;
    if (en == null) { sb.AppendLine("== " + c.TableCollectionName + ": no English table"); continue; }
    sb.AppendLine("== " + c.TableCollectionName + " (" + c.SharedData.Entries.Count + " keys)");
    foreach (var shared in c.SharedData.Entries)
    {
        if (keyArg != "-" && shared.Key.IndexOf(keyArg, System.StringComparison.OrdinalIgnoreCase) < 0) continue;
        var enEntry = en.GetEntry(shared.Id);
        var missing = new System.Collections.Generic.List<string>();
        foreach (var l in locales)
        {
            var t = c.GetTable(l.Identifier) as UnityEngine.Localization.Tables.StringTable;
            var e = t == null ? null : t.GetEntry(shared.Id);
            if (e == null || string.IsNullOrEmpty(e.Value)) missing.Add(l.Identifier.Code);
        }
        if (missingArg == "any" && missing.Count == 0) continue;
        if (missingArg != "-" && missingArg != "any" && !missing.Contains(missingArg)) continue;
        listed++;

        sb.Append("- ").Append(shared.Key).Append(" [").Append(shared.Id).Append("]");
        if (enEntry != null && enEntry.IsSmart) sb.Append(" [smart]");
        sb.AppendLine();
        var sharedNote = meta(shared.Metadata.MetadataEntries);
        if (sharedNote.Length > 0) sb.AppendLine("    note: " + sharedNote);
        var enNote = enEntry == null ? "" : meta(enEntry.MetadataEntries);
        if (enNote.Length > 0) sb.AppendLine("    en note: " + enNote);
        sb.AppendLine("    en: " + (enEntry == null || string.IsNullOrEmpty(enEntry.Value) ? "<EMPTY>" : esc(enEntry.Value)));
        sb.AppendLine("    missing: " + (missing.Count == 0 ? "none" : missing.Count == locales.Count ? "all" : string.Join(",", missing)));
        foreach (var code in show)
        {
            var t = c.GetTable(code) as UnityEngine.Localization.Tables.StringTable;
            var e = t == null ? null : t.GetEntry(shared.Id);
            if (e == null) continue;
            if (!string.IsNullOrEmpty(e.Value)) sb.AppendLine("    " + code + ": " + esc(e.Value));
            var note = meta(e.MetadataEntries);
            if (note.Length > 0) sb.AppendLine("    " + code + " note: " + note);
        }
    }
}
sb.Append(listed + " entries listed");
return sb.ToString();
