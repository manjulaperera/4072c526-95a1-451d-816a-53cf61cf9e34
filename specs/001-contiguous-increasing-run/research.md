# Research: Longest Contiguous Strictly Increasing Run

**Feature**: `001-contiguous-increasing-run`  
**Date**: 2026-08-03

Phase 0 decisions resolving technical choices for the .NET 8 implementation plan.

---

## R1 — Algorithm choice

**Decision**: Single-pass O(n) index scan with `currentStart`, `currentLength`, `bestStart`, `bestLength`.

**Rationale**:

- Constitution Principle VIII mandates O(n) when sufficient.
- Contiguous run detection reduces to comparing each element with its predecessor.
- Earliest maximum-length tie-break requires updating best only when `currentLength > bestLength` (strict inequality).
- AC-010 confirms contiguous semantics; dynamic-programming LIS is incorrect.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| Non-contiguous LIS (DP, O(n log n)) | Contradicts AC-010 expected output |
| Multi-pass (find all runs, then max) | Unnecessary; single pass suffices |
| LINQ-heavy pipeline | Harder to prove O(n) and tie-break; less readable |

---

## R2 — Public namespace and project naming

**Decision**: `CodingTest.Handlers.SubSequenceHandler` in `src/CodingTest`.

**Rationale**:

- Approved plan contract specifies `CodingTest.Handlers`.
- Aligns with repository anonymity (no organisation-specific namespace).
- Functional spec used `String.Handlers` as exercise placeholder; plan namespace supersedes for implementation.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| `String.Handlers` (spec placeholder) | Plan explicitly requires `CodingTest.Handlers` |
| Flat namespace `SubSequenceHandler` | Violates conventional project-root namespace alignment |

---

## R3 — Parsing strategy

**Decision**: Split input on ASCII space (`' '`); parse tokens with `int.TryParse` (InvariantCulture); fail-fast on first invalid token.

**Rationale**:

- All 11 supplied cases use single ASCII spaces (FR-S01).
- FR-C01: null guard before split.
- FR-C02: empty string returns empty sequence (handler returns `""`).
- FR-C03: `FormatException` for non-numeric / empty tokens; `OverflowException` for out-of-range values.
- `InvariantCulture` avoids locale-dependent parsing surprises.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| Regex split on `\s+` | Not evidenced in supplied inputs; would change behaviour for repeated spaces unless explicitly tested |
| `ReadOnlySpan<char>` manual scan | Valid optimization later; split + parse is clear and sufficient |
| Skip invalid tokens | Contradicts FR-C03 fail-fast clarification |

---

## R4 — Separation of responsibilities

**Decision**: Three internal units — `IntegerSequenceParser`, `ContiguousRunEvaluator`, `RunFormatter` — orchestrated by `SubSequenceHandler`.

**Rationale**:

- Constitution Principle IX.
- Each unit is independently unit-testable.
- No DI container; static or simple instance methods.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| All logic in `SubSequenceHandler` | Violates separation; harder to test parsing vs evaluation |
| Interface + DI for each service | Over-engineered for exercise scope (Principle XI) |

---

## R5 — Test organisation

**Decision**:

- `MainBusinessLogicUnitTests` — exactly 11 tests mapped to AC-001…AC-011 (`Test_Case_One`…`Test_Case_Eleven`).
- `Supplementary/InvalidInputTests` — FR-C01…FR-C03 and derived edge cases.
- Large inputs loaded from embedded resources copied from `specs/.../acceptance/ac-NN-input.txt`.

**Rationale**:

- Constitution Principle III: supplied tests separate from supplementary.
- Preserves exact evaluator strings without editing.
- `[Trait("Category", "Unit_Tests")]` on supplied class matches reference format.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| Single test class for all tests | Blurs supplied vs supplementary boundary |
| Read fixtures from spec path at runtime | Brittle when tests run from different working directories |

---

## R6 — .NET quality tooling

**Decision**: Root `Directory.Build.props` + `.editorconfig`; CI runs `dotnet format --verify-no-changes`; `TreatWarningsAsErrors` + NET analyzers enabled.

**Rationale**:

- Constitution “Modern .NET Quality Controls”.
- Central props apply to all three projects consistently.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| Per-project settings only | Duplication; drift risk |
| StyleCop vs built-in analyzers | Built-in analyzers sufficient; fewer dependencies |

---

## R7 — CI timing

**Decision**: Add GitHub Actions immediately after solution skeleton (Phase A); add coverage collection and Docker validation after all 11 supplied tests pass (Phase F).

**Rationale**:

- Constitution “Continuous Integration” — CI early.
- Coverage and Docker are secondary objectives; depend on working core.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| CI only at end | Misses continuous verification during development |
| Full pipeline before any code | Need skeleton projects to restore/build |

---

## R8 — Docker multi-stage build

**Decision**: Three-stage Dockerfile colocated with `CodingTestCli`:

1. **build** — `mcr.microsoft.com/dotnet/sdk:8.0`, restore + publish Release.
2. **publish** — (optional merge with build) output to `/app/publish`.
3. **runtime** — `mcr.microsoft.com/dotnet/runtime:8.0`, copy publish output, `ENTRYPOINT ["dotnet", "CodingTestCli.dll"]`.

Pass input via `docker run ... <args>` or stdin pipe.

**Rationale**:

- Exercise secondary objective: containerisation.
- Runtime image avoids SDK in production layer.
- CLI project is the natural container entry point.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| Single-stage SDK image | Larger image; not best practice |
| Containerise test project | Evaluator needs runnable demo, not test runner |

---

## R9 — AI-assisted development log

**Decision**: Maintain `docs/ai-development-log.md` with chronological entries: prompt summary, AI suggestion, human decision, verification command/output.

**Rationale**:

- Constitution “Controlled AI-Assisted Development”.
- Provides audit trail without embedding org names.

**Alternatives considered**:

| Alternative | Rejected because |
| ----------- | ---------------- |
| Inline code comments for AI notes | Clutters production code |
| No log | Misses constitution documentation requirement |

---

## R10 — Empty and single-token behaviour

**Decision**:

- Empty input string → return `""` (FR-C02).
- Single valid token → return that token unchanged (derived run of length 1).

**Rationale**:

- FR-C02 clarified explicitly.
- Single token is the base case of run logic (`bestLength = 1`).

**Alternatives considered**: None requiring clarification; behaviour follows from spec + algorithm.

---

## Open items deferred to implementation

| Item | Deferred to | Reason |
| ---- | ----------- | ------ |
| Exact exception messages | Implementation | Not part of behavioural contract |
| CLI argument vs stdin precedence | `Program.cs` | UX detail; document in README |
| Coverage threshold % | CI config | Exercise requires reporting, not a fixed gate |
