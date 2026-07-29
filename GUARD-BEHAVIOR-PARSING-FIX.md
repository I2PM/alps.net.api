# Guard behavior parsing fix (2026-07-05)

## Context

Models with a guard (interruption) behavior — tested with an Orderer/Supplier model where the
Supplier has a "Cancel Order" guard (`Order-Supplier.owl`, kept as a test resource in
`UnitTestProject/src/`) — were parsed incorrectly:

1. **Every state and transition of the guarded subject's normal SBD was re-parented into the
   guard behavior.** `state.getContainedBy(out behavior)` returned the `GuardBehavior`
   (`GBD_8_SID_1_GuardExtension_3`) for all of the Supplier's *normal* states, making them
   indistinguishable from real guard states. Downstream consumers that classify states by their
   containing behavior (normal layer vs. guard layer) tagged the whole Supplier SBD as "guard".
2. **`SubjectBehavior.getInitialStateOfBehavior()` was always `null` after an import**, so the
   guard's entry state (the lightning-bolt receive state) could not be resolved as an object.
3. **`ModelLayer.getLayerType()` was unreliable**: the layer `SID_7`, explicitly typed
   `standard-pass-ont:GuardLayer` in the OWL, reported `STANDARD` (or `EXTENSION`, depending on
   triple order), and its `SubjectExtension` individual was silently removed from the layer.

## Root causes

### 1. The guard behavior claimed ownership of guarded elements

`BehaviorDescribingComponent.setContainedBy()` is last-writer-wins. Two paths let a
`GuardBehavior` call it on elements it merely *guards*:

- **Observer replay** (the main path, triggered by the standard `guardsBehavior` triple):
  `GuardBehavior.parseAttribute` → `addGuardedBehavior(guardedSBD)` →
  `guardedSBD.register(this)` → `PASSProcessModelElement.register` immediately replays *all*
  connected elements of the guarded SBD (all its states/transitions) into
  `guardBehavior.updateAdded(...)`. The inherited `SubjectBehavior.updateAdded` treats every
  incoming `IBehaviorDescribingComponent` as its own component and calls
  `component.setContainedBy(this)` — re-parenting the entire guarded SBD into the guard.
- **Parse hook** (same effect for `guardsState` triples): after `parseAttribute` accepts a
  triple, `successfullyParsedElement(object)` is called, and `SubjectBehavior`'s override stamps
  `setContainedBy(this)` on the referenced element.

**Fix** (`StandardPASS/PassProcessModelElements/SubjectBehaviors/GuardBehavior.cs`):
`GuardBehavior` now overrides `updateAdded` and `successfullyParsedElement` and skips elements
that are in its `guardedBehaviors`/`guardedStates` dictionaries (helper `isGuardedElement`).
Guarding still tracks those elements (removal/rename notifications keep working via the observer
registration); they just are not *contained* anymore. The guard's own components — parsed from
its `contains`/`hasInitialState` triples — are unaffected.

### 2. Initial state never set during import

`SubjectBehavior.parseAttribute` handled `hasInitialState`, but passed its own (still null)
*field* instead of the parsed element:

```csharp
setInitialState(initialStateOfBehavior);   // field, always null at that point
```

**Fix** (`StandardPASS/PassProcessModelElements/SubjectBehavior.cs`): pass the parsed
`initialState`. As a side effect `setInitialState` also stamps
`IState.StateType.InitialStateOfBehavior` on the state, making the flag robust even when the
exporter does not rdf-type the state as `InitialStateOfBehavior`.

### 3. Layer type inference overrode the explicit model statement

- `ModelLayer.parseAttribute` correctly parses `rdf:type GuardLayer` → `LayerType.GUARD`, but
  `checkLayerTypes()` (re-run on every `addElement`) re-infers the type from the *C# types* of
  contained elements. Exporters type a guard's extension individual as generic
  `abstract-pass-ont:SubjectExtension`, so inference yielded `EXTENSION` — or `STANDARD` when
  the extension was missing (see next point) — overwriting the explicit `GUARD`.
- `ModelLayer.addElement` ejected any extension that is not an `IGuardExtension` C# instance
  from a GUARD-typed layer. Since the exported individual is a generic `SubjectExtension`, the
  guard's own extension was removed from its layer.
- The duplicate-extension check in `addElement` compared the just-added extension **against
  itself** (`ext.getExtendedSubject().Equals(subjExt.getExtendedSubject())` over a collection
  that already contains `subjExt`), so any extension whose `extends` reference was already
  resolved removed itself from the layer.

**Fixes** (`ALPS/ALPSModelElements/ModelLayer.cs`):

- New flag `layerTypeExplicitlySet`, set when a specific layer type (`GuardLayer`, `MacroLayer`,
  `BaseLayer`) is parsed from `rdf:type`. `checkLayerTypes()` skips inference when set. The
  generic `ExtensionLayer` type does not pin the type (it is the parent of Guard-/MacroLayer and
  both often appear on the same individual).
- GUARD layers now accept generic `SubjectExtension` individuals (only `IMacroExtension` is
  rejected there).
- The duplicate-extension check skips the element itself.

### 4. (Robustness) Model import crashed when no candidate class matched

`BasicPASSProcessModelElementFactory.decideForElement` returned a default (null) pair when all
candidate classes scored `canParse == -1`, and the caller dereferenced it →
`NullReferenceException` aborting the whole import. Observed when additional assemblies are
registered via `ReflectiveEnumerator.addAssemblyToCheckForTypes` (the unit-test setup does this).

**Fix** (`parsing/BasicPASSProcessModelElementFactory.cs`): start the max-search below the
"cannot parse" score so a deterministic candidate is returned instead of crashing.

## Verification

New test class `UnitTestProject/GuardBehaviorParsingTest.cs` loads `Order-Supplier.owl` (with
`standard_PASS_ont_v_1.1.0.owl` + `ALPS_ont_v_0.8.0.owl`) and asserts:

- the guard behavior is parsed as `IGuardBehavior`, normal SBDs are not;
- **no state of the guarded (Supplier) SBD resolves to a guard behavior** (before the fix, all 8
  normal Supplier states and 22 further components were claimed by the guard);
- the guard behavior contains exactly its own 3 states, all resolving back to it;
- model-wide, exactly the guard's 3 states are owned by a guard behavior (Orderer and Customer
  Satisfaction Department states are all normal);
- the guard's entry state (`SBD_8_GuardReceiveState_1`, "Receive Cancellation") is resolvable via
  `getInitialStateOfBehavior()`, is an `IReceiveState` and flagged `InitialStateOfBehavior`;
- the Suppliers subject exposes both its base SBD and the guard behavior via `getBehaviors()`;
- `SID_7.getLayerType() == GUARD`, `SID_1 == STANDARD`; the guard layer keeps its
  `SubjectExtension` (extending the Suppliers subject) and the Customer Satisfaction Department;
- subjects resolve to their layers via `getContainedBy` (Suppliers → `SID_1`,
  Customer Satisfaction Department → `SID_7`).

Result: all 9 tests red before the fixes (matching the reported symptoms exactly), all green
after. The rest of the suite is unchanged: 19 passed / 10 failed both on the unpatched baseline
and after the fixes (the 10 failures are pre-existing `UriFormatException` /
extends-import issues unrelated to guards).

Note for running the tests: no .NET 7 runtime needs to be installed;
`DOTNET_ROLL_FORWARD=Major dotnet test` works. The test project had three pre-existing compile
errors that were fixed alongside (`ReflectiveEnumerator` made public again, `getTriples()` →
`getIncompleteTriples()`/`getPredicate()`, dotNetRDF pinned back to 2.7.5 to match the library —
3.0 lacks `VDS.RDF.Ontology.OntologyGraph`). `Env.cs` referenced a non-existent
`abstract-layered-pass-ont.owl`; it now loads `ALPS_ont_v_0.8.0.owl`.

## Consumer notes (e.g. workflow engines building a domain model on top)

- **State classification**: walk `state.getContainedBy(out ISubjectBehavior b)`; `b is
  IGuardBehavior` ⇒ guard-layer state with `LayerId = b.getModelComponentID()`; `b is
  IMacroBehavior` ⇒ macro; otherwise normal. This is now reliable. Do not string-match
  "GuardExtension" in ids.
- **Subject classification**: `subject.getContainedBy(out IModelLayer layer)` +
  `layer.getLayerType()` now reliably reports `GUARD` for guard layers. Subjects that only exist
  on the guard layer (e.g. a Customer Satisfaction Department that is only reachable from guard
  states) can be tagged with that layer's id/kind.
- **Breaking change to watch**: a layer explicitly typed `GuardLayer` now reports
  `LayerType.GUARD` instead of `EXTENSION`/`STANDARD`. Code that collected extension layers via
  `getLayerType() == EXTENSION` must also accept `GUARD` (and `MACRO`).
- **Initial states**: with guard states now visible alongside normal states of the same subject,
  a subject can expose several `InitialStateOfBehavior` states (one per behavior). Select the
  subject's start state from the *base* behavior (or from states classified as normal); each
  guard's entry state is `guardBehavior.getInitialStateOfBehavior()`.

## Known remaining issues (out of scope, not touched)

- 10 pre-existing unit test failures (`UriFormatException: The URI is empty` in export-related
  tests, extends-import assertions).
- `ModelLayer.checkLayerTypes()` counts `IALPSModelElement`s by iterating the dictionary's
  key-value pairs (`elements.OfType<IALPSModelElement>()`), which never matches — the
  abstract-layer detection there is a no-op.
- `ReflectiveEnumerator`'s assembly registration is global state; registering extra assemblies
  changes parsing results for every subsequent import in the same process.
