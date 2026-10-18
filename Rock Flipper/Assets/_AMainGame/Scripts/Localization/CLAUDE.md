# Scripts/Localization — Unity Localization helpers

15 files. A reusable toolkit layer on top of **Unity Localization** (`com.unity.localization` 1.5.13), carried over from a previous game. The localization *strategy and decisions* (languages, translation method, workflow) live in [LOC/CLAUDE.md](../../../../../LOC/CLAUDE.md). This file describes the code and data as they are.

## Files

- `LocalizedStrings.cs` — `ScriptableObjectWithInit` holding generic `LocalizedString` fields (stage, slot, on/off, speed, damage, cash, maxed, cost, recommended). Reached via `LocalizedStrings.Instance` (`Entry.Instance.localizedStrings`; in edit mode `DevEntry.Instance.localizedStrings`). Asset: `Data/Localization/LocalizedStrings.asset`. Toolkit side.
- `SpecificLocalizedStrings.cs` — same pattern for game-specific strings (`Entry.Instance.specificLocalizedStrings`). Several fields (jumpy, dark, ball, lightning bolts, aura wave) are leftovers from the previous game. Asset: `Data/Localization/SpecificLocalizedStrings.asset`.
- `Dev/ILocalizable.cs` — editor interface (`Editor_IsLocalized`, `Editor_TryCreatingLocalizationTerms`). **Nothing implements it yet.**
- `Dev/UnlocalizedObjectFinder.cs` — SO with a context menu that lists every `ILocalizable` in chosen folders and the open scene whose `Editor_IsLocalized` is false. Only useful once types implement `ILocalizable`.
- `UnityLocalization/LocalizationUtility.cs` — `CreateEnglishEntry(collection, key, text)` (editor only) and `GenerateKey(text)` (strips formatting and non-alphanumerics, max 70 chars). Keys are derived from the English text.
- `UnityLocalization/LocalizedStringExtension.cs` — `IsValidAndNotEmpty()`; used for "localized if set, else plain string" fallbacks (see `SkillMetaData`).
- `UnityLocalization/UnityLocalizationHelperSettings.cs` — SO with the default `LocalizedTmpFont` + `LocalizedMaterial` (point at the `Common Assets` table's `NotoFont` / `NotoFontMaterial`). Asset: `Data/Localization/UnityLocalizationHelperSettings.asset` (auto-created in editor if missing).
- `UnityLocalization/LanguageSetterButton.cs` — sets `LocalizationSettings.SelectedLocale`; editor context menu fills the button label in the target language/font, with an optional " (AI)" suffix. Used by `Prefabs/UI/Home/Language Button.prefab` (copied from Incoremental): one instance per locale (17, every non-pseudo locale) under `Lanaguage Select Screen/Wrapper/Language Menu` in `Main.unity`. Run the context menu after changing a button's locale. The screen itself is `Scripts/Home/LanguageSelectScreen.cs`.
- `UnityLocalization/Editor/LocalizedTextMeshProAutoSetup.cs` — TMP context menu **Setup Localization**: adds a `GameObjectLocalizer`, tracks `m_text`, and binds `m_fontAsset`/`m_sharedMaterial` to the per-locale font from `UnityLocalizationHelperSettings`, then opens the picker. `GameObjectLocalizer` context menus: **Search For Localized Terms**, **Create Localized String Entry** (creates an English entry keyed by `GenerateKey`).
- `UnityLocalization/Editor/LocalizedStringPicker.cs` — search window for existing entries by text.
- `UnityLocalization/Editor/GlossaryUtility.cs` — dumps every English value of chosen tables into a text field.
- `UnityLocalization/Editor/String Table IO/` — `LocalizedStringsExporter` / `LocalizedStringsImporter` SOs + `StringTableIO` (JSON). Export takes an explicit list of `LocalizedString`s, `All` or `UntranslatedOnly`, and includes shared and per-locale comments. **Import only fills empty entries; it never overwrites an existing translation**, so a changed English source leaves stale translations with nothing flagging them.

## Data (`Data/Localization/`)

- **Locales**: en (project locale) plus zh, zh-TW, ja, ko, ru, uk, pl, cs, de, fr, es, it, pt-BR, tr, th, vi, and two pseudo-locales. Only zh-TW has a fallback locale set.
- **String tables** — `Common/` is toolkit, `Rock Flipper/` is game-specific:
  - `Common Strings` (default string table): 91 keys, translated in all 16 locales. Includes the language-switch strings `AI translated notice` and `Reset to apply translation notice` ("You may need to restart the game if it won't fully change to the new language."). Some keys are leftovers from the previous game (`game.Rock Crusher`, `Boss Level`, `Beat All Bosses`, Animal/Food/Fantasy/Space).
  - `Test Strings`: 1 test key.
  - `Rock Flipper Strings`: 6 placeholder keys (`P0 Rock`…`P5 Rock`), English only.
  - `Rock Flipper Skill Strings`: 1 key (`Max Rock Count`), English only.
  - `Common Assets` (default asset table): `NotoFont` / `NotoFontMaterial` per locale (Noto Sans, NotoSansJP/KR/SC/TC/Thai). The fonts must be Addressable: Noto Sans in `Localization-Assets-Shared`, the others in `Localization-Assets-<locale>`. A font that isn't throws `InvalidKeyException` when its locale's text loads. Assign entries through the table editor (or `AssetTableCollection.AddAssetToTable`), which creates the group entries. Copying table files alone doesn't.
- **Localization Settings.asset**:
  - Startup selectors in order: command line (`-language=`) → PlayerPrefs → system (OS) locale → `en`. **No Steam language selector**: the Steam client language is ignored.
  - String database: `m_UseFallback: 1`, but `m_MissingTranslationState: -1` (all flags), so a missing entry shows "No translation found for '…'" to players, in shipping builds too.
  - Preload behavior 2, synchronous initialization on.
  - `Common Strings`, `Common Assets`, `Rock Flipper Strings` and `Rock Flipper Skill Strings` are flagged **Preload** for every locale (a `Preload` label on their Addressables entries); `Test Strings` isn't. Flag any new real table too. The language buttons switch the locale on hover, and without preloading a quick hover across them makes queued string lookups run on released table handles ("Load Table Operation Failed", "Attempting to use an invalid operation handle").
- Fonts (`_AMainGame/Fonts/`): every TMP font asset is dynamic. Main UI font is `NotoSans-Regular SDF`. TMP has no Thai word segmentation, so long Thai text won't wrap without zero-width spaces.

## Coverage audit (2026-10-18)

A snapshot. Re-check before relying on the numbers.

- **No localized UI in the game**: 0 `GameObjectLocalizer`s in `Main.unity`, `Run.unity` or `_AMainGame` prefabs (about 36 + 22 scene texts and 115 prefab texts with content). The only one in the project is in `_Exp/`.
- `LocalizedStrings` / `SpecificLocalizedStrings` are read in only 2 places in code.
- **Plain `string` fields shown to players** (need `LocalizedString`):
  - `Run/Skill tree/Graph/SkillDescriptor.cs` — `descriptionFormat` (`string.Format` with `{0}`; 46 prefabs) and `extraDescription`.
  - `Run/Skill tree/Graph/SkillMetaData.cs` — `title` / `titleGroup` (44 prefabs). Has `localizedTitle` / `localizedTitleGroup` with fallback, but none are set.
  - `Run/Skill tree/SkillNode.cs` — `description`.
  - `Run/Combat/Tutorials/TutorialTaskController.cs` — `taskDescription`.
  - `Run/Currencies/CurrencyConfigAsset.cs` — `header`.
  - `Run/Skill tree/Special Crusher Configs/SpecialCrusherConfigAsset.cs` — `specialCrusherDescription`.
  - `UI/General Dialog/GeneralDialog.cs`, `GeneralDialogLauncher.cs` — `message`.
  - `UI Systems/Horizontal Menu/HorizontalMenuItem.cs` — `title`.
  - `UI Systems/ButtonPrompt/ContextButtonPrompt/ContextButtonPrompt.cs` — `text`.
- **Literals in code**:
  - `Run/Combat/Tutorials/TutorialTaskManager.cs` — `"Tutorial Task [x/y]:"`.
  - `Run/Shop/Items/ItemView.cs` — `"Lv. {level}"`.
  - `Run/Shop/Backgrounds/PreferredBackgroundStatus.cs` — two status sentences.
  - `Run/Shop/Backgrounds/UnlockBackgroundButton.cs` — `UnlockText` + cost.
- **Numbers**: `Helpers/PureSripting/BigNumber/BigNumberString.cs` uses fixed English suffixes (K, M, B, T, then letter pairs). Decimal formatting follows the OS culture implicitly; no code sets a culture. The only culture-sensitive parsing outside the editor is in `FHC/Core/Architecture/SimpleJSON.cs`, used only by `Marketing/Steam Discount/SteamStoreStateDetector.cs`.

## Conventions / gotchas

- Toolkit vs game: `Common` tables, this folder, `LocalizedStrings` = reusable. `Rock Flipper` tables and `SpecificLocalizedStrings` = this game.
- After **Create Localized String Entry**, the new English entry and its shared data are only marked dirty. Save the project for them to reach disk.
- `SkillMetaData`'s "localized if set, else plain string" pattern lets a type move to localization one prefab at a time.
