---
name: translate
description: Translate Rock Flipper's English text into the game's other languages. Lists untranslated string table entries, translates them with game context (placeholders, Smart Strings, rich text, length, register per language) and writes them straight into the Unity Localization string tables, or translates text the owner pastes. Use when asked to translate, fill in missing translations, translate a table, key or locale, re-translate entries whose English changed, or translate other text (store page, patch notes) into the game's languages. Hooking text up to localization in code, prefabs or scenes is the localize skill.
---

# Translate text

Read these first. This skill is the working checklist; they hold the facts:
- `LOC/CLAUDE.md`: the owner's decisions (languages, AI translation with no human review) and the plan.
- `Rock Flipper/Assets/_AMainGame/Scripts/Localization/CLAUDE.md`: the tables, locales and fallbacks.

**Out of scope:** making text localizable (new entries, `LocalizedString`, prefab bindings) is the `localize` skill. The English source belongs to the owner: never edit English entries, and don't hunt for stale translations, since the owner tracks changes to the source.

## Targets

16 locales besides English: `zh` (Simplified), `zh-TW` (Traditional, Taiwan usage, not a character conversion of `zh`), `ja`, `ko`, `ru`, `uk`, `pl`, `cs`, `de`, `fr`, `es` (Spain), `it`, `pt-BR` (Brazil), `tr`, `th`, `vi`. Skip the two pseudo-locales. `strings.cs` prints the live list.

## Helpers

Run from the repo root (`D:/UP/rock-flipper`) through the playtest skill's runner. `R=.claude/skills/playtest/scripts/run.sh`, `S=.claude/skills/translate/scripts`:

| Command | What it does |
|---|---|
| `bash $R $S/strings.cs TABLE=all KEY=- MISSING=any SHOW=-` | Entries with their key, id, `[smart]` flag, metadata notes (`note:` from the shared entry, `en note:` from the English entry, other metadata types as `<TypeName>`) and English text, plus which locales are missing. `TABLE` is a collection name or `all`; `KEY` a key substring; `MISSING` `any`, a locale code or `-` (all entries); `SHOW` the locales whose current text to print (`de,fr`, `all`, `-`). Read only |
| `bash $R $S/write.cs FILE=<abs path>.tsv OVERWRITE=false DRY=true` | Writes a TSV of translations into one table and saves the changed table assets. Validates the whole file first and writes nothing if any key or locale is unknown. Gives each entry the English entry's Smart flag. `OVERWRITE=false` leaves existing translations alone. Edit mode only |

TSV for `write.cs`, written with the Write tool to the session scratchpad (not a shell heredoc, which mangles `\n` and non-ASCII). One file per table:

```
table	Rock Flipper Strings
# lines starting with # are ignored
key	zh	ja	de
Preferred	首选	お気に入り	Bevorzugt
Dismiss All	全部忽略	すべて閉じる	Alle verwerfen
```

Cells are tab-separated; write a newline inside a text as `\n`. An empty cell leaves that locale alone.

## Workflow

### 1. Scope the batch

- Translate what the owner named (a table, keys, or "everything missing"), into every locale missing it unless they named locales. `strings.cs ... MISSING=any` lists it.
- **Existing translations stay** unless the owner asks for them to be redone (usually after they changed the English). Then use `OVERWRITE=true`, only for those keys.
- Text that isn't in a table (store page, patch notes, a pasted paragraph): the owner says the format per task. If they don't, reply with the translations inline, one block per locale, and ask before creating anything in `LOC/` (its `CLAUDE.md` says to ask before setting conventions there).

### 2. Get the context for each entry

Read each entry's metadata first. Comments sit in two places, the shared entry (`note:`) and the English entry (`en note:`), and they often explain a term or an abbreviation: `BK.GR`'s English note says it's "Background" shortened to fit the UI. A note in a target locale (`<code> note:` with `SHOW`) applies to that translation. Where the notes don't settle the meaning:
- Find where the text is shown: grep the entry id (`m_KeyId: <id>`) in `Prefabs/` and `Scenes/`, and the key or table name in `Scripts/`. A button label, a tooltip and a sentence need different wording.
- Game terms (Rock, Tier, Pure, Flipper Bot, Skill Tree, Shop, Cash…): read their meaning in `GDD/Glossary.md`. There's no translation glossary yet. Pick one translation per term, use it everywhere, and list your choices in the report so the owner can build the glossary from them. Before choosing, check how an already-translated entry renders the term (`KEY=<term> SHOW=all`).
- If you still can't tell what a fragment or abbreviation means, ask. Don't guess.

### 3. Translate

- **Placeholders and markup stay exact.** Keep `{0}`, `{name}` and Smart String syntax (`{count:plural:…}`, `{x:choose(…)…}`), translating only the words inside. Plural forms follow the target language: Russian, Ukrainian, Polish and Czech need more forms, Japanese, Korean, Chinese, Thai and Vietnamese one. Keep rich-text tags (`<b>`, `<color=…>`, `<sprite…>`) around the matching words, which can move within the sentence. Keep `[NEWLINE]` and other `[TOKEN]` shorthands, and `[@…]` escapes, as they are.
- **Don't translate**: the game title `Rock Flipper`, platform and product names (`Steam`, `Steam Deck`). The language-name entry `LanguageName` is the language's own name in that language.
- **Register**: match the existing translations in the same locale (`SHOW=<locale>` on a few translated entries). As of 2026-10-18, `de` uses "du", `es` "tú", `fr` "vous". If a locale has nothing to match, ask.
- **Length**: UI labels are short. Stay close to the English length, and prefer the shorter wording for buttons and headers in `de`, `ru`, `pl`, `fr` and `pt-BR`. Respect any limit in the note.
- **Thai**: TMP doesn't break lines between Thai words. The fix is deferred (`LOC/CLAUDE.md`, Open Questions), so translate Thai normally and don't insert zero-width spaces (U+200B) unless the owner asks.
- **Problems in the English** (typos, broken grammar, ambiguity): don't fix the English. Translate the evident meaning and list the problem in the report.

### 4. Write

1. `unity status`. The Editor must be connected and in Edit mode.
2. Write the TSV, run `write.cs ... DRY=true`, check the counts and skips, then run it with `DRY=false`.
3. Read it back with `strings.cs ... SHOW=<locales>` and check that nothing is still listed as missing that should have been filled.

The tables are existing assets, so there are no new `.meta` files to wait for. If table files change on disk outside the Editor (a `git restore`, a pull), run `eval 'UnityEditor.AssetDatabase.Refresh(); return "ok";'` before writing. Otherwise the Editor writes its stale in-memory copy back over them.

### 5. Report

- what was translated: tables, number of entries × locales, plus anything skipped and why
- term choices made for game words, per locale where they differ meaningfully
- problems found in the English source
- anything not checked: layout and overflow in game (a screenshot pass is the QA step in the plan, not part of this skill unless asked), Thai line breaking
