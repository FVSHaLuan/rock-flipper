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

Cells are tab-separated; write a newline inside a text as `\n` and a zero-width space as `​` (Thai, see Translate). `strings.cs` prints both back the same way. An empty cell leaves that locale alone.

## Workflow

### 1. Scope the batch

- Translate what the owner named (a table, keys, or "everything missing"), into every locale missing it unless they named locales. `strings.cs ... MISSING=any` lists it.
- **Existing translations stay** unless the owner asks for them to be redone (usually after they changed the English). Then use `OVERWRITE=true`, only for those keys.
- Text that isn't in a table (store page, patch notes, a pasted paragraph): the owner says the format per task. If they don't, reply with the translations inline, one block per locale, and ask before creating anything in `LOC/` (its `CLAUDE.md` says to ask before setting conventions there).

### 2. Get the context for each entry

Read each entry's metadata first. Comments sit in two places, the shared entry (`note:`) and the English entry (`en note:`), and they often explain a term or an abbreviation: `BK.GR`'s English note says it's "Background" shortened to fit the UI. A note in a target locale (`<code> note:` with `SHOW`) applies to that translation. Where the notes don't settle the meaning:
- Find where the text is shown: grep the entry id (`m_KeyId: <id>`) in `Prefabs/` and `Scenes/`, and the key or table name in `Scripts/`. A button label, a tooltip and a sentence need different wording.
- Game terms (Rock, Tier, Pure, Flipper Bot, Skill Tree, Shop, Cash…): use the fixed translation from `LOC/Glossary.tsv` in every locale, and follow `LOC/Glossary.md`. It holds each term's meaning, the do-not-translate list, units and inflection rules. For a game term the glossary lacks, read its meaning in `GDD/Glossary.md` and pick one translation. Keep it as close to the English term's length as possible: a term repeats across labels, buttons and descriptions, so a long one makes all of them longer. Among accurate options, take the one closest in length. Then add the term to both glossary files and list it in the report.
- If you still can't tell what a fragment or abbreviation means, ask. Don't guess.

### 3. Translate

- **Placeholders and markup stay exact.** Keep `{0}`, `{name}` and Smart String syntax (`{count:plural:…}`, `{x:choose(…)…}`), translating only the words inside. Plural forms follow the target language: Russian, Ukrainian, Polish and Czech need more forms, Japanese, Korean, Chinese, Thai and Vietnamese one. Keep rich-text tags (`<b>`, `<color=…>`, `<sprite…>`) around the matching words, which can move within the sentence. Keep `[NEWLINE]` and other `[TOKEN]` shorthands, and `[@…]` escapes, as they are.
- **Don't translate**: the game title `Rock Flipper`, platform and product names (`Steam`, `Steam Deck`), and the rest of the list in `LOC/Glossary.md` (tier codes, monolith names, currency prefixes). The language-name entry `LanguageName` is the language's own name in that language.
- **Register**: match the existing translations in the same locale (`SHOW=<locale>` on a few translated entries). As of 2026-10-18, `de` uses "du", `es` "tú", `fr` "vous". If a locale has nothing to match, ask.
- **Length**: UI labels are short. Stay close to the English length, and prefer the shorter wording for buttons and headers in `de`, `ru`, `pl`, `fr` and `pt-BR`. Respect any limit in the note.
- **Thai word breaks**: Thai has no spaces between words, and TMP can only wrap a line at a space or a zero-width space (U+200B). Without one, it breaks mid-word wherever the line runs out (`LOC/CLAUDE.md`, Decisions). So every `th` text gets `​` between its words, written as that 6-character escape in the TSV (`write.cs` turns it into the character):
  - `คุณ​อาจ​ต้อง​รีสตาร์ท​เกม` ("you may need to restart the game")
  - Split at word boundaries only, never inside a word. Keep a compound that reads as one unit together (ครบถ้วน, รีสตาร์ท), and keep a leading vowel (เ แ โ ใ ไ) with the consonant after it.
  - None at the start or end of the text, and none next to an ordinary space, which already allows a break.
  - Never inside a placeholder, a Smart String's syntax, a rich-text tag, a `[TOKEN]` or a `[@…]` escape. A break just before or after one is fine, and so is one between Thai words inside a Smart String's word options.
  - Single-word labels need none.
- **Problems in the English** (typos, broken grammar, ambiguity): don't fix the English. Translate the evident meaning and list the problem in the report.

### 4. Write

1. `unity status`. The Editor must be connected and in Edit mode.
2. Write the TSV, run `write.cs ... DRY=true`, check the counts and skips, then run it with `DRY=false`.
3. Read it back with `strings.cs ... SHOW=<locales>` and check that nothing is still listed as missing that should have been filled, and that multi-word `th` texts show their `​` breaks.

The tables are existing assets, so there are no new `.meta` files to wait for. If table files change on disk outside the Editor (a `git restore`, a pull), run `eval 'UnityEditor.AssetDatabase.Refresh(); return "ok";'` before writing. Otherwise the Editor writes its stale in-memory copy back over them.

### 5. Report

- what was translated: tables, number of entries × locales, plus anything skipped and why
- game terms added to the glossary, and any place where a glossary term didn't fit (it read wrong in context, or overflowed)
- problems found in the English source
- anything not checked: layout and overflow in game (a screenshot pass is the QA step in the plan, not part of this skill unless asked), including whether Thai wraps at the inserted breaks
