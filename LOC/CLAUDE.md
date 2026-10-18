# Localization workspace for the game Rock Flipper.

## Overview
`LOC/` is the working folder for localization: anything produced or gathered while translating the game that isn't the game's data itself. Apart from the reference material below, no structure, file format or workflow has been decided yet.

## Contents
- `Reference/`: external reference material (platform and store rules, third-party docs), one Markdown file per source, named in PascalCase after its topic. Each file links its source and the date it was copied.
  - [SteamLanguages.md](Reference/SteamLanguages.md): the languages Steam supports, at both levels (Full Platform Support and Game Support Only), with Steamworks API and Web API language codes.

## Relation to the Unity Project
- The **shipping** localization data lives in the Unity project, not here: Unity Localization settings, locales and string tables are in `Rock Flipper/Assets/_AMainGame/Data/Localization/` (`Locales/`, `Tables/`, `Localization Settings.asset`, `LocalizedStrings.asset`, `SpecificLocalizedStrings.asset`). Text content data is in `Rock Flipper/Assets/_AMainGame/Texts/`.
- `LOC/` sits outside `Rock Flipper/`, so Unity doesn't import it and files here need no `.meta` files.
- Nothing in `LOC/` reaches the build on its own. Moving work from here into the string tables (or back) is a deliberate step, not something to assume happens.

## Relation to the GDD
- Game terms come from [GDD/Glossary.md](../GDD/Glossary.md). Use the GDD names, not code identifiers, when writing source text or translation context.
- Localization work doesn't change the design. Don't edit the GDD from here unless asked.

## Working Here
- Until the owner sets conventions, ask before creating a folder structure, file format or naming scheme here. Once one is agreed, record it in this file.
- Keep this file up to date as the folder gains content, so it stays the index of what's in `LOC/`.
