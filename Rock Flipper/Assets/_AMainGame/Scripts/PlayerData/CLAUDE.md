# Scripts/PlayerData — cross-run save data

5 files, small, but safety-critical: this is the persisted-across-runs save model, with a partial-class versioning/migration convention. Read this before adding any persisted field. (For per-run save data, see `Scripts/Run/RunData/`, documented in [Run/CLAUDE.md](../Run/CLAUDE.md).)

## Files

- `PlayerData.cs` — `[Serializable] partial class PlayerData` (namespace `Agame`). Fields: `lastTimeSaved`, `installTime`/`timeSpentInGame`, `lastLaunchVersion`/`currentVersion` (parsed into `Version` objects by `UpdateVersions()`), `randomSeed`, `lastSlotId`, `generalStorage` (a `MiniStorage`, see `Scripts/Common/CLAUDE.md`), `platformUserId`/`pseudoUserId` (generated from `SystemInfo.deviceUniqueIdentifier`).
- `PlayerData_Correction.cs` — partial: `CorrectData(PlayerData defaultData)` — migration/repair, runs once per load.
- `PlayerData_Serialization.cs` — partial: `UpdateBeforeSave()` — pre-save normalization hook.
- `PlayerDataObject.cs` — `WritableScriptableObject<PlayerData>` (`FH.Core.Architecture.WritableData`), the actual asset referenced by `Entry.playerDataObject`/`playerDataObjectDemo`. Owns load/save orchestration.
- `PlayerDataSaver.cs` (root of `Scripts/`, not this subfolder) — `ExtendedMonoBehaviour` on the `Entry` prefab; orchestrates *when* saves happen.

## Load/save/correction pipeline (exact mechanics)

`PlayerDataObject.Data` lazy-loads on first access: `LoadData()` asks `WritableDataManagerProvider` for save bytes (JSON, optional encryption), deserializes; if no save exists, **clones `defaultData`** (a fresh `PlayerData()` with Inspector-configured defaults) via `BinarySerializationHelper.Clone`. Then `PlayerDataObject.OnDataLoaded(PlayerData data)` runs — **every load, new or existing**:
1. `data.CorrectData(defaultData)` — repair/migrate.
2. `data.UpdateVersions()` — shifts `currentVersion` → `lastLaunchVersion`, sets `currentVersion = Application.version`.
3. `data.UpdatePseudoUserId()` — assigns stable pseudo id if missing.

`SaveData()` (override) calls `CurrentData.UpdateBeforeSave()` first, then the base save (`WritableDataManagerProvider...SaveData(Key, CurrentData, fileFormat, UseEncryption, password)`).

`PlayerDataSaver` (`entry.playerDataSaver`): `SaveNow()` (immediate), `SetSaveThisFrame()` (deferred to end of frame — **the normal way to request a save**). Save-lock API: `AddUnsavableLock(object)`/`RemoveUnsavableLock(object)` (via `BalancerWithObjects`) — while any lock is held, `Save()` is skipped entirely. `SaveAndAddUnsavableLock(object)` forces one final save then locks (e.g. entering a screen where mid-edit data shouldn't autosave). Also saves `entry.runDataManager.ActiveRunDataObject` (the current run's data) in the same pass. `OnApplicationQuit()` separately saves `GameSetting` as a safety net.

## How to add a new persisted field safely

1. Add `[SerializeField] private <Type> myField;` to `PlayerData.cs` (existing `#region FIELDS` groups are numbered `[Header("N. ...")]` — follow that).
2. Add a public property in the matching `#region PROPERTIES` section, mirroring the existing get/set style.
3. **If it needs a non-default default value**: initialize it in `PlayerData_Correction.cs`'s `CorrectData(defaultData)`, guarded so it only fires for old saves missing it — pattern used for `randomSeed`: `if (randomSeed == 0) { randomSeed = ...; }`. This is the mechanism that makes old save files safe: an old JSON blob missing the field deserializes it to the type default, and `CorrectData()` (called every load) is the one chance to detect "default = never set" and fix it.
4. **If it needs derived/non-serialized state** (like `Version` objects parsed from serialized strings): do that in `PlayerData_Serialization.cs` or inline, called from `OnDataLoaded` (see `UpdateVersions()`) — **not** in `CorrectData`, which is for repair/migration specifically.
5. **If it needs normalization right before writing to disk** (e.g. flattening a runtime dict into a serializable list): add to `UpdateBeforeSave()`.

`CorrectData` receives `defaultData` (the ScriptableObject asset's Inspector-configured defaults) so migration logic can pull "what should this have been" rather than hardcoding values. It sets `isCorrectingData = true` for its duration and `correctedData = true` when done (other code, like `UpdateBeforeSave`'s `SaveTaskInstancesToList`, checks this flag and no-ops until correction has run at least once).

## Conventions / gotchas

- `PlayerData.UnlockedPremium` currently `throw new NotImplementedException()` — a stub, don't rely on it existing yet (see `Scripts/F2P/CLAUDE.md` for where this matters).
- Two `PlayerDataObject` assets exist (`playerDataObject`/`playerDataObjectDemo`); which is "live" is decided by `Entry.PlayerDataObject` (branch switch) — always go through `entry.PlayerDataObject.Data`/`playerData` (from `ExtendedMonoBehaviour`), never hold a direct reference to one specific asset.
- Never call `PlayerDataObject.SaveData()` directly from gameplay code — go through `entry.playerDataSaver.SetSaveThisFrame()` (or `SaveNow()` if truly synchronous is needed), and respect `AddUnsavableLock`/`RemoveUnsavableLock` mid multi-step mutations.
- Saves are JSON, optionally encrypted per-platform (`encryptOnStandalone`/`encryptOnMobile`/`encryptOnEditor`, off by default).
