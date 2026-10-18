# Scripts/Localization — Unity Localization helpers

15 files. A reusable toolkit layer on top of **Unity Localization** (`com.unity.localization` 1.5.13), carried over from a previous game. The localization *strategy and decisions* (languages, translation method, workflow) live in [LOC/CLAUDE.md](../../../../../LOC/CLAUDE.md). This file describes the code and data as they are.

## Files

- `LocalizedStrings.cs` — `ScriptableObjectWithInit` holding generic `LocalizedString` fields: terms (stage, slot, on/off, speed, damage, cash, maxed, cost, recommended, level, `lvl` "Lvl.", `exp` "Exp", `naInDemo`, `moreInFullGame`, `allUnlocked`) and a few toolkit sentences (`costWillBeFullyRefunded`, `unsupportedSaveVersion`). Reached via `LocalizedStrings.Instance` (`Entry.Instance.localizedStrings`; in edit mode `DevEntry.Instance.localizedStrings`). Asset: `Data/Localization/LocalizedStrings.asset`. Toolkit side.
- `SpecificLocalizedStrings.cs` — same pattern for game-specific text (`Entry.Instance.specificLocalizedStrings`), with entries in the `Rock Flipper Strings` table: background terms and status sentences, item tooltip terms, the 5 item rarity names (read through `ChestRarity.GetLocalizedName()`), and the Level/Chest bar tooltips. Sentences live here too when their component is in a scene, so localizing them needs no scene edit. `Instance` has no edit-mode fallback, and `DevEntry` has no reference to it. Asset: `Data/Localization/SpecificLocalizedStrings.asset`.
- `Dev/ILocalizable.cs` — editor interface (`Editor_IsLocalized`, `Editor_TryCreatingLocalizationTerms`). **Nothing implements it yet.**
- `Dev/UnlocalizedObjectFinder.cs` — SO with a context menu that lists every `ILocalizable` in chosen folders and the open scene whose `Editor_IsLocalized` is false. Only useful once types implement `ILocalizable`.
- `UnityLocalization/LocalizationUtility.cs` — `CreateEnglishEntry(collection, key, text)` (editor only) and `GenerateKey(text)` (strips formatting and non-alphanumerics, max 70 chars). Keys are derived from the English text.
- `UnityLocalization/LocalizedStringExtension.cs` — `IsValidAndNotEmpty()` and `GetLocalizedStringOr(plain)`, for "localized if set, else plain string" fallbacks (see `SkillMetaData`, `Item.ItemName`, `CombatBackground.BackgroundName`); `GetLocalizedStringUpper()` (upper case by the selected locale's culture, for all-caps labels like MAXED, so entries stay in normal case); editor-only `Editor_GetEnglishString()` (the entry's English text, read from the table collection; `Item` builds its id from it).
- `UnityLocalization/UnityLocalizationHelperSettings.cs` — SO with the default `LocalizedTmpFont` + `LocalizedMaterial` (point at the `Common Assets` table's `NotoFont` / `NotoFontMaterial`). Asset: `Data/Localization/UnityLocalizationHelperSettings.asset` (auto-created in editor if missing).
- `UnityLocalization/LanguageSetterButton.cs` — sets `LocalizationSettings.SelectedLocale`; editor context menu fills the button label in the target language/font, with an optional " (AI)" suffix. Used by `Prefabs/UI/Home/Language Button.prefab` (copied from Incoremental): one instance per locale (17, every non-pseudo locale) under `Lanaguage Select Screen/Wrapper/Language Menu` in `Main.unity`. Run the context menu after changing a button's locale. The screen itself is `Scripts/Home/LanguageSelectScreen.cs`.
- `UnityLocalization/Editor/LocalizedTextMeshProAutoSetup.cs` — TMP context menu **Setup Localization**: adds a `GameObjectLocalizer`, tracks `m_text`, and binds `m_fontAsset`/`m_sharedMaterial` to the per-locale font from `UnityLocalizationHelperSettings`, then opens the picker. `GameObjectLocalizer` context menus: **Search For Localized Terms**, **Create Localized String Entry** (creates an English entry keyed by `GenerateKey`).
- `UnityLocalization/Editor/LocalizedStringPicker.cs` — search window for existing entries by text.
- `UnityLocalization/Editor/GlossaryUtility.cs` — dumps every English value of chosen tables into a text field.
- `UnityLocalization/Editor/String Table IO/` — `LocalizedStringsExporter` / `LocalizedStringsImporter` SOs + `StringTableIO` (JSON). Export takes an explicit list of `LocalizedString`s, `All` or `UntranslatedOnly`, and includes shared and per-locale comments. **Import only fills empty entries; it never overwrites an existing translation**, so a changed English source leaves stale translations with nothing flagging them.

## Data (`Data/Localization/`)

- **Locales**: en (project locale) plus zh, zh-TW, ja, ko, ru, uk, pl, cs, de, fr, es, it, pt-BR, tr, th, vi, and two pseudo-locales. Every non-English, non-pseudo locale has a **Fallback Locale** metadata pointing to `en`, except zh-TW, which falls back to zh (and so on to `en`). Add the same metadata to any new locale.
- **String tables** — `Common/` is toolkit, `Rock Flipper/` is game-specific:
  - `Common Strings` (default string table): 106 keys. 90 are translated in every locale; the 16 newest (`Unlock`, `Shop`, `Skill Tree`, `Items`, `Demo`, `Lvl`, `Exp`, `NA in demo`…) are English only (2026-10-18). Includes the language-switch strings `AI translated notice` and `Reset to apply translation notice` ("You may need to restart the game if it won't fully change to the new language."). Some keys are leftovers from the previous game (`game.Rock Crusher`, `Boss Level`, `Beat All Bosses`, Animal/Food/Fantasy/Space).
  - `Test Strings`: 1 test key.
  - `Rock Flipper Strings`: 53 keys (`P0 Rock`…`P5 Rock` placeholders, the skill title groups `Backgrounds`, `Flipper Bots`, `Mouse`, `All Rocks` and `Monolith L II`…`Monolith L XII`, Shop, Items and demo-notice text, the `SpecificLocalizedStrings` terms and sentences, the 8 non-placeholder item names and the 5 background names), English only. Most new entries carry a translator comment.
  - `Rock Flipper Skill Strings`: 93 keys, the BuildAgent prefabs' skill titles and description formats, English only.
  - `Common Assets` (default asset table): `NotoFont` / `NotoFontMaterial` per locale (Noto Sans, NotoSansJP/KR/SC/TC/Thai). The fonts must be Addressable: Noto Sans in `Localization-Assets-Shared`, the others in `Localization-Assets-<locale>`. A font that isn't throws `InvalidKeyException` when its locale's text loads. Assign entries through the table editor (or `AssetTableCollection.AddAssetToTable`), which creates the group entries. Copying table files alone doesn't.
- **Localization Settings.asset**:
  - Startup selectors in order: command line (`-language=`) → PlayerPrefs → system (OS) locale → `en`. **No Steam language selector**: the Steam client language is ignored.
  - String database: `m_UseFallback: 1`, so an entry that is missing or empty in the selected locale shows the fallback locale's text (English). `m_MissingTranslationState: -1` (all flags) means "No translation found for '…'" still shows, in shipping builds too, when the English entry is missing as well.
  - Preload behavior 2, synchronous initialization on.
  - `Common Strings`, `Common Assets`, `Rock Flipper Strings` and `Rock Flipper Skill Strings` are flagged **Preload** for every locale (a `Preload` label on their Addressables entries); `Test Strings` isn't. Flag any new real table too. The language buttons switch the locale on hover, and without preloading a quick hover across them makes queued string lookups run on released table handles ("Load Table Operation Failed", "Attempting to use an invalid operation handle").
- Fonts (`_AMainGame/Fonts/`): every TMP font asset is dynamic. Main UI font is `NotoSans-Regular SDF`. TMP has no Thai word segmentation, so long Thai text won't wrap without zero-width spaces.

## Coverage audit (2026-10-18)

A snapshot. Re-check before relying on the numbers.

- **No localized UI in the game**: 0 `GameObjectLocalizer`s in `Main.unity`, `Run.unity` or `_AMainGame` prefabs (about 36 + 22 scene texts and 115 prefab texts with content). The only one in the project is in `_Exp/`.
- Code-built text localized through the term holders: background status/unlock button/list item, item view and tooltip (rarity included), Shop stat button and Skill Tree tooltip (Lvl., MAXED, refund note), Level and Chest bar tooltips, the old-save dialog. Item names (`Item.localizedItemName`, 8 non-placeholder items) and background names (`CombatBackground.localizedBackgroundName`, in `Run.unity`) are localized; placeholder items still use plain `itemName`.
- **Plain `string` fields shown to players** (need `LocalizedString`):
  - `Run/Skill tree/SkillNode.cs` — `description`.
  - `Run/Combat/Tutorials/TutorialTaskController.cs` — `taskDescription`.
  - `Run/Currencies/CurrencyConfigAsset.cs` — `header`.
  - `Run/Skill tree/Special Crusher Configs/SpecialCrusherConfigAsset.cs` — `specialCrusherDescription`.
  - `UI/General Dialog/GeneralDialog.cs` — `message`. `GeneralDialogLauncher.cs` has `localizedMessage` with fallback to `message`, but none are set.
  - `UI Systems/Horizontal Menu/HorizontalMenuItem.cs` — `title`.
  - `UI Systems/ButtonPrompt/ContextButtonPrompt/ContextButtonPrompt.cs` — `text`.
- **Literals in code**:
  - `Run/Combat/Tutorials/TutorialTaskManager.cs` — `"Tutorial Task [x/y]:"`, `"Hint:"` (not used by any scene or prefab).
  - `Run/Currencies/CurrencyEarningSpeedDisplayer.cs` — the `"/s"` suffix.
  - `UI/Feedback/FeedbackPopup.cs` — `"Sending..."`, `"Done: {result}. Thank you!"`.
  - Unused scripts: `UI/Audio Setting/MusicToggleButton.cs` (TURN MUSIC ON/OFF), `ContextButtonPromptView.cs` (`[HOLD]`), `HorizontalMenuListItem.cs` (`[LOCKED]`), `Common/TextConst.cs`.
- **Numbers**: `Helpers/PureSripting/BigNumber/BigNumberString.cs` uses fixed English suffixes (K, M, B, T, then letter pairs). Decimal formatting follows the OS culture implicitly; no code sets a culture. The only culture-sensitive parsing outside the editor is in `FHC/Core/Architecture/SimpleJSON.cs`, used only by `Marketing/Steam Discount/SteamStoreStateDetector.cs`.

## Conventions / gotchas

- Toolkit vs game: `Common` tables, this folder, `LocalizedStrings` = reusable. `Rock Flipper` tables and `SpecificLocalizedStrings` = this game.
- After **Create Localized String Entry**, the new English entry and its shared data are only marked dirty. Save the project for them to reach disk.
- `SkillMetaData`'s "localized if set, else plain string" pattern lets a type move to localization one prefab at a time.
