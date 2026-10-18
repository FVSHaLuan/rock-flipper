---
name: localize
description: Localize player-facing text in Rock Flipper with Unity Localization. Covers finding or reusing string table entries, converting code-driven text (displayers, string.Format fields) to LocalizedString, binding TMP text and fonts with GameObjectLocalizer, the serialized LocalizedString YAML, and verifying the result in the connected Editor. Use when asked to localize, translate or "hook up" a component, prefab, screen, label or string, or to add a localized string entry.
---

# Localize text

Read these first. This skill is the working checklist; they hold the facts:
- `Rock Flipper/Assets/_AMainGame/Scripts/Localization/CLAUDE.md`: helpers, tables, settings, the coverage audit.
- `LOC/CLAUDE.md`: the owner's decisions (16 locales, AI translation, partly-live language switch) and the proposed plan.

## 1. Find out how the text gets on screen

- **Static text** (typed into a TMP component, never set by code): use the TMP context menu **Setup Localization**, then pick or create an entry. Done.
- **Code-driven text** (a script writes `.text`/`UnifiedText.Text`, often via `string.Format`): the text must be localized in the script. A `GameObjectLocalizer` on the same TMP may only bind the font and material (step 4).
- Find every user of the component (`grep` the script's GUID from its `.meta` in `Prefabs/` and `Scenes/`), and check scenes for overrides of the fields you'll change: `grep -n "propertyPath: <field>" Scenes/*.unity`.

## 2. Reuse an entry before creating one

`Common Strings` already has 91 keys translated in all 16 locales (e.g. `Now Playing`, `OK`, `Settings`, `Level`). Search before adding:

```bash
cd "Rock Flipper/Assets/_AMainGame/Data/Localization/Tables/Common/Common Strings"
grep -n -B1 "m_Key: Now Playing" "Common Strings Shared Data.asset"     # m_Id is on the line above
id=<m_Id>; for f in Common\ Strings_*.asset; do echo "$f: $(grep -A1 "m_Id: $id" "$f" | grep m_Localized)"; done
```

- Toolkit text goes in `Common Strings`, game-specific text in a `Rock Flipper` table.
- Prefer putting only the translatable phrase in the table and keeping rich-text tags and symbols (`<b>`, `▶`) in the code-side format. It lets you reuse plain entries and keeps markup away from the AI translator.
- A new entry: **Create Localized String Entry** (or `LocalizationUtility.CreateEnglishEntry`), then save the project, because the entry is only marked dirty. It is English only until translated. Ask the user whether to add the other 15 locales now, since the plan is to translate once the text freezes.

## 3. Convert the script

Follow the existing "localized if set, else plain" pattern (`SkillMetaData`, `GeneralDialogLauncher`, using `Agame.Localization.IsValidAndNotEmpty()`), so prefabs can move over one at a time.

For a format-based displayer, keep the `string format` field and feed the localized phrase in as an extra placeholder. The old format keeps working, and an unset label fills in as empty:

```csharp
[SerializeField, Tooltip("{0}: music name, {1}: label")]
private string format = "{0}";
[SerializeField, Tooltip("Fills {1} in format, e.g. \"Now Playing\"")]
private LocalizedString localizedLabel;

private string label = string.Empty;

protected override void OnEnable()
{
    base.OnEnable();
    if (localizedLabel.IsValidAndNotEmpty())
        localizedLabel.StringChanged += OnLabelChanged;   // fires now and on every locale change
}

private void OnDisable() => localizedLabel.StringChanged -= OnLabelChanged;

private void OnLabelChanged(string value) { label = value; ForceDisplay(); }
```

- Subscribing to `StringChanged` makes the text update live on a language switch. A one-off `GetLocalizedString()` doesn't (LOC: "text that refreshes on a switch changes right away").
- `ValueDisplayer<T>` only redraws when its value changes, so call `ForceDisplay()` after the label changes.
- If you convert something listed in the coverage audit in `Scripts/Localization/CLAUDE.md`, remove it from that list.

## 4. Wire the prefab

- **Code-driven TMP**: Setup Localization adds three tracked properties: `m_text`, `m_fontAsset` and `m_sharedMaterial`. Keep the font and material, which CJK/Thai need. **Remove the `m_text` property**, because the script owns the text and the two would fight.
- Serialized `LocalizedString` YAML. The table is referenced by the **Shared Data asset's GUID**:

```yaml
  localizedLabel:
    m_TableReference:
      m_TableCollectionName: GUID:7da8669fdb0c01b47a5b9e567d12a3f1   # Common Strings Shared Data.asset.meta
    m_TableEntryReference:
      m_KeyId: 512897407561728                                         # the entry's m_Id
      m_Key: 
    m_FallbackState: 0
    m_WaitForCompletion: 0
    m_LocalVariables: []
```

- Run `unity status` first. With a connected Editor, set fields through `unity-cli` (SerializedObject in `eval`/`eval_file`) rather than by hand. If you do hand-edit YAML, use the Edit tool, not a Python/sed script: escapes like `▶` in the file get mangled through the shell, and the exact match fails.
- Scene edits the user didn't ask for: ask first (root `CLAUDE.md`).

## 5. Verify in the Editor (no Play mode needed)

1. `eval 'UnityEditor.AssetDatabase.Refresh(); return "ok";'`, then `recompile`, poll `recompile_status` until `completed`, then `get_console_logs --severity error`.
2. Load the prefab, read the fields with `SerializedObject`, and render the string for a few locales.

**Gotcha:** `LocalizedString` has **no `GetLocalizedString(Locale)` overload**. `ls.GetLocalizedString(locale)` binds to `params object[] arguments` and quietly returns the *current* locale's text. To read a specific locale, use either of:

```csharp
LocalizationSettings.StringDatabase.GetLocalizedString("Common Strings", keyId, locale);
ls.LocaleOverride = locale; ls.GetLocalizedString();
```

Other eval notes:
- `new LocalizedString("Common Strings", keyId)` works; `TableReference.TableCollectionName` is read-only.
- Use `eval_file` with a scratchpad `.cs` for anything multi-line.

Play mode testing of the live language switch: ask the user first (root `CLAUDE.md`).

## 6. Report

Say which entry was reused or created, which locales are translated and which are English only, what changed on the prefab (including any `m_text` binding you removed), and what was or wasn't tested.
