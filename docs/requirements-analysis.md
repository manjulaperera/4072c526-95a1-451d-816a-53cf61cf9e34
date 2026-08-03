# Requirements Analysis

This document captures the behavioural contract derived from the supplied requirement
artefacts. It does not add requirements beyond what those files support.

## 1. Authoritative Source Files

| File | Role | Status |
|------|------|--------|
| `requirements/code-test.md` | Problem statement and all 11 evaluator input/output pairs | Present |
| `requirements/reference/BusinessLogicUnitTests.cs` | Expected evaluator test structure and public API usage | **Not present in the repository** |

### Available reference artefact

The repository currently contains `requirements/reference/unit_test_format.txt`, which
provides one example xUnit test (`Test_Case_One`) calling the production API. It uses
test class name `MainBusinessLogicUnitTests` (not `BusinessLogicUnitTests`). Until
`BusinessLogicUnitTests.cs` is supplied, `code-test.md` and `unit_test_format.txt` are
the only authoritative sources available.

---

## 2. Required Public Contract

Values below are taken from `unit_test_format.txt` and `code-test.md`. Where a field is
not stated in those files, it is marked as unspecified.

| Contract element | Value | Source |
|------------------|-------|--------|
| Namespace | **Not specified** | — |
| Class name | `SubSequenceHandler` | `unit_test_format.txt` |
| Method name | `GetLongestIncreasingSubSequence` | `unit_test_format.txt` |
| Parameter type | `string` (`input`) | `unit_test_format.txt` |
| Return type | `string` | `unit_test_format.txt` (inferred from usage) |

The method is invoked as `SubSequenceHandler.GetLongestIncreasingSubSequence(input)`.
Static versus instance binding is not explicitly declared in the supplied files.

---

## 3. Summary of All 11 Supplied Evaluator Cases

All inputs and expected outputs are defined in `requirements/code-test.md`.

| ID | Input (summary) | Expected output | Run length |
|----|-----------------|-----------------|------------|
| AC-001 / Test_Case_01 | `6 1 5 9 2` | `1 5 9` | 3 |
| AC-002 / Test_Case_02 | Large sequence (~2000 integers; full input in `code-test.md`) | `1710 2461 9288 10195 10431 12485` | 6 |
| AC-003 / Test_Case_03 | Large sequence (~2000 integers; full input in `code-test.md`) | `10298 10897 12291 15037 18446 23435 25333 27266` | 8 |
| AC-004 / Test_Case_04 | 65 integers beginning `923 11613 30483 …` (full input in `code-test.md`) | `3862 16353 22813 28735` | 4 |
| AC-005 / Test_Case_05 | ~100 integers (full input in `code-test.md`) | `11084 11970 24975 30922` | 4 |
| AC-006 / Test_Case_06 | ~200 integers (full input in `code-test.md`) | `3808 3908 10386 19306` | 4 |
| AC-007 / Test_Case_07 | ~500 integers (full input in `code-test.md`) | `125 1841 5882 18464 28317 31497` | 6 |
| AC-008 / Test_Case_08 | ~500 integers (full input in `code-test.md`) | `9139 17687 25106 26202 27592 30937` | 6 |
| AC-009 / Test_Case_09 | Very large sequence (~4000+ integers; full input in `code-test.md`) | `918 1089 5133 7725 18035 24605 26716 27095` | 8 |
| AC-010 / Test_Case_10 | `6 2 4 6 1 5 9 2` | `2 4 6` | 3 |
| AC-011 / Test_Case_11 | `6 2 4 3 1 5 9` | `1 5 9` | 3 |

---

## 4. Behavioural Rules Demonstrated by the Supplied Cases

These rules are supported by the problem statement in `code-test.md` and by the
expected outputs of the 11 evaluator cases.

### Contiguous strictly increasing run

A valid run is a **contiguous** slice of the input in which each value is **strictly
greater** than the immediately preceding value. The algorithm must return the longest
such run (see §5 for proof this is not classical non-contiguous LIS).

### Equal or smaller values terminate the run

When the next integer is equal to or less than the previous value in the current
contiguous slice, the current run ends and a new run begins at that value.

- **AC-011** (`6 2 4 3 1 5 9` → `1 5 9`): the run `2 4` terminates at `3`; the winning
  run starts at `1`.
- **AC-010** (`6 2 4 6 1 5 9 2` → `2 4 6`): the run `2 4 6` terminates before `1`.

### Earliest maximum-length run wins

When two or more runs share the maximum length, the run that **starts at the smallest
index** in the input must be returned.

- **AC-010** is the decisive case: runs `2 4 6` (index 1) and `1 5 9` (index 4) both
  have length 3; expected output is `2 4 6`, the earlier run.
- This matches the problem statement: *"If more than 1 sequence exists with the
  longest length, output the earliest one."*

### Output values are separated by one space

Every expected output in `code-test.md` uses single ASCII spaces between integers, with
no leading or trailing space shown (e.g. `1 5 9`, not ` 1 5 9 ` or `1  5  9`).

---

## 5. Evidence This Is Not Classical Non-Contiguous LIS

**Classical longest increasing subsequence (LIS)** allows choosing non-contiguous elements
by index. **This exercise expects contiguous runs**, as shown by the evaluator outputs.

### Decisive evidence — AC-010 / Test_Case_10

| | |
|-|-|
| **Input** | `6 2 4 6 1 5 9 2` |
| **Expected output** | `2 4 6` (length 3) |

**Contiguous-run reading**

| Index | Value | Active run |
|-------|-------|------------|
| 0 | 6 | `6` |
| 1–3 | 2 4 6 | `2 4 6` ← length 3 |
| 4–6 | 1 5 9 | `1 5 9` ← length 3 |
| 7 | 2 | `2` |

Earliest length-3 run: `2 4 6` — matches expected output.

**Classical LIS reading**

Non-contiguous subsequence **2, 4, 6, 9** exists at indices 1, 2, 3, 6 — **length 4**.

Classical LIS would require a length-4 result. The supplied expected output is length 3,
so the contract is **not** classical non-contiguous LIS.

### Supporting evidence

| Case | Input | Output | Note |
|------|-------|--------|------|
| AC-001 | `6 1 5 9 2` | `1 5 9` | Contiguous block at indices 1–3 |
| AC-011 | `6 2 4 3 1 5 9` | `1 5 9` | Contiguous block at indices 4–6; earlier run `2 4` is shorter |

Cases AC-002 through AC-009 are large regression inputs consistent with the
contiguous-run model but do not alone distinguish it from classical LIS.

---

## 6. Behaviours Not Specified by the Supplied Files

The following edge cases and error conditions are **not defined** in `code-test.md` or
`unit_test_format.txt`. Any handling must be documented separately before
implementation; it is not part of the current evaluator contract.

| Unspecified behaviour | Notes |
|-----------------------|-------|
| **Null input** | No requirement for `null` string parameter |
| **Empty input** | No requirement for `""` or whitespace-only strings |
| **Malformed tokens** | No requirement for non-numeric or partial tokens |
| **Integer overflow** | No requirement for values outside `int` (or other) range |
| **Repeated whitespace** | Problem describes normal input as single-separated integers; behaviour for multiple or irregular spaces is not defined |

Additional unspecified items:

- Namespace for `SubSequenceHandler`
- Explicit `public` / `static` modifiers
- Exception type, message, or error return on failure
- Minimum run length when no increasing pair exists (implicitly length 1 for any single
  element, but not formally stated)

---

## 7. Traceability Table

Maps acceptance criteria (from `code-test.md` test cases) to evaluator test method
names. Method naming follows the convention requested for the full evaluator suite;
only `Test_Case_One` appears in the available `unit_test_format.txt` reference.

| Acceptance criterion | Evaluator test | Input (summary) | Expected output |
|---------------------|----------------|-----------------|-------------------|
| AC-001 | Test_Case_01 | `6 1 5 9 2` | `1 5 9` |
| AC-002 | Test_Case_02 | Large sequence (~2000 integers) | `1710 2461 9288 10195 10431 12485` |
| AC-003 | Test_Case_03 | Large sequence (~2000 integers) | `10298 10897 12291 15037 18446 23435 25333 27266` |
| AC-004 | Test_Case_04 | 65 integers (see `code-test.md`) | `3862 16353 22813 28735` |
| AC-005 | Test_Case_05 | ~100 integers (see `code-test.md`) | `11084 11970 24975 30922` |
| AC-006 | Test_Case_06 | ~200 integers (see `code-test.md`) | `3808 3908 10386 19306` |
| AC-007 | Test_Case_07 | ~500 integers (see `code-test.md`) | `125 1841 5882 18464 28317 31497` |
| AC-008 | Test_Case_08 | ~500 integers (see `code-test.md`) | `9139 17687 25106 26202 27592 30937` |
| AC-009 | Test_Case_09 | ~4000+ integers (see `code-test.md`) | `918 1089 5133 7725 18035 24605 26716 27095` |
| AC-010 | Test_Case_10 | `6 2 4 6 1 5 9 2` | `2 4 6` |
| AC-011 | Test_Case_11 | `6 2 4 3 1 5 9` | `1 5 9` |

Full inputs for AC-002 through AC-009 are recorded only in `requirements/code-test.md`.
