# Scripts/UI Systems — screen navigation, prompts, tooltips, menus

63 files, the largest/most architecturally significant UI folder. Read this before building any new screen or menu. Integrates tightly with `Scripts/Input` (device detection) — see `Scripts/CLAUDE.md`'s entry for that folder.

Almost every non-static class extends `ExtendedMonoBehaviour` (see `Scripts/Common/CLAUDE.md`) and reaches managers via `entry.xxx` — `Entry` exposes `uiScreenManager`, `uiSelectedEventManager`, `buttonPromptManager`, `activeContextButtonPromptManager`, `inputManager`, `completeInputBlocker`, `mouseCursorVisibilityManager`, etc. **Service-locator style — read dependencies off `entry`, don't `FindObjectOfType`.**

## UIScreen / UIScreenManager (`UIScreen/`)

**Single global screen stack** (not per-canvas) — `UIScreenManager` holds one `List<UIScreen> screenStack` for the whole game, registered on `Entry`.

- `UIScreen.cs` — `[RequireComponent(CanvasGroup)]` base for any screen/popup. **You don't call open/close on the manager directly**: enabling the GameObject (`OnEnable`) or `ShowPopup()` → `PutToStack()`; disabling (`OnDisable`) or `PopFromStack()` → pops. Manager deactivates the previous top before activating the new one, reactivates the new top automatically when the active one pops.
- Re-entrant Put/Pop calls (from inside `HandleBecomeActive`/`HandleBecomeInactive`) are **queued**, not executed synchronously — don't assume immediate execution when chaining transitions from those callbacks.
- Events: `OnBecomeActive`, `OnBecomeInactive`, `OnInteractabilityChanged` (C#) + UnityEvent equivalents for inspector wiring.
- `Interactable` (bool) gates a screen (via `CanvasGroup.interactable`) without removing it from the stack.
- `HidePreviousPopupContent` (inspector flag) — whether the screen underneath keeps its content active when this one becomes active on top.
- First-selectable handling built in: on activation, tries a "predefined" flagged selectable, then last-focused-in-screen, then first active in `firstSelectables`, via `entry.uiSelectedEventManager.SetCurrentSelectedGameObject(...)`.
- **Gotcha**: `alwaysPopFromStackWhenDisable` defaults `true` — simply `SetActive(false)`-ing a screen always pops it from the stack unless explicitly set `false`. Surprising if you toggle screens for pooling rather than navigation.
- `UIScreenChildInteraction.cs` — base for child components needing `protected bool Interactable` (true only if parent screen active+interactable).

## ButtonPrompt / ContextButtonPrompt (`ButtonPrompt/`) — gamepad/KBM glyph system

Namespace `Agame.UI.ButtonPrompts`. Two layers: low-level glyph resolver + higher-level contextual prompt list.

- **Trigger**: `Scripts/Input/InputManager.cs` fires `OnActiveInputDeviceChanged` when the active device changes (computed via `SimplifiedDeviceRecognition.cs`); every `ButtonPromptView` re-resolves its sprite on that event.
- `ButtonPromptManager.cs` — `ScriptableObject` (`entry.buttonPromptManager`), `List<InputActionButtonPromptSprites> promptMap` mapping `InputActionReference` → `ButtonPromptSprites` (mouseAndKeyboard/xbox/ps/switch/steam/otherGamepad sprites + `isHold`/`activeContextOrderId`). **To configure a prompt per device**: assign an enum value in the promptMap entry, run `Editor_Sync` context menu — it auto-derives sprites from `MouseKeyboardButtonGlyphTable`/`GamepadButtonGlyphTable` (one table per platform; unset platform buttons fall back to Xbox).
- `ButtonPromptView` (abstract) / `ButtonPromptViewImage` (concrete) — set `InputAction`, it displays the right sprite for the current device.
- `ContextButtonPrompt` (abstract: `InputActionReference`, `Text`, `OrderId`) — represents "an action available right now" ("Press A to confirm"). Subtypes: `SelectableContextPrompt` (active only while sibling `Selectable` is focused), `SimpleContextPrompt` (always active while enabled), `UIInputActionContextPrompt` (mirrors a sibling `UIInputActionBase`'s interactability — **the typical "hook a prompt to a button" pattern**).
- `ActiveContextButtonPromptManager` (`entry.activeContextButtonPromptManager`) — single sorted list across the game, `Add/RemovePrompt`, batches `OnActivePromptListChanged` once per frame.
- `ActiveContextButtonPromptsView` — per-`UIScreen` filtered/pooled render of the prompt bar; `ContextButtonPromptView` renders one row (icon, text, optional hold-progress bar bound to `UIHoldAction.OnUpdatedHoldProgress`).
- **To add a context prompt to a screen**: put `UIInputActionContextPrompt` next to your button, set `Text`, ensure the screen has an `ActiveContextButtonPromptsView`.
- **Gotcha**: `Editor_Sync` tools are load-bearing — after adding a new `InputAction`, run them or `GetPromptSprites` throws `KeyNotFoundException` at runtime with no editor-vs-build fallback.

## ToolTip system (`ToolTip/`)

Namespace `Agame.UI.ToolTips`. Request/handle-based, not simple show/hide.

- `ToolTipTriggerer` — attach to any object; implements pointer-enter/exit + select/deselect handlers, so it shows on hover **or** UI-navigation focus automatically. Builds a `ToolTipRequest`, calls `CommonEntry.CommonInstance.toolTipManager.Show(request)` (**note: lives on `CommonEntry`, not `Entry`**), holds the returned `requestId` to `Hide()`/`Update()` later.
- `ToolTipManager` — pulls a pooled instance, positions relative to whichever screen-corner anchor keeps it on-screen, auto-hides on mouse-down/up unless `doNotHideOnMouseDown`.
- `ToolTip` — the pooled visual prefab (`GeneralPoolMemberSimplified`); `SetContent()` sets `mainText` — extend for richer content.
- `TooltipContentSetter` (abstract) — override `UpdateContent()` to compute text lazily right before showing (`OnBeforeShow`) — use when the text is expensive or state-dependent.
- **To attach a tooltip**: add `ToolTipTriggerer`, fill `mainText` (or a `TooltipContentSetter` subclass), set positioning — nothing else required.

## Menus, list selection, pointer wrappers, UI Inputs, MiniConsole

- **`Horizontal Menu/`** — tab-strip. `HorizontalMenu` owns `List<HorizontalMenuItem>` (auto-discovered), `MoveNext()`/`MoveBack()` (wraps, skips locked via overridable `HorizontalMenuItem.IsUnlocked`). `List/` subfolder mirrors items 1:1 as a header/tab-button strip.
- **`Vertical Menu/`** — sidebar driven by current UI-navigation focus (not explicit index). `VerticalMenu` watches `uiSelectedEventManager.OnSelectionChanged`; whichever `VerticalMenuItem` is focused becomes active, disabling other items' `interactable` so nav can't tab past the section.
- **`ListItemSelection/`** — spatial "which row is framed in viewport" tracker, independent of UI focus. `ListItemSelectionManager` recomputes visibility of registered `ListItemSelectableRect`s each `LateUpdate`, exposes `SelectedItem`/`OnSelectedItemChanged`. Use for auto-scrolling-feed-style highlight, not keyboard-focus-driven UX.
- **`PointerInput/`** (namespace `Agame.PointerInput`) — thin `IPointer*Handler`→UnityEvent adapters: `PointerEvents`, `PointerEnterAndExitEvents`, `PointerDragEvents`, `PointerStandaloneEvents` (raw `Mouse.current` polling, bypasses event system — non-UI-targeted global mouse events only).
- **`UI Inputs/`** — richer-than-`Button` input-bound controls. `UIInputActionBase` (extends `Selectable` directly) binds an `InputActionReference`, fires `OnActionStarted/Performed/Canceled/Disrupted`; auto-respects parent `UIScreen` interactability. **Static global kill-switch**: `UIInputActionBase.AddDisabledLock(obj)`/`RemoveDisabledLock(obj)` disables **every** `UIInputActionBase` in the game (reference-counted `BalancerWithObjects`). `UIInputAction` = standard button. `UIHoldAction` = adds hold-duration progress (what `ContextButtonPromptView` looks for). `SequentialUIInputActions` chains a list, one active at a time (classic tutorial "press keys in sequence").
- **`MiniConsole/`** (namespace `BSB.UISystems`) — scrolling log/dialogue feed. `MiniConsole.PushItem(string)` or `PushItem(MiniConsoleItemData)` enqueues; rate-limited, pooled, capped. `MiniConsolePlayer` reads a `TextAsset` script line-by-line, `_`-prefixed lines are commands (`_WAIT`, `_WAITFORANYKEY`, `_IMPORT`, ...) via `CommandTerminal`'s parser.
- **`UI Selections/`** — `UISelectedEventManager` (`entry.uiSelectedEventManager`) is the focus-tracking hub: wraps `EventSystem.current.currentSelectedGameObject`, fires `OnSelectionChanged`. Nearly every widget here subscribes to this instead of polling the EventSystem.
- **`CompleteInputBlocker/`** — `CompleteInputBlocker.cs`, a **second, separate** global "block everything" switch: reference-counted (`AddBlockLock`/`RemoveBlockLock`), shows a blocking canvas AND disables the whole `InputActionAsset` AND internally calls `UIInputActionBase.AddDisabledLock` too.
- **`ScrollRect Scroll by Selection/`** — auto-scrolls a `ScrollRect`/`EnhancedScroller` to keep the focused item visible, driven by `uiSelectedEventManager`.

## Conventions & gotchas

- **Two independent global "everything off" switches, easy to confuse**: `UIInputActionBase.AddDisabledLock/RemoveDisabledLock` (just input-action buttons) vs. `CompleteInputBlocker.AddBlockLock/RemoveBlockLock` (also disables the whole InputActionAsset + shows a blocking canvas; internally calls the former too). Both reference-counted — always pair Add/Remove with the same key object.
- `UIInputActionBase` is a `Selectable` subclass, not a `Button` wrapper — don't put both a `UIInputActionBase` and a plain `Button`/`Selectable` on the same GameObject. Use `SelectableExtentions.SetInteractable()` when toggling interactability generically (it special-cases `UIInputActionBase`'s custom setter, which fires `OnActionDisrupted` for in-progress holds).
- **Selection-driven, not click-driven**: `VerticalMenu`, `ScrollRectScrollBySelection`, `ListItemSelection`, context prompts all key off `UISelectedEventManager` rather than pointer events (the whole UI must work identically for gamepad/keyboard nav and mouse). Wire new interactive widgets through `entry.uiSelectedEventManager.SetCurrentSelectedGameObject(...)`, not `EventSystem.current.currentSelectedGameObject` directly, or tracking desyncs.
- Namespace inconsistency: most of this folder has no namespace, but `ButtonPrompt`/`ToolTip` use `Agame.UI.*`, `PointerInput` uses `Agame.PointerInput`, `MiniConsole` uses `BSB.UISystems` — check the existing file's namespace before adding a sibling.
