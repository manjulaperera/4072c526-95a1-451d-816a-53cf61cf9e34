---
description: "Task list for longest contiguous strictly increasing run feature"
---

# Tasks: Longest Contiguous Strictly Increasing Run

**Input**: Design documents from `/specs/001-contiguous-increasing-run/`  
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/public-api.md](./contracts/public-api.md)

**Tests**: Included — constitution Principle III (test-first) and plan Phase B require 11 supplied evaluator tests **and** failing supplementary tests before production implementation.

**Organization**: Tasks grouped by user story. US1 = core evaluator behaviour (P1). US2 = supplementary invalid-input tests (FR-C01–FR-C03). US3 = CLI verification entry point.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies on incomplete tasks)
- **[Story]**: US1, US2, or US3
- Every task includes an exact file path

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: .NET 8 solution skeleton with quality tooling

- [x] T001 Create solution file `CodingTest.sln` at repository root
- [x] T002 [P] Create class library project `src/CodingTest/CodingTest.csproj` targeting `net8.0`
- [x] T003 [P] Create console project `src/CodingTestCli/CodingTestCli.csproj` targeting `net8.0`
- [x] T004 [P] Create test project `tests/CodingTestUnitTests/CodingTestUnitTests.csproj` targeting `net8.0` with xUnit and FluentAssertions package references
- [x] T005 Add all projects to `CodingTest.sln` and add project references: `src/CodingTestCli/CodingTestCli.csproj` → `src/CodingTest/CodingTest.csproj`; `tests/CodingTestUnitTests/CodingTestUnitTests.csproj` → `src/CodingTest/CodingTest.csproj`
- [x] T006 [P] Create shared MSBuild properties in `Directory.Build.props` (nullable enable, ImplicitUsings enable, TreatWarningsAsErrors, EnableNETAnalyzers, AnalysisLevel latest, EnforceCodeStyleInBuild, ContinuousIntegrationBuild)
- [x] T007 [P] Create code style rules in `.editorconfig` at repository root

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Stub handler, fixture infrastructure, and CI — MUST complete before user story implementation

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T008 Create stub public handler in `src/CodingTest/Handlers/SubSequenceHandler.cs` (`CodingTest.Handlers` namespace; `GetLongestIncreasingSubSequence` throws `NotImplementedException`)
- [x] T009 [P] Copy verbatim acceptance fixtures from `specs/001-contiguous-increasing-run/acceptance/ac-NN-input.txt` and `ac-NN-expected.txt` into `tests/CodingTestUnitTests/Fixtures/` as embedded resources in `tests/CodingTestUnitTests/CodingTestUnitTests.csproj`
- [x] T010 Create fixture loader `tests/CodingTestUnitTests/Fixtures/AcceptanceFixtureReader.cs` to read embedded AC-001…AC-011 inputs and expected outputs
- [x] T011 Create GitHub Actions workflow `.github/workflows/ci.yml` with restore, `dotnet format --verify-no-changes`, build, and test steps for `CodingTest.sln`
- [x] T012 Verify solution skeleton builds with zero warnings via `dotnet build CodingTest.sln --configuration Release`

**Checkpoint**: Foundation ready — user story implementation can begin

---

## Phase 3: User Story 1 — Find longest contiguous increasing run (Priority: P1) 🎯 MVP

**Goal**: `SubSequenceHandler.GetLongestIncreasingSubSequence` returns the longest contiguous strictly increasing run for all 11 supplied evaluator cases (AC-001…AC-011).

**Independent Test**: `dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Unit_Tests"` → 11 passed

### Tests for User Story 1 ⚠️ Write FIRST — must FAIL before implementation

> **NOTE: Confirm all supplied tests fail (red) against the T008 stub before T016–T019**

- [x] T013 [US1] Create supplied evaluator test class `tests/CodingTestUnitTests/Supplied/MainBusinessLogicUnitTests.cs` with `[Trait("Category","Unit_Tests")]` and methods `Test_Case_One`…`Test_Case_Eleven` (matching `requirements/reference/unit_test_format.txt`) using Given/When/Then and FluentAssertions; load inputs/outputs from `AcceptanceFixtureReader` preserving exact strings
- [x] T014 [US1] Run supplied tests and confirm all 11 fail against stub in `src/CodingTest/Handlers/SubSequenceHandler.cs`

### Supplementary tests ⚠️ Write FIRST — must FAIL before invalid-input implementation

> **NOTE: T015 must complete (red) before T021 implements FR-C01–FR-C03**

- [x] T015 [US2] Create supplementary test class `tests/CodingTestUnitTests/Supplementary/InvalidInputTests.cs` with `[Trait("Category","Supplementary")]` covering FR-C01 (`ArgumentNullException`), FR-C02 (empty → `""`), FR-C03 (`FormatException` on invalid/empty tokens including repeated spaces and leading/trailing spaces, `OverflowException` on overflow), plus single-token input (`"42"` → `"42"`); confirm all supplementary tests fail against stub

### Implementation for User Story 1 (valid evaluator paths only)

- [x] T016 [P] [US1] Implement `IntegerSequenceParser` in `src/CodingTest/Parsing/IntegerSequenceParser.cs` for **valid evaluator inputs only** (ASCII space split; parse base-10 int32 tokens; defer FR-C01–FR-C03 guards to T021)
- [x] T017 [P] [US1] Implement O(n) `ContiguousRunEvaluator` in `src/CodingTest/Evaluation/ContiguousRunEvaluator.cs` tracking `currentStart`, `currentLength`, `bestStart`, `bestLength`; continue run only on strict `>`; update best only when `currentLength > bestLength`
- [x] T018 [P] [US1] Implement `RunFormatter` in `src/CodingTest/Formatting/RunFormatter.cs` (join with single ASCII space; no leading/trailing space)
- [x] T019 [US1] Wire parse → evaluate → format in `src/CodingTest/Handlers/SubSequenceHandler.cs` for valid inputs
- [x] T020 [US1] Verify all 11 supplied evaluator tests pass: `dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Unit_Tests" --configuration Release`

**Checkpoint**: User Story 1 complete — MVP delivers full evaluator compatibility (supplementary tests still expected red until Phase 4)

---

## Phase 4: User Story 2 — Invalid and boundary input (Priority: P2)

**Goal**: Documented invalid-input behaviour (FR-C01–FR-C03) implemented and green in supplementary test suite.

**Independent Test**: `dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Supplementary"` → all supplementary tests pass

### Implementation for User Story 2

- [x] T021 [US2] Extend `src/CodingTest/Parsing/IntegerSequenceParser.cs` and `src/CodingTest/Handlers/SubSequenceHandler.cs` to satisfy FR-C01–FR-C03
- [x] T022 [US2] Verify supplementary tests pass

**Checkpoint**: Invalid-input contract tested and green

---

## Phase 5: User Story 3 — CLI verification entry point (Priority: P3)

**Goal**: Console application invokes handler for manual and containerised verification.

**Independent Test**: `dotnet run --project src/CodingTestCli/CodingTestCli.csproj -- "6 1 5 9 2"` prints `1 5 9`

### Implementation for User Story 3

- [x] T023 [US3] Implement CLI entry point in `src/CodingTestCli/Program.cs`
- [x] T024 [US3] Run manual smoke tests per `specs/001-contiguous-increasing-run/quickstart.md` section 4 using `src/CodingTestCli/Program.cs`

**Checkpoint**: CLI independently verifies core behaviour

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Coverage, Docker, documentation, and full quickstart validation

- [x] T025 [P] Add coverlet.collector package reference to `tests/CodingTestUnitTests/CodingTestUnitTests.csproj`
- [x] T026 Extend `.github/workflows/ci.yml` with coverage collection step (`--collect:"XPlat Code Coverage"`)
- [x] T027 Create multi-stage `src/CodingTestCli/Dockerfile` (SDK build/publish → runtime image with `ENTRYPOINT ["dotnet", "CodingTestCli.dll"]`)
- [x] T028 [P] Add Docker build validation job/stage to `.github/workflows/ci.yml`
- [x] T029 [P] Expand `README.md` with build, test, format, CLI, Docker, and CI verification instructions per `specs/001-contiguous-increasing-run/quickstart.md` section 10
- [x] T030 Update `docs/ai-development-log.md` with implementation completion entry and verification commands run
- [x] T031 Run full quickstart validation checklist in `specs/001-contiguous-increasing-run/quickstart.md` section 10 (Definition of done)

---

## Phase 7: SonarCloud code quality and coverage

**Purpose**: Publish coverage and quality metrics to SonarCloud for dashboard visibility

- [x] T032 [P] Create `sonar-project.properties` at repository root (project key, OpenCover report path, source/test exclusions)
- [x] T033 [P] Create `coverlet.runsettings` with OpenCover output format for SonarCloud import
- [x] T034 Extend `.github/workflows/ci.yml` with `dotnet-sonarscanner` begin/end gated by `SONAR_ENABLED` repo variable; require `SONAR_TOKEN` secret, `SONAR_ORGANIZATION` and `SONAR_PROJECT_KEY` repo variables
- [x] T035 Document SonarCloud setup in `README.md` (create project on SonarCloud first; add badges from SonarCloud UI after import)

---

## Dependencies & Execution Order

### Phase Dependencies

| Phase | Depends on | Blocks |
| ----- | ---------- | ------ |
| 1 Setup | — | Phase 2 |
| 2 Foundational | Phase 1 | Phases 3–5 |
| 3 US1 (P1) | Phase 2 | MVP demo |
| 4 US2 (P2) | Phase 3 tests (T015 red); US1 core (T019) | — |
| 5 US3 (P3) | Phase 3 (T020 green) | — |
| 6 Polish | Phases 3–5 | Done |

### User Story Dependencies

- **US1 (P1)**: Starts after Phase 2 — supplied tests (T013–T014) before core implementation (T016–T019)
- **US2 (P2)**: Supplementary tests (T015) before FR-C implementation (T021); can overlap US1 core once T015 is red
- **US3 (P3)**: Starts after US1 (T020 green); CLI is thin wrapper only

### Within User Story 1

1. T013–T014: Supplied tests written and failing (red)
2. T015: Supplementary tests written and failing (red)
3. T016–T018: Internal components for valid paths (parallel)
4. T019: Handler wiring
5. T020: All 11 supplied tests green

---

## Parallel Opportunities

### Phase 1 (after T001)

```text
T002 src/CodingTest/CodingTest.csproj
T003 src/CodingTestCli/CodingTestCli.csproj
T004 tests/CodingTestUnitTests/CodingTestUnitTests.csproj
T006 Directory.Build.props
T007 .editorconfig
```

### Phase 3 US1 implementation (after T015 red)

```text
T016 src/CodingTest/Parsing/IntegerSequenceParser.cs
T017 src/CodingTest/Evaluation/ContiguousRunEvaluator.cs
T018 src/CodingTest/Formatting/RunFormatter.cs
```

### Phase 6 polish

```text
T025 tests/CodingTestUnitTests/CodingTestUnitTests.csproj
T028 .github/workflows/ci.yml
T029 README.md
```

---

## Parallel Example: User Story 1

```bash
# Step 1 — write failing supplied tests (T013 then T014)
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Unit_Tests"
# Expected: 11 failed

# Step 2 — write failing supplementary tests (T015)
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Supplementary"
# Expected: all failed

# Step 3 — implement valid-path components (T016, T017, T018), wire (T019)
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Unit_Tests" --configuration Release
# Expected: 11 passed

# Step 4 — implement FR-C (T021), verify supplementary (T022)
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --filter "Category=Supplementary" --configuration Release
# Expected: all passed
```

---

## Implementation Strategy

### MVP First (User Story 1 only)

1. Complete Phase 1: Setup (T001–T007)
2. Complete Phase 2: Foundational (T008–T012)
3. Complete Phase 3: Supplied + supplementary tests red → valid-path implementation → 11/11 green (T013–T020)
4. **STOP and VALIDATE**: 11/11 supplied evaluator tests pass
5. Complete Phase 4: FR-C implementation (T021–T022)

### Incremental Delivery

1. Setup + Foundational → CI green on skeleton
2. US1 → 11 evaluator tests pass → **MVP**
3. US2 → supplementary invalid-input tests pass
4. US3 → CLI smoke tests pass
5. Polish → coverage, Docker, README, full quickstart done

---

## Task Summary

| Metric | Count |
| ------ | ----- |
| **Total tasks** | 35 |
| Phase 1 Setup | 7 |
| Phase 2 Foundational | 5 |
| Phase 3 US1 (+ supplementary tests) | 8 |
| Phase 4 US2 | 2 |
| Phase 5 US3 | 2 |
| Phase 6 Polish | 7 |
| Phase 7 SonarCloud | 4 |

| User story | Tasks | Independent test |
| ---------- | ----- | ---------------- |
| US1 (P1) | T013–T014, T016–T020 | 11 supplied tests pass |
| US2 (P2) | T015, T021–T022 | Supplementary FR-C tests pass |
| US3 (P3) | T023–T024 | CLI prints `1 5 9` for sample input |

**MVP scope**: Phase 1 + Phase 2 + Phase 3 (T001–T020) — delivers full evaluator compatibility.

---

## Notes

- Preserve exact AC fixture strings; diff against `specs/001-contiguous-increasing-run/acceptance/` if any large test fails
- Supplied test class MUST be `MainBusinessLogicUnitTests` with `Test_Case_One`…`Test_Case_Eleven` per `requirements/reference/unit_test_format.txt`
- Public API namespace is `CodingTest.Handlers` per `specs/001-contiguous-increasing-run/contracts/public-api.md`
- Do not replace best run on equal length (`currentLength > bestLength` only) — AC-010 depends on this
- Remove default scaffold test files (e.g. `UnitTest1.cs`) if template generates them
- No organisation names in any file (constitution repository anonymity)

