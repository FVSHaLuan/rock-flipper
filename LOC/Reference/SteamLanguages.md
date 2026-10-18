# Steam Languages

Source: [Steamworks Documentation › Languages Supported on Steam](https://partner.steamgames.com/doc/store/localization/languages), copied 2026-10-18. Valve updates this list from time to time, so check the source before relying on it for a release.

## Two levels of support

- **Full Platform Support** (31 languages): players can run the Steam client and browse the Store in these languages, and Valve translates the Steam client UI, Store and Community into them. **This is the only set supported by the Steamworks APIs.**
- **Game Support Only** (72 languages): developers can only mark them as supported by their game, in the "Edit Store" section of Steamworks. The mark shows on the store page and players can search by it (since October 2022). The Steamworks APIs don't support these languages.

## Full Platform Support languages

**API language code** is used with the client-side Steamworks API (the SDK and Steamworks.NET). **Web API language code** is used with the Steamworks Web API.

| English name | Native name | API language code | Web API language code |
|---|---|---|---|
| Arabic\* | العربية | `arabic` | `ar` |
| Bulgarian | български език | `bulgarian` | `bg` |
| Chinese (Simplified) | 简体中文 | `schinese` | `zh-CN` |
| Chinese (Traditional) | 繁體中文 | `tchinese` | `zh-TW` |
| Czech | čeština | `czech` | `cs` |
| Danish | Dansk | `danish` | `da` |
| Dutch | Nederlands | `dutch` | `nl` |
| English | English | `english` | `en` |
| Finnish | Suomi | `finnish` | `fi` |
| French | Français | `french` | `fr` |
| German | Deutsch | `german` | `de` |
| Greek | Ελληνικά | `greek` | `el` |
| Hungarian | Magyar | `hungarian` | `hu` |
| Indonesian | Bahasa Indonesia | `indonesian` | `id` |
| Italian | Italiano | `italian` | `it` |
| Japanese | 日本語 | `japanese` | `ja` |
| Korean | 한국어 | `koreana` | `ko` |
| Malay | Bahasa Melayu | `malay` | `ms` |
| Norwegian | Norsk | `norwegian` | `no` |
| Polish | Polski | `polish` | `pl` |
| Portuguese | Português | `portuguese` | `pt` |
| Portuguese-Brazil | Português-Brasil | `brazilian` | `pt-BR` |
| Romanian | Română | `romanian` | `ro` |
| Russian | Русский | `russian` | `ru` |
| Spanish-Spain | Español-España | `spanish` | `es` |
| Spanish-Latin America | Español-Latinoamérica | `latam` | `es-419` |
| Swedish | Svenska | `swedish` | `sv` |
| Thai | ไทย | `thai` | `th` |
| Turkish | Türkçe | `turkish` | `tr` |
| Ukrainian | Українська | `ukrainian` | `uk` |
| Vietnamese | Tiếng Việt | `vietnamese` | `vi` |

\* **Arabic** works differently from the rest of this list. It can be used for game depots and chosen as the game language in the per-game settings panel. However, the Steam UI isn't translated into Arabic, and players can't choose Arabic as the language to run Steam in.

**Codes that don't follow the obvious pattern:** `koreana` (not `korean`), `schinese` / `tchinese`, `brazilian` (Portuguese-Brazil), `latam` (Spanish-Latin America), and `spanish`, which means Spain only.

## Game Support Only languages

These can only be selected as languages the game supports in its store settings. They have no API or Web API codes.

Afrikaans, Albanian, Amharic, Armenian, Assamese, Azerbaijani, Bangla, Basque, Belarusian, Bosnian, Catalan, Cherokee, Croatian, Dari, Estonian, Filipino, Galician, Georgian, Gujarati, Punjabi (Gurmukhi), Hausa, Hebrew, Hindi, Icelandic, Igbo, Irish, Kannada, Kazakh, Khmer, K'iche', Kinyarwanda, Konkani, Kyrgyz, Latvian, Lithuanian, Luxembourgish, Macedonian, Malayalam, Maltese, Maori, Marathi, Mongolian, Nepali, Odia, Persian, Quechua, Scots, Serbian, Punjabi (Shahmukhi), Sindhi, Sinhala, Slovak, Slovenian, Sorani, Sotho, Swahili, Tajik, Tamil, Tatar, Telugu, Tigrinya, Tswana, Turkmen, Urdu, Uyghur, Uzbek, Valencian, Welsh, Wolof, Xhosa, Yoruba, Zulu.

## FAQ

**Q: What if the game supports none of the Full Platform Support languages?**
A: Store descriptions, art capsules and other store content must still be entered under "English" in the store editor's language drop-downs. English is Steam's fallback language and needs content, even if that content is actually in another language.

## Notes (not from the source page)

- In Steamworks.NET, `SteamApps.GetCurrentGameLanguage()` returns the language the player chose for this game, and `SteamUtils.GetSteamUILanguage()` returns the Steam client's UI language. Both return the **API language code** strings above. Rock Flipper's code doesn't call either one yet (checked 2026-10-18).
