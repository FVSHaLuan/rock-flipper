# Scripts/F2P — ads & IAP

18 files. Explicitly reuse-oriented: interface + editor-stub + platform-implementation split, meant to carry to future games mostly unchanged except for the files under `AppSpecific/`.

## How the ad system works end-to-end

1. Call site uses a static facade: `Agame.F2P.InterAds.Show(callback)` (interstitial) or `RewardedAds.Show(callback)`, or drop `InterAdsHelper`/`RewardedAdsHelper` on a button (`Helpers/`).
2. Each facade's **static constructor** picks a concrete implementation by preprocessor branch:
   - Not `VersionBranchInfo.IsF2P` → implementation stays `null` (facade methods short-circuit first, safe).
   - `UNITY_EDITOR` → `EditorInterAds`/`EditorRewardedAds` (`Editor Implementations/`) — shows a blocking `EditorUtility.DisplayDialog` instead of a real ad, so reward flows are testable without a network SDK. These **throw if ever reached outside the editor** — a deliberate safety net against shipping the stub.
   - `UNITY_ANDROID || UNITY_IOS` → `AppodealInterAds.Instance`/`AppodealRewardedAds.Instance` — **these classes do not exist in the codebase.** This is the extension point: implement `IInterAdsImplementation`/`IRewardedAdsImplementation` with a static `Instance` to plug in a real mobile ad SDK when a game actually ships F2P on mobile.
   - Else (desktop/console) → `NoInterAds`/`NoRewardedAds` — always report unavailable.
3. `InterAds.Show` is additionally hard-gated by `#if !BSB_F2P` (compiled out entirely if that define is unset). `RewardedAds.Show` checks `VersionBranchInfo.IsF2P` at runtime instead.
4. Premium bypass: `RewardedAds.Show` auto-succeeds (`callback(true, true)`) without showing anything if `PlayerData.UnlockedPremium`; `InterAds.Enabled` is `false` outright when premium is unlocked. `InterAds` also enforces a `MinAdsInterval` (300s) between shows.

## How IAP works end-to-end

1. `UnityPurchaser.cs` (namespace `FMod`, `DontDestroyOnLoad` singleton) owns the real Unity IAP integration (`IDetailedStoreListener`) — configure `consumableProductsIds`/`nonconsumableProductsIds` in the inspector; self-initializes on iOS/Android only.
2. `IAPHub.cs` is the facade to call: `IAPHub.BuyProduct(id)`, `RestoreIAP()`, subscribe `OnPurchasedProduct`. No-ops on non-mobile platforms (`#if UNITY_IOS || UNITY_ANDROID`).
3. **`AppSpecific/` is the per-game glue you're expected to edit**: `IAPHelper.cs` (`BuyPremium()` → `IAPHub.BuyProduct(GameConst.PremiumProductId)`) and `PurchasedProductsHandler.cs` (`HandlePurchasedPremium()` — **currently `throw new NotImplementedException()`, a stub every new game must fill in**, e.g. set `PlayerData.UnlockedPremium = true` and save). A new project reusing this layer should either fill in this stub or replace these two files, keeping `IAPHub`/`UnityPurchaser` untouched.
4. `UsePremiumFeaturePopup.cs` — the standard "convert a locked feature into ads-or-purchase" paywall UI (`[RequireComponent(UIScreen)]`). `Show(callback)` presents Watch-Ads/Buy-Premium buttons; result is `Result.Canceled/InitiatedPremiumPurchase/WatchedAds`.

## Other

- `PremiumCheck.cs` — a `ValueDisplayer<bool>` watching `PlayerData.UnlockedPremium`, fires `onPremiumUnlocked`/`onPremiumLocked` UnityEvents — drop on any GameObject to declaratively gate premium-only UI.
- `Rating/MobileRatingLauncher.cs` — native store-review prompt (iOS `RequestStoreReview`, Android Google Play in-app review).
- `Rating/MobileRatingRequestPopupShower.cs` — threshold-based nudge, `PlayerPrefs`-indexed into an inspector `thresholds` list; `SetAsRated()` permanently suppresses. Skipped if `!VersionBranchInfo.IsF2P`.

## Conventions / gotchas

- **Reuse boundary is `AppSpecific/`** (plus `PremiumCheck` at the root, and the not-yet-written Appodeal implementations) — everything else (`InterAds`, `RewardedAds`, `IAPHub`, `UnityPurchaser`, interfaces, editor/no-op implementations) should carry over to new games unmodified.
- Master switch: `BSB_F2P` scripting define (toggle via `FH/Version Branching/Enable|Disable F2P` menu, see `Scripts/FeatureBranching/CLAUDE.md`) — mirrored at runtime by `VersionBranchInfo.IsF2P`.
- **Ad network not currently wired for mobile** — expect a compile error if you set `UNITY_ANDROID`/`UNITY_IOS` + `BSB_F2P` without first adding `AppodealInterAds`/`AppodealRewardedAds`.
- Premium bypasses ads but not IAP restore — both funnel through `IAPHub`; make sure `PurchasedProductsHandler.HandlePurchasedPremium()` and `IAPHub.RestoreIAP()`'s restore-purchase path both set `PlayerData.UnlockedPremium`, or premium users on fresh installs won't get ad-free treatment.
- Namespace inconsistency: `Agame.F2P` throughout except `UnityPurchaser` (`FMod`, matches a similar oddity in `Scripts/Dev`) — legacy, not semantic.
