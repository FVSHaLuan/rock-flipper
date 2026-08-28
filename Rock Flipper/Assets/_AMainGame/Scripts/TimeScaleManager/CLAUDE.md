# Scripts/TimeScaleManager — pause / slow-motion

8 files. Non-obvious semantics — read before touching pause, slow-motion, or "unscaled" timers.

## Files

- `TimeScaleManager.cs` — core manager (`ExtendedMonoBehaviour`, ~412 lines, on the `Entry` prefab as `entry.timeScaleManager`). Owns `Time.timeScale`/`Time.fixedDeltaTime`, tracks pause vs. slowdown independently via two `BalancerWithObjects`, exposes gameplay-unscaled time, fires events.
- `GamePauser.cs` — convenience: `Pause()`/`Unpause()` call `AddPauseGame(this)`/`RemovePauseGame(this)`.
- `GameplaySlowndownLock.cs` — auto-locks slowdown in `OnEnable`, unlocks in `OnDisable` — attach to a GameObject to make its active-state declaratively control slowdown.
- `ITimeScaleControl.cs` / `TimeScaleControl.cs` / `TimeScaleControlStandalone.cs` — pluggable time-scale multiplier/override controls (Mono and non-Mono variants).
- `TimeScaleControlType.cs` — `enum { Override, Multiply }`.
- `TimeScaleMode.cs` — `enum { ScaledTime, GameplayUnscaledTime, UnscaledTime, GameplayUnscaledTimeAbsolute }`.

## API — requesting a pause or slowdown

- **Pause**: `entry.timeScaleManager.AddPauseGame(object lockOwner)` / `RemovePauseGame(lockOwner)`, or attach a `GamePauser` and call `Pause()`/`Unpause()`. Backed by `unpausedBalancer` (`BalancerWithObjects`, a `HashSet<object>`) — any number of independent holders; only unpauses once **every** holder releases.
- **Slowdown**: `AddGameplaySlowdown(object lockOwner)`/`RemoveGameplaySlowdown(lockOwner)`, or attach `GameplaySlowndownLock` (auto on `OnEnable`/`OnDisable`).
- **Time-scale multiply/override** (a third, independent mechanism — e.g. an ability that slows time to 0.5x without pausing): implement `ITimeScaleControl`, or use `TimeScaleControlStandalone(TimeScaleControlType, float)` for non-Mono callers, or drop a `TimeScaleControl` component (self-registers `OnEnable`/unregisters `OnDisable`). Multiple `Multiply` controls compound; `Override` replaces the value outright.

Lock objects must be reference types (passing a value type throws in editor) and each `Remove` must pass the exact same reference used in `Add`.

## How the final `Time.timeScale` is computed (`UpdateTimeScale()`)

1. If `unpausedBalancer` is not balanced (something is paused) → `timeScale = 0`, full stop, regardless of controls.
2. Else start from `1`, fold in registered `controls` (`Override` sets, `Multiply` multiplies).
3. If gameplay is being slowed down → result clamped to `Mathf.Min(result * 0.2, 0.2)` — **gameplay-slowdown always caps at 20% speed** (hardcoded `GameplaySlowTimeScale`, not Inspector-tunable — change this file to change "how slow").
4. Sets `Time.timeScale`, lerps `Time.fixedDeltaTime` toward a `0.0001f` floor as scale shrinks (restored to max when paused).
5. Fires `OnTimeStopped`/`OnTimeResumed` only when the computed scale crosses to/from exactly `0`.

## Paused vs. slowed-down vs. unscaled — the exact distinction

- **Paused** (`IsGameplayBeingPaused`): full stop. `GameplayUnscaledDeltaTime`/`...Absolute` both return `0` — gameplay animations/timers stop entirely even if "unscaled", because pause means gameplay freezes, full stop. UI that must keep animating during pause should use `TimeScaleMode.UnscaledTime` (raw `Time.unscaledDeltaTime`, always `1`x regardless of pause/slowdown).
- **Slowed down** (`IsGameplayBeingSlowedDown`, only relevant if not paused): caps `timeScale` at `0.2`. `GameplayUnscaledDeltaTime` ("ignore Unity's global timeScale but still respect pause/slowdown") returns `Time.deltaTime` (scaled) while slowed down, `Time.unscaledDeltaTime` (full speed) while not — so gameplay VFX/animations driven by this clock still visibly slow down together with physics, even though it's called "unscaled".
- **`GameplayUnscaledDeltaTimeAbsolute`** — ignores slowdown entirely, only respects pause: full `Time.unscaledDeltaTime` unless paused (then `0`). Use for things that should keep running at full speed during a slowdown but freeze on full pause.
- `TimeScaleMode` enum ties this to generic call sites (`ExtendedMonoBehaviour.GetDeltaTime/GetTimeScale/GetTime(TimeScaleMode)`, and `ExtendedMonoBehaviourWithTime`'s per-component field — see `Scripts/Common/CLAUDE.md`): `ScaledTime` = plain, fully affected by everything; `GameplayUnscaledTime` = the slowdown-respecting clock above; `UnscaledTime` = always real; `GameplayUnscaledTimeAbsolute` = real but frozen on pause.

## Conventions / gotchas

- `GameplaySlowndownLock`/`TimeScaleControl` tie lock lifetime to `OnEnable`/`OnDisable` — the simplest way to add a temporary effect is enable/disable (or instantiate/destroy) a prefab holding one, no manual balancer bookkeeping.
- `TimeScaleControlStandalone` exists specifically so non-MonoBehaviour game-logic classes can register a time-scale override without needing a GameObject.
- Because `TimeScaleManager` is itself a field on `Entry`, it exists as soon as `Entry.Awake()` runs — no extra wait needed beyond the normal `ExtendedMonoBehaviour` reference-resolution.
