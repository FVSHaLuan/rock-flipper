# Scripts/Helpers — reusable utility library

242 files, by far the largest reusable folder. **Check here before writing a new utility — there is very likely already one.** Split into two asmdef-backed halves:

- `PureSripting/` (`Helper.PureScripting.asmdef`, ~51 files) — engine-agnostic pure C#, no Unity dependency.
- `UnityExtend/` (`Helpers.UnityExtend.asmdef` + `.Editor` asmdef, ~191 files) — Unity-specific components/extensions.

File names throughout are highly self-descriptive — for anything not covered below, `Glob`/`Grep` for the concept name inside the relevant half is likely to find it directly.

## PureSripting/ (engine-agnostic)

| Category | What | Key files |
|---|---|---|
| BigNumber | Human-readable big-number formatting (suffix letters, scientific toggle) + order-preserving int encoding | `BigNumber/BigNumberString.cs`, `BigNumberStringUnited.cs`, `IntegerCoding.cs` |
| FastRandom | Xorshift PRNG (SharpNeatLib), faster than `System.Random` | `FastRandom/FastRandom.cs`, `FastRandomExtensions.cs` |
| Weighted-random | `IWeighted`/`IWeightedFlexible` interfaces, `Weighted<T>`/`WeightedInt`/`WeightedFlexible`, `PickOne<T>()` extension over a weighted list | `Weights/*.cs` |
| Kinematics/ballistics | Public-domain projectile arc solver (Forrest Smith) + time-to-reach-target math | `Kinematics/Ballistics.cs`, `KinematicMath.cs` |
| String/number formatting | String manipulation, verbal numbers/Roman numerals, ordinal suffix, `[bracket]`-templated substitution, rich-text-aware scanning, seconds→HH:MM:SS, byte→KB/MB/GB, random string, `double` Lerp/MoveTowards, `System.Random.Range` extensions | `StringHelper.cs`, `NumberToStringExtensions.cs`, `NumberOrdinalSuffix.cs`, `CustomParameterizedStrings.cs`, `FormattedStringHelper.cs`, `TimeStringHelper.cs`, `DataCapacityHelper.cs`, `RandomString.cs`, `DoubleHelper.cs`, `SystemRandomExtension.cs` |
| Direction enums | 4-way/8-way direction + vector conversion | `Direction4.cs`, `Direction8.cs` |
| Collections/pooling | Pooled `List<T>`, fixed-capacity Inspector-serializable "array" structs, `SerializableHashSet<T>` | `List Pool/*.cs`, `StructList/StructList5.cs`, `StructList10.cs`, `SerializableHashSet.cs` |
| Stateful values | `StackedValue` (sum of independently add/removable float stacks — buffs), `TemporaryValue` (decay/restore-over-time wrapper — shields/temp HP), `FakeProgress` (simulated loading progress), `FrequentExecution` (throttle callback to N/sec) | `StackedValue.cs`, `TemporaryValue/TemporaryValue.cs`, `FakeProgress.cs`, `FrequentExecution.cs` |
| Graph generators | Abstract `GraphGenerator` + fake/test data generators for prototyping charts | `GraphGenerators/*.cs` |
| Number conditions | Generic serializable "equal/less/more than X" comparator | `NumberConditions/NumberCondition.cs` |
| Misc | `IUnique<T>`, `IProgress`/`IRandomGenerator`, `SerializableTimeSpan`, reflection-based `DefaultValuesVerifier` | root files, `Dev/DefaultValuesVerifier.cs` |

## UnityExtend/ (Unity-specific)

| Category | What | Key files/folders |
|---|---|---|
| Extensions | Math extras, Vector2/3 equality wrapper, reflection component copier, List/Dictionary helpers, Rect/RectTransform/ScrollRect/Image/Color/LayerMask/Direction extensions | `Extensions/*.cs` |
| Random (component-based) | Unity-`Random`-backed float/int/double components, `StatisticalWeight` (`IWeighted` on children for weighted-pick), random rotation/sprite-color/action | `Random/*.cs` |
| Weighted GameObject activation | Pick/activate children by weight | `GameObject/Activation/RandomObjectActivation.cs`, `RandomWeightedObjectActivation.cs` |
| Attributes | `[LargeNumberField]`, `[NoOverride]` (blocks prefab-instance override), `[UnityLayerAttribute]` — with custom PropertyDrawers | `Attributes/*/` |
| Physics (2D) | Speedometer, POV cone collider builder, typed cast wrappers, rigidbody freezer/limiter | `Physics/*.cs` |
| Animation | Scale tweens (bounce/breathe/tween-to), `Expandable` family (UI grows to fit content), `DelayValueEffect` (lagging/eased follower — delayed health bar), `ConstantWorldTransform/` (keep child's world rotation/scale/position fixed regardless of parent), `ProgrammaticAnimation/` (eye-look, floating shadow) | `Animation/*/` |
| Color / renderer abstraction | `ColorPalette`, `ColorTintBouncer`, `UnifiedColoredObject` (abstract base + per-renderer-type impls: Sprite/Text/UI/Particle/Camera/CanvasGroup — color any of them uniformly), `UnifiedSpriteImage` (target `SpriteRenderer` or UI `Image` interchangeably) | `Color/*`, `Rendering/UnifiedSpriteImage/` |
| Rendering | `PropertyBlockData` (declarative `MaterialPropertyBlock` setters), `YSortingManager` (Y-axis 2D depth sort), `CircuitLine` (segment-based right-angle LineRenderer) | `Rendering/*/` |
| Particle system | Attractors, size-by-Z, look-at-direction, bulk property setters, emission-rate-by-shape-area, periodic burster | `ParticleSystem/*/` |
| UI value display | `ValueDisplayer`/`ValueDisplayerUnified` (abstract base for anything showing a numeric value), `CountingText` (animated count-up), `ProgressBar/` family (`ImageFillProgressBar`, `PercentageTextProgressBar`, `TransformProgressBar`, `UISizeProgressBar`), `ProgressViews/` (drive transform position/scale from 0-1 progress), `MultiGraphicTargetsSelectable/` (Button/Toggle driving multiple target Graphics) | `UI/ValueDisplay/*`, `UI/MultiGraphicTargetsSelectable/` |
| Input | `ClickHold` (unified down/hold/drag with click-speed ramp-up, `IHoverMaxBuyService` hook for "hold to buy max" UI) | `Input/ClickHold/` |
| Media | `AudioRandomSeeker`, `FullscreenVideoPlayer` | `Media/Audio/`, `Media/Video/` |
| Camera | Pipe camera output to a UI RawImage or mesh Renderer | `Camera/*.cs` |
| Screen | Aspect scaler/limiter, size-change detection, narrow-screen detection, fullscreen toggle | `Screen/*.cs` |
| Setters | One-shot "apply this value" components (alpha, layer, rotation, velocity, transform position/rotation/scale — local and random variants) | `Setters/*.cs`, `Setters/Transform/*.cs` |
| Core infra | `LinkedValue<T>` (editor/runtime dual-path cached value, ScriptableObject-asset variants available), `IDAsset` (ScriptableObject with auto-GUID stable id), `GeneralList` (runtime-instantiated UI list from a template item), `InfiniteFloatList` (auto-extrapolating serialized list — level-scaling curves) | `Core/*/` |
| DevTools | Gizmo helpers, fake/randomized data displayers for prototyping without real data, linear/circle object arrangement | `DevTools/*/` |

## Conventions / gotchas

- A few `[Obsolete]`/`[Obsolete("unfinished")]` classes exist in-tree (e.g. `Vector2DInt`, `GameObjectMesh`, `DelayedActivation`) — check for the attribute before building on an unfamiliar class here.
- `PureSripting/` has no Unity dependency by design — if you need a utility usable from both a pure-C# test project and Unity, it belongs there, not in `UnityExtend/`.
