---
name: localize
description: Localize player-facing text in Rock Flipper with Unity Localization. Covers finding or reusing string table entries and the LocalizedStrings/SpecificLocalizedStrings term holders, splitting text fixed in code into reusable terms, [TOKEN] shorthands, converting code-driven text (displayers, string.Format fields) to LocalizedString, binding TMP text and fonts with GameObjectLocalizer, the serialized LocalizedString YAML, and verifying the result in the connected Editor. Use when asked to localize or "hook up" a component, prefab, screen, label or string, to turn hard-coded or static text into localized text, or to add a localized string entry. Translating the text into other languages is the translate skill.
---

# Localize text

Read these first. This skill is the working checklist; they hold the facts:
- `Rock Flipper/Assets/_AMainGame/Scripts/Localization/CLAUDE.md`: helpers, tables, settings, the coverage audit.
- `LOC/CLAUDE.md`: the owner's decisions (16 locales, AI translation, partly-live language switch) and the proposed plan.

## 1. Find out how the text gets on screen

- **Static text** (typed into a TMP component, never set by code): use the TMP context menu **Setup Localization**, then pick or create an entry. Done.
- **Code-driven text** (a script writes `.text`/`UnifiedText.Text`, often via `string.Format`): the text must be localized in the script. A `GameObjectLocalizer` on the same TMP may only bind the font and material (step 6).
- **A literal in code** (`"Lv. {level}"`, `UnlockText + cost`): break it into terms (step 3) instead of making one entry for the whole text.
- Find every user of the component (`grep` the script's GUID from its `.meta` in `Prefabs/` and `Scenes/`), and check scenes for overrides of the fields you'll change: `grep -n "propertyPath: <field>" Scenes/*.unity`.

## 2. Reuse terms and entries before creating any

### Term holders: `LocalizedStrings` and `SpecificLocalizedStrings`

Two ScriptableObjects expose single terms to code as `LocalizedString` fields, so any script can use a term without a serialized field of its own. Check them first whenever code needs a word or short label:

| | `LocalizedStrings` | `SpecificLocalizedStrings` |
|---|---|---|
| Holds | Toolkit terms most games would use (OK, Cancel, Play, Level, Cost…) | Rock Flipper terms (Rock, Pure Rock, Monolith, Flipper Bot…) |
| Access | `LocalizedStrings.Instance` (`entry.localizedStrings`) | `SpecificLocalizedStrings.Instance` (`entry.specificLocalizedStrings`) |
| Asset | `Data/Localization/LocalizedStrings.asset` | `Data/Localization/SpecificLocalizedStrings.asset` |
| Entries in | `Common Strings` | `Rock Flipper Strings` |
| Fields (2026-10-18) | `stage`, `slot`, `on`, `off`, `speed`, `damage`, `cash`, `maxed`, `cost`, `recommended`, `level`, `lvl`, `exp`, `naInDemo`, `moreInFullGame`, `allUnlocked`, plus 2 sentences | background, item, rarity and Level/Chest bar terms and sentences |

- Read the class for the current fields; the table above is a snapshot.
- To decide which holder: would another of the owner's games use the word? Then `LocalizedStrings`, otherwise `SpecificLocalizedStrings`.
- **A term that's missing**: add a `public LocalizedString <term>;` field (camelCase of the term) to the right class, reuse or create its table entry, and set the field on the asset (YAML in step 6). Then any later text can reuse it.
- `SpecificLocalizedStrings.Instance` reads `Entry.Instance` only, so it fails in edit mode. `LocalizedStrings.Instance` falls back to `DevEntry` there. In an `eval`, load the asset with `AssetDatabase.LoadAssetAtPath`.
- The two existing callers (`Home/RunDataSlotView`, `Game Setting/UI/ResolutionSlider`) call `GetLocalizedString()` once, so their text doesn't change on a language switch. New code should redraw on a switch (step 5).

### Table entries

`Common Strings` already has 99 keys, most translated in all 16 other locales (e.g. `Now Playing`, `OK`, `Settings`, `Level`). Not every key has a term-holder field. Search before adding:

```bash
cd "Rock Flipper/Assets/_AMainGame/Data/Localization/Tables/Common/Common Strings"
grep -n -B1 "m_Key: Now Playing" "Common Strings Shared Data.asset"     # m_Id is on the line above
id=<m_Id>; for f in Common\ Strings_*.asset; do echo "$f: $(grep -A1 "m_Id: $id" "$f" | grep m_Localized)"; done
```

- Toolkit text goes in `Common Strings`, game-specific text in a `Rock Flipper` table.
- Prefer putting only the translatable phrase in the table and keeping rich-text tags and symbols (`<b>`, `▶`) in the code-side format. It lets you reuse plain entries and keeps markup away from the AI translator.
- A new entry: **Create Localized String Entry** (or `LocalizationUtility.CreateEnglishEntry`), then save the project, because the entry is only marked dirty. It is English only until translated. Ask the user whether to translate it into the other 16 locales now (the `translate` skill), since the plan is to translate once the text freezes.
- Give a new entry a translator comment when the English alone is ambiguous (a fragment, an abbreviation, a word with several meanings), saying where it's shown and what it means. The translation is AI with no human review, and the comment is its only context. Single terms almost always need one: "Cost" (noun, a price label), "Rock" (the game object, not the music genre).

## 3. Break fixed text into terms

When the text is fixed in code, don't create one entry for the whole string. Split it into terms, reuse existing ones (step 2), and create the missing ones as term-holder fields so later texts can reuse them.

- **What becomes a term**: labels, nouns, UI words and stat names. Each is its own entry and term-holder field.
- **What stays in code**: the glue. Numbers and values, separators and punctuation (`: `, ` / `, `x`), symbols, rich-text tags, and how the parts are arranged on screen.
- Examples from the coverage audit:
  - `UnlockText + cost` → `Unlock` (in `Common Strings`, no field yet: add `unlock` to `LocalizedStrings`) + the cost formatted in code.
  - `"Lv. {level}"` → the `Level` entry, through a new `level` field, if the full word fits. If the layout needs the abbreviation, add a separate `Lv` term with a comment saying it's the short form of Level.
  - `"Tutorial Task [x/y]:"` → a `Tutorial Task` term + `" {x}/{y}:"` built in code.
- **Where to stop splitting**: keep words that depend on each other grammatically in one entry. Word order, gender, case and plurals change between languages, and code that glues words together can't follow them.
  - A noun phrase is one term: `Pure Rock`, not `Pure` + `Rock`.
  - A sentence is one entry, with a placeholder (`{0}` or a named Smart String argument) for each value. `Run/Shop/Backgrounds/PreferredBackgroundStatus.cs`'s status sentences are like that.
  - Fill a sentence's placeholder with a term only when the sentence doesn't have to inflect it (a name, or a label shown after a colon). Otherwise write the word into the sentence's entry.
- If unsure whether a split will translate cleanly, ask the user, showing the planned terms.

## 4. Use shorthands where they fit

`ShortHandManager` (`Scripts/ShortHands/`, `entry.shortHandManager.Apply(str)`) replaces `[TOKEN]`s in text at display time. The dictionary is in its `Init()`. As of 2026-10-18 there's only `[NEWLINE]`. `[@text]` escapes to a literal `[text]`. An unknown token logs an error in the Editor.

- When an entry needs something a token provides, write the token into the entry instead of the raw value, e.g. `[NEWLINE]` rather than a literal line break. The translate skill keeps tokens as they are.
- `Apply` only runs inside `GeneralDialogLauncher.Launch`. If you put a token into text shown by another path, call `entry.shortHandManager.Apply(text)` after reading the localized string. Check that path's other texts for literal square brackets, which then need `[@…]`.
- A **new token** fits a value or a styled bit of text that recurs inside many entries and should stay the same everywhere, e.g. a currency icon with its colored name (`[CASH]` returning `<sprite…>` + `LocalizedStrings.Instance.cash`). It's a toolkit code change, so propose it to the user before adding it. A plain word that sits on its own is a term (step 3), not a token.

## 5. Convert the script

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

For text built from term-holder terms (step 3), redraw on every locale change and read the terms during the redraw. `GetLocalizedString()` is synchronous here (preloaded tables, synchronous initialization):

```csharp
protected override void OnEnable()
{
    base.OnEnable();
    LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    Display();
}

private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

private void OnLocaleChanged(Locale _) => Display();

private void Display()
{
    costText.Text = $"{LocalizedStrings.Instance.cost.GetLocalizedString()}: {cost}";
}
```

Subscribing to one term's `StringChanged` works too when the text uses only that term. Don't change the term holder's `LocalizedString` itself (arguments, `LocaleOverride`), because every caller shares it.

## 6. Wire the prefab

- **Localized + plain fallback pairs** (`SkillMetaData.titleGroup`/`localizedTitleGroup` and `title`/`localizedTitle`, `SkillDescriptor.descriptionFormat`/`localizedDescriptionFormat` and `extraDescription`/`localizedExtraDescription`, `GeneralDialogLauncher.message`/`localizedMessage`): in the same edit that sets the `LocalizedString`, **clear the plain string**. Once the localized field is set the plain text is never shown, so a leftover copy only drifts out of date. Take the English for a new entry from the plain field before you clear it. Leave the plain field in the code, since prefabs that haven't been converted still use it.
  - **Prefab variants**: localize the base prefab first. In a variant that overrides the plain field, set the localized field as a variant override, then revert the plain override (`PrefabUtility.RevertPropertyOverride`) so it inherits the base's empty string. Don't leave an override set to "". A variant that doesn't override the plain field inherits the base's localized field and needs no change.
  - **Gotcha**: once the base is localized, a variant's localized field looks set, because it's inherited from the base, even though it holds the base's entry. So a plain override in the variant (`SerializedProperty.prefabOverride`) means the variant needs its own entry. Don't skip it as "already localized" just because the localized field is set. Read the text from the plain override.
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

## 7. Verify in the Editor (no Play mode needed)

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
- For a bulk edit (many prefabs or entries), write the script with a dry-run flag. Run it dry first to check key collisions and the planned entries, then for real. A long run can fail in the CLI with "Main thread operation timed out after 5000ms" while the Editor keeps running it to the end. Don't re-run it blindly. Check `git status` and re-run the dry pass to see what's left.

Play mode testing of the live language switch: ask the user first (root `CLAUDE.md`).

## 8. Report

Say which entries were reused or created (with any translator comment), how a code literal was split into terms, which term-holder fields were added, any shorthand used or proposed, which locales are translated and which are English only, what changed on the prefab (including any `m_text` binding you removed), and what was or wasn't tested.
