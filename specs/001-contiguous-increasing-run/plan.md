# Implementation Plan: Longest Contiguous Strictly Increasing Run

**Branch**: `001-contiguous-increasing-run` | **Date**: 2026-08-03 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/001-contiguous-increasing-run/spec.md` plus approved plan constraints (`.NET 8`, project layout, O(n) algorithm, quality gates, CI, Docker).

## Summary

Implement a .NET 8 class library that finds the **longest contiguous strictly increasing run** in a whitespace-separated integer string and returns it formatted with single-space delimiters. A console application provides a manual verification entry point. All behaviour is locked by 11 supplied evaluator tests (added before production code) plus supplementary tests for clarified invalid-input rules (FR-C01–FR-C03).

The core algorithm is a **single-pass O(n) scan** tracking `currentStart`, `currentLength`, `bestStart`, and `bestLength`. The best run is replaced **only** when `currentLength > bestLength`, preserving the earliest run on ties.

**Namespace note**: The functional spec references `String.Handlers`; this plan adopts **`CodingTest.Handlers`** per approved implementation contract (see [contracts/public-api.md](./contracts/public-api.md)).

## Technical Context

**Language/Version**: C# 12 / .NET 8 (`net8.0`)

**Primary Dependencies**:

| Project | Purpose | Key packages |
| ------- | ------- | ------------ |
| `src/CodingTest` | Production library | None (BCL only) |
| `src/CodingTestCli` | Console entry point | Project reference → `CodingTest` |
| `tests/CodingTestUnitTests` | Automated tests | xUnit, FluentAssertions, coverlet |

**Storage**: N/A (in-memory string processing only)

**Testing**: xUnit + FluentAssertions; coverlet for coverage after core implementation passes

**Target Platform**: Cross-platform (.NET 8 on Windows, Linux, macOS; Docker for CLI)

**Project Type**: Class library + CLI + unit test suite

**Performance Goals**: O(n) time, O(1) auxiliary space beyond parsed values (single linear scan; no multi-pass evaluation)

**Constraints**:

- Nullable reference types enabled
- Warnings treated as errors
- `dotnet format` enforced in CI
- .NET analyzers enabled
- No DI, ORM, web stack, or unnecessary abstractions
- Repository anonymity (no organisation names in code, docs, workflows, or Docker metadata)

**Scale/Scope**: Inputs up to ~4,000+ tokens (AC-009); linear scan must handle evaluator-scale sequences without super-linear algorithms

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Plan compliance | Notes |
| --------- | ---------------- | ----- |
| I. Authoritative behavioural contract | PASS | All 11 AC fixtures drive evaluator test class |
| II. Specification-first | PASS | Spec clarified; tests precede handler implementation |
| III. Test-first (NON-NEGOTIABLE) | PASS | 11 supplied tests in dedicated class before `SubSequenceHandler` body |
| IV. Clean, idiomatic C# | PASS | Static handler, small focused types, minimal comments |
| V. Evaluator-compatible public contract | PASS | `CodingTest.Handlers.SubSequenceHandler.GetLongestIncreasingSubSequence(string)` |
| VI. Contiguous strictly increasing semantics | PASS | O(n) scan; AC-010 disproves non-contiguous LIS |
| VII. Sequence evaluation rules | PASS | Strict `>` continues run; `>=` terminates; earliest best via `>` only update |
| VIII. Single-pass O(n) | PASS | One index loop; no nested re-scan |
| IX. Separation of responsibilities | PASS | Parsing, evaluation, formatting in distinct units |
| X. Invalid-input documentation | PASS | FR-C01–FR-C03 covered by supplementary tests |
| XI. Simplicity over architecture | PASS | Three projects only; no extra layers |
| Modern .NET quality controls | PASS | Nullable, analyzers, warnings-as-errors, editorconfig |
| CI immediately after skeleton | PASS | GitHub Actions in phase 2 (post-skeleton) |
| Completion criteria | PLANNED | Coverage + Docker after core tests green |

**Post-design re-check**: All gates pass. No constitution violations requiring complexity tracking.

## Project Structure

### Documentation (this feature)

```text
specs/001-contiguous-increasing-run/
├── spec.md
├── plan.md                 # This file
├── research.md             # Phase 0 decisions
├── data-model.md           # Phase 1 data shapes
├── quickstart.md           # Verification guide
├── contracts/
│   └── public-api.md       # Public API contract
├── acceptance/             # Verbatim AC-001…AC-011 fixtures
│   ├── ac-NN-input.txt
│   └── ac-NN-expected.txt
└── tasks.md                # Created by /speckit-tasks (not this command)
```

### Source Code (repository root)

```text
CodingTest.sln
.editorconfig
Directory.Build.props          # Shared: nullable, warnings as errors, analyzers
.github/
└── workflows/
    └── ci.yml                 # restore → format → build → test → (coverage later)
docs/
└── ai-development-log.md      # AI-assisted decision record
src/
├── CodingTest/
│   ├── CodingTest.csproj
│   ├── Handlers/
│   │   └── SubSequenceHandler.cs      # Public façade
│   ├── Parsing/
│   │   └── IntegerSequenceParser.cs   # Tokenize + parse + validate
│   ├── Evaluation/
│   │   └── ContiguousRunEvaluator.cs  # O(n) run tracking
│   └── Formatting/
│       └── RunFormatter.cs            # int[] → space-delimited string
└── CodingTestCli/
    ├── CodingTestCli.csproj
    ├── Program.cs                     # Reads stdin or args; writes result
    └── Dockerfile                     # Multi-stage build (see research.md)
tests/
└── CodingTestUnitTests/
    ├── CodingTestUnitTests.csproj
    ├── Supplied/
    │   └── MainBusinessLogicUnitTests.cs  # AC-001…AC-011 only
    ├── Supplementary/
    │   └── InvalidInputTests.cs       # FR-C01…FR-C03
    └── Fixtures/
        └── AcceptanceFixtureReader.cs # Loads acceptance/ac-NN-*.txt
```

**Structure Decision**: Three-project layout separates reusable library (`CodingTest`), runnable CLI (`CodingTestCli`), and tests (`CodingTestUnitTests`). Internal folders (`Parsing`, `Evaluation`, `Formatting`) enforce constitution Principle IX without introducing frameworks.

## Implementation Phases

### Phase A — Solution skeleton (no business logic)

1. Create `CodingTest.sln` with three projects under `src/` and `tests/`.
2. Add root `Directory.Build.props` and `.editorconfig`.
3. Wire project references: `CodingTestCli` → `CodingTest`; `CodingTestUnitTests` → `CodingTest`.
4. Add stub `SubSequenceHandler` throwing `NotImplementedException`.
5. Add GitHub Actions workflow: `dotnet restore`, `dotnet format --verify-no-changes`, `dotnet build`, `dotnet test`.

### Phase B — Test-first (evaluator cases)

1. Add xUnit + FluentAssertions to test project.
2. Implement `MainBusinessLogicUnitTests` with 11 tests named `Test_Case_One` … `Test_Case_Eleven` matching `requirements/reference/unit_test_format.txt`.
3. Load large-case inputs/outputs from `specs/001-contiguous-increasing-run/acceptance/ac-NN-*.txt` (copy or link as embedded resources — prefer **embedded resources** under `tests/.../Fixtures/` copied verbatim from acceptance fixtures to keep tests self-contained).
4. Confirm all 11 tests fail (red) against stub handler.

### Phase C — Core implementation

1. **Parsing** (`IntegerSequenceParser`):
   - Null → throw `ArgumentNullException`.
   - Empty string → return empty `ReadOnlySpan<int>` or empty list.
   - Split on ASCII space; reject empty tokens (`FormatException`).
   - Parse with `int.TryParse` / `int.Parse`; invalid → `FormatException`; overflow → `OverflowException`.

2. **Evaluation** (`ContiguousRunEvaluator`):
   - Single loop index `i` from `0` to `n-1`.
   - State: `currentStart`, `currentLength`, `bestStart`, `bestLength`, `previousValue`.
   - If `i == 0` or `values[i] > previousValue`: extend current run (`currentLength++` or start at 1).
   - Else: reset `currentStart = i`, `currentLength = 1`.
   - If `currentLength > bestLength`: update `bestStart`, `bestLength` (**not** `>=`).
   - Return slice `[bestStart .. bestStart + bestLength)`.

3. **Formatting** (`RunFormatter`): Join with single space; no leading/trailing space.

4. **Handler** (`SubSequenceHandler`): Orchestrate parse → evaluate → format.

5. All 11 supplied tests green.

### Phase D — Supplementary tests + invalid input

1. Add `InvalidInputTests` for FR-C01–FR-C03.
2. Add edge cases: single token, whitespace-only (if in scope), tie-break sanity.

### Phase E — CLI + README

1. `CodingTestCli`: accept input string as argument or stdin; print result.
2. Expand `README.md` with build, test, format, Docker, and CI verification steps (see [quickstart.md](./quickstart.md)).

### Phase F — Coverage + Docker (after core green)

1. Add coverlet to test project; publish coverage in CI (e.g. `dotnet test --collect:"XPlat Code Coverage"`).
2. Multi-stage `Dockerfile` for `CodingTestCli` (build → publish → runtime).
3. Add Docker build/run step to CI (optional job or stage after tests pass).

### Phase G — AI development log

1. Maintain `docs/ai-development-log.md` with dated entries: decisions, AI suggestions accepted/rejected, verification performed.

## Algorithm Specification

```
Given: values[0..n-1]

currentStart ← 0
currentLength ← 0
bestStart ← 0
bestLength ← 0

for i from 0 to n-1:
    if i == 0 OR values[i] > values[i - 1]:
        if currentLength == 0:
            currentStart ← i
            currentLength ← 1
        else:
            currentLength ← currentLength + 1
    else:
        currentStart ← i
        currentLength ← 1

    if currentLength > bestLength:
        bestStart ← currentStart
        bestLength ← currentLength

return values[bestStart .. bestStart + bestLength)
```

**Tie-break proof**: Replacing best only on strict `>` means the first run achieving `bestLength` is retained; later equal-length runs are ignored.

## Quality & Tooling

| Control | Location | Setting |
| ------- | -------- | ------- |
| Nullable | `Directory.Build.props` | `<Nullable>enable</Nullable>` |
| Warnings as errors | `Directory.Build.props` | `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` |
| Analyzers | `Directory.Build.props` | `EnableNETAnalyzers`, analysis level latest |
| Formatting | `.editorconfig` + CI | `dotnet format --verify-no-changes` |
| Coverage | Test csproj + CI | coverlet collector after Phase C |

## CI Pipeline (initial skeleton)

```yaml
# .github/workflows/ci.yml (conceptual stages)
# 1. checkout
# 2. setup-dotnet (8.x)
# 3. dotnet restore
# 4. dotnet format --verify-no-changes
# 5. dotnet build --configuration Release
# 6. dotnet test --configuration Release --no-build
# (later) 7. coverage upload
# (later) 8. docker build
```

## Docker (multi-stage, CLI)

See [research.md](./research.md) for stage breakdown: SDK build → publish → `mcr.microsoft.com/dotnet/runtime:8.0` runtime image with published `CodingTestCli` DLL entrypoint.

## Complexity Tracking

> No constitution violations. Three projects are justified: library (testable core), CLI (verification/Docker), tests (isolated verification).

| Decision | Why needed | Simpler alternative rejected |
| -------- | ---------- | ------------------------------ |
| Separate Parsing / Evaluation / Formatting types | Principle IX separation | Single god-method harder to test and read |
| Embedded fixture resources | Large AC inputs (~4k tokens) | Inline strings in test file harm readability |
| CLI project | Docker + manual verification | Library-only cannot satisfy containerised demo |

## Artifact Index

| Artifact | Path |
| -------- | ---- |
| Research decisions | [research.md](./research.md) |
| Data model | [data-model.md](./data-model.md) |
| Public API contract | [contracts/public-api.md](./contracts/public-api.md) |
| Verification guide | [quickstart.md](./quickstart.md) |
| Acceptance fixtures | [acceptance/](./acceptance/) |

## Next Step

Run **`/speckit-tasks`** to generate dependency-ordered `tasks.md`, then **`/speckit-implement`** to execute tasks (implementation phase — not part of this command).
