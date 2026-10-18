# Localization workspace for the game Rock Flipper.

## Overview
`LOC/` is the working folder for localization: anything produced or gathered while translating the game that isn't the game's data itself. The owner's decisions are under [Decisions](#decisions). No folder structure, file format or exchange workflow has been decided yet.

## Decisions
Set by the owner on 2026-10-18.
- **Languages**: all 17 locales already in the project: English (source) and 16 targets, Simplified Chinese (zh), Traditional Chinese (zh-TW), Japanese, Korean, Russian, Ukrainian, Polish, Czech, German, French, Spanish (Spain, `es`), Italian, Portuguese-Brazil (pt-BR), Turkish, Thai, Vietnamese. The two pseudo-locales are for testing only.
- **Translation**: AI translation, with no human reviewer.
- **Switching language in game**: partly live. Text that refreshes on a switch changes right away. If a player sees untranslated text left over from switching, they must restart the game. The existing `Common Strings` entry `Reset to apply translation notice` already says this ("You may need to restart the game if it won't fully change to the new language.").
- **Big-number suffixes**: keep the current K/M/B/T suffixes for every language for now. No CJK-style units (万/億).

Set by the owner later on 2026-10-18.
- **Where translations go**: mostly straight into the string table assets. For any other text, the owner sets the format per task.
- **English source changes**: the owner tracks them and asks for re-translation of the changed entries. Nothing needs to detect stale translations, so plan item 5 is dropped.
- **Glossary**: to be built later (plan item 4). Until then, translations keep game terms consistent on their own and report the choices.
- **AI notice**: the `AI translated notice` is shown on the language select screen.
- **Thai line breaking**: zero-width spaces (U+200B) go between Thai words, added during translation (the `translate` skill has the rules). Thai has no spaces between words, and TMP only wraps at spaces and U+200B (it has no Thai word segmentation). Without them, long Thai text breaks mid-word wherever the line runs out, and Auto Size text first shrinks toward its minimum size. Not chosen: a word-splitting tool run over the tables, and splitting at runtime. The Thai translations made before this decision (`Common Strings`) got their breaks on 2026-10-18.

## Proposed Plan
Proposed on 2026-10-18 after an audit of the project. The owner hasn't confirmed it item by item, so treat it as the default direction and check before deviating. The audit (what's wired, what isn't, runtime pitfalls) is in [Scripts/Localization/CLAUDE.md](../Rock%20Flipper/Assets/_AMainGame/Scripts/Localization/CLAUDE.md).

1. **Runtime fixes** (toolkit, reusable):
   - Add a Steam language locale selector, gated by `#if !DISABLESTEAMWORKS`. Place it after PlayerPrefs and before the system locale. It maps Steam API codes (`schinese`, `koreana`, `brazilian`…; see [SteamLanguages.md](Reference/SteamLanguages.md)) to project locales.
   - Give every locale an English fallback. Done on 2026-10-18.
   - Show the "No translation found" message only in the Editor and dev builds.
   - Centralize number formatting.
2. **Hook up all English text**:
   - Static UI: the TMP **Setup Localization** context menu.
   - Player-facing `string` fields: change them to `LocalizedString`, using Smart Strings in place of `{0}` formats.
   - Literals in code: move them into `LocalizedStrings` / `SpecificLocalizedStrings`.
   - Have converted types implement `ILocalizable`, so `UnlocalizedObjectFinder` tracks coverage.
3. **Tables and keys**:
   - Keep `Common` (toolkit) separate from `Rock Flipper` (game), with game tables for UI, Skills and Content.
   - Move or remove the previous game's leftover keys from `Common`.
   - Keep keys generated from the English text for generic words. Use semantic keys (e.g. `skill.flipRange.desc`) for game content.
4. **Source quality for AI translation**: since nobody reviews the output, give the AI the context:
   - a glossary built from [GDD/Glossary.md](../GDD/Glossary.md), including terms not to translate
   - a translator comment on every entry
   - length limits where layout is tight
5. ~~**Stale translations**~~: dropped. The owner manages English source changes (see Decisions).
6. **Translate after the text freezes**, once balancing stops changing the wording.
7. **QA**:
   - a pseudo-locale pass for missed text and about 30% text expansion
   - a screenshot sweep per language for overflow, with extra care for CJK
   - Thai line breaking: check that long Thai text wraps at the zero-width spaces (see Decisions)
8. **Steam store page**: localize the description and any text in the capsule art.

## Open Questions
- Whether language buttons also get an "(AI)" tag (`LanguageSetterButton` supports one). The notice itself is decided (see Decisions).
## Contents
- `Reference/`: external reference material (platform and store rules, third-party docs), one Markdown file per source, named in PascalCase after its topic. Each file links its source and the date it was copied.
  - [SteamLanguages.md](Reference/SteamLanguages.md): the languages Steam supports, at both levels (Full Platform Support and Game Support Only), with Steamworks API and Web API language codes.

## Relation to the Unity Project
- The **shipping** localization data lives in the Unity project, not here: Unity Localization settings, locales and string tables are in `Rock Flipper/Assets/_AMainGame/Data/Localization/` (`Locales/`, `Tables/`, `Localization Settings.asset`, `LocalizedStrings.asset`, `SpecificLocalizedStrings.asset`). Text content data is in `Rock Flipper/Assets/_AMainGame/Texts/`.
- The localization code (helpers, editor tools, export/import) and a coverage audit are documented in [Scripts/Localization/CLAUDE.md](../Rock%20Flipper/Assets/_AMainGame/Scripts/Localization/CLAUDE.md).
- `LOC/` sits outside `Rock Flipper/`, so Unity doesn't import it and files here need no `.meta` files.
- Nothing in `LOC/` reaches the build on its own. Moving work from here into the string tables (or back) is a deliberate step, not something to assume happens.

## Relation to the GDD
- Game terms come from [GDD/Glossary.md](../GDD/Glossary.md). Use the GDD names, not code identifiers, when writing source text or translation context.
- Localization work doesn't change the design. Don't edit the GDD from here unless asked.

## Working Here
- Two skills cover the work: `localize` makes text localizable (string table entries, `LocalizedString` in code, prefab and scene bindings), and `translate` fills in the other languages.
- Until the owner sets conventions, ask before creating a folder structure, file format or naming scheme here. Once one is agreed, record it in this file.
- Keep this file up to date as the folder gains content, so it stays the index of what's in `LOC/`.
