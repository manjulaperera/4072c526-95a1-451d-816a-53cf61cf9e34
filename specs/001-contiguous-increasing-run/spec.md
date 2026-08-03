# Feature Specification: Longest Contiguous Strictly Increasing Run

**Feature Directory**: `specs/001-contiguous-increasing-run`

**Created**: 2026-08-03

**Status**: Clarified

**Input**: Build a C# function that accepts one string containing any number of integers separated by whitespace and returns the longest contiguous strictly increasing run.

**Authoritative behavioural sources**:

- `requirements/code-test.md` — problem statement and all 11 evaluator test cases
- `requirements/reference/unit_test_format.txt` — evaluator test structure and public method signature example

## Clarifications

### Session 2026-08-03

- Q: What should the function return or throw when `input` is `null`? → A: Throw `ArgumentNullException`.
- Q: What should the function return when `input` is an empty string (`""`)? → A: Return empty string `""`.
- Q: How should the function behave when the input contains tokens that are not valid base-10 integers? → A: Throw an exception on the first invalid token (fail-fast; no partial result).

---

## Public Contract *(supplied)*

The following surface is required by the exercise brief and reference test format:

```csharp
namespace String.Handlers
{
    public static class SubSequenceHandler
    {
        public static string GetLongestIncreasingSubSequence(string input)
    }
}
```

**Inputs**: one `string` value containing zero or more integer tokens.

**Output**: one `string` value containing the selected run formatted as integer tokens separated by exactly one ASCII space (`U+0020`).

Evaluator tests follow the pattern in `requirements/reference/unit_test_format.txt` (Given / When / Then with FluentAssertions `Should().Be(...)`).

---

## Supplied Behavioural Rules

The following rules are derived directly from `requirements/code-test.md` and confirmed by the supplied evaluator cases (especially AC-010):

1. **Parse in order** — Read integer tokens from the input string in their original left-to-right order.
2. **Contiguous runs only** — A candidate run consists of adjacent tokens from the parsed sequence; skipping intermediate values is not permitted.
3. **Strict increase** — A token continues the current run only when its numeric value is strictly greater than the immediately preceding token in that run.
4. **Run termination** — A token equal to or less than the immediately preceding token ends the current run; that token may start a new run of length 1.
5. **Longest run wins** — Return the contiguous strictly increasing run with the greatest length.
6. **Earliest tie-break** — When multiple runs share the maximum length, return the one that starts earliest in the input (lowest starting index).
7. **Output formatting** — Return the chosen run as integer tokens separated by exactly one space, with no leading or trailing space.

**Semantic note (supplied by counter-evidence)**: Although the problem statement in `requirements/code-test.md` uses the phrase “longest increasing subsequence”, AC-010 (`6 2 4 6 1 5 9 2` → `2 4 6`, not `2 4 6 9`) proves the evaluator expects **contiguous** strictly increasing runs, not classical non-contiguous longest increasing subsequence.

---

## User Scenarios & Testing

### User Story 1 — Find the longest contiguous increasing run (Priority: P1)

A caller supplies a whitespace-separated list of integers and receives the longest contiguous strictly increasing sub-run, formatted as a string.

**Why this priority**: This is the sole capability the exercise evaluates.

**Independent Test**: Invoke `GetLongestIncreasingSubSequence` with each acceptance criterion input and compare the result to the expected output.

**Acceptance Scenarios**: See **Acceptance Criteria AC-001 through AC-011** below.

---

### Edge Cases

| Scenario | Source | Expected behaviour |
| -------- | ------ | ------------------ |
| Multiple max-length runs | AC-010, AC-011 (supplied) | Return the earliest starting run |
| Equal adjacent values break a run | AC-010 (supplied) | Equal values terminate the current run |
| Single-token input | Derived from FR-005 (supplementary) | Return that single token (run of length 1) |
| Null input | Clarified 2026-08-03 | Throw `ArgumentNullException` |
| Empty string | Clarified 2026-08-03 | Return empty string `""` |
| Non-numeric or unparseable tokens | Clarified 2026-08-03 | Throw an exception on the first invalid token |

---

## Acceptance Criteria

Exact inputs and expected outputs are preserved without modification.

- **Short cases (AC-001, AC-004, AC-010, AC-011)**: reproduced inline below.
- **Large cases (AC-002, AC-003, AC-005, AC-006, AC-007, AC-008, AC-009)**: full single-line inputs and outputs are stored verbatim in `acceptance/ac-NN-input.txt` and `acceptance/ac-NN-expected.txt` within this feature directory (extracted from `requirements/code-test.md`).

### AC-001 (Test case 1)

| Field | Value |
| ----- | ----- |
| **Input** | `6 1 5 9 2` |
| **Expected output** | `1 5 9` |

### AC-002 (Test case 2)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-02-input.txt` |
| **Expected output** | `1710 2461 9288 10195 10431 12485` |

### AC-003 (Test case 3)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-03-input.txt` |
| **Expected output** | `10298 10897 12291 15037 18446 23435 25333 27266` |

### AC-004 (Test case 4)

| Field | Value |
| ----- | ----- |
| **Input** | `923 11613 30483 19569 24201 13461 1189 30793 8848 16914 16053 21700 22116 3852 20909 5231 31469 3862 16353 22813 28735 4421 3618 32303 9932 31892 7823 22547 28888 11143 11695 3339 2094 11023 9661 27440 7186 24750 15427 24502 31606 23515 3563 29553 12145 22184 11409 28824 6636 10658 21404 5578 27807 14073 13967 31310 3132 4321 7643 1951 13289 24375 17912 11304` |
| **Expected output** | `3862 16353 22813 28735` |

### AC-005 (Test case 5)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-05-input.txt` |
| **Expected output** | `11084 11970 24975 30922` |

### AC-006 (Test case 6)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-06-input.txt` |
| **Expected output** | `3808 3908 10386 19306` |

### AC-007 (Test case 7)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-07-input.txt` |
| **Expected output** | `125 1841 5882 18464 28317 31497` |

### AC-008 (Test case 8)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-08-input.txt` |
| **Expected output** | `9139 17687 25106 26202 27592 30937` |

### AC-009 (Test case 9)

| Field | Value |
| ----- | ----- |
| **Input** | See `acceptance/ac-09-input.txt` |
| **Expected output** | `918 1089 5133 7725 18035 24605 26716 27095` |

### AC-010 (Test case 10)

| Field | Value |
| ----- | ----- |
| **Input** | `6 2 4 6 1 5 9 2` |
| **Expected output** | `2 4 6` |

### AC-011 (Test case 11)

| Field | Value |
| ----- | ----- |
| **Input** | `6 2 4 3 1 5 9` |
| **Expected output** | `1 5 9` |

---

## Requirements

### Functional Requirements *(supplied)*

- **FR-001**: The function MUST accept one string input containing integer tokens separated by whitespace.
- **FR-002**: The function MUST parse integer tokens in their original left-to-right order.
- **FR-003**: The function MUST identify contiguous strictly increasing runs within the parsed sequence.
- **FR-004**: The function MUST treat a token equal to or less than the immediately preceding token as terminating the current run.
- **FR-005**: The function MUST return the longest contiguous strictly increasing run found in the input.
- **FR-006**: When multiple runs share the maximum length, the function MUST return the run that starts earliest in the input.
- **FR-007**: The function MUST return the selected run as tokens separated by exactly one ASCII space.
- **FR-008**: The function MUST produce the expected output for every acceptance criterion AC-001 through AC-011.

### Functional Requirements *(supplementary assumptions)*

- **FR-S01**: Whitespace between tokens is assumed to be the ASCII space character (`U+0020`), matching all supplied evaluator inputs. The problem statement in `requirements/code-test.md` refers to “single whitespace”; no supplied case uses tabs or multiple consecutive spaces.
- **FR-S02**: Integer tokens are assumed to be base-10 representations that fit the range of a standard signed 32-bit integer, matching the numeric magnitudes present in the supplied cases.
- **FR-S03**: The function is assumed to be deterministic and to depend only on the input string (no hidden state or external inputs).

### Functional Requirements *(clarified — invalid or boundary input)*

The authoritative sources do not define behaviour for invalid or boundary inputs. The following were clarified on 2026-08-03:

- **FR-C01**: When `input` is `null`, the function MUST throw `ArgumentNullException`.
- **FR-C02**: When `input` is an empty string (`""`), the function MUST return an empty string (`""`).
- **FR-C03**: When `input` contains a token that is not a valid base-10 integer (including empty tokens produced by repeated spaces or overflow values), the function MUST throw an exception on the first such token and MUST NOT return a partial result.

---

## Key Entities

- **Input string**: An ordered sequence of integer tokens separated by whitespace.
- **Contiguous run**: A maximal-length slice of adjacent tokens where each token is strictly greater than the one before it within that slice.
- **Result string**: The selected run encoded as integer tokens separated by exactly one space.

---

## Success Criteria

### Measurable Outcomes

- **SC-001**: For each of AC-001 through AC-011, the function output matches the expected output exactly (string equality).
- **SC-002**: AC-010 passes, confirming contiguous-run semantics rather than non-contiguous subsequence selection.
- **SC-003**: When two maximum-length runs exist in the same input, the returned run is the one with the earlier start index (verified by AC-010 and AC-011).
- **SC-004**: All returned values use single-space delimiters with no leading or trailing spaces (verified across all 11 acceptance criteria).
- **SC-005**: Null input throws `ArgumentNullException`; empty input returns `""`; first invalid token throws an exception (verified by unit tests covering FR-C01 through FR-C03).

---

## Assumptions

### Supplied (from authoritative sources)

- Evaluator correctness is defined by the 11 cases in `requirements/code-test.md`.
- Test invocation shape matches `requirements/reference/unit_test_format.txt` (`SubSequenceHandler.GetLongestIncreasingSubSequence`, FluentAssertions equality check).
- Tie-breaking favours the earliest maximum-length run when lengths are equal.

### Supplementary (not stated in authoritative sources)

- Token separator is ASCII space, as used in every supplied input.
- All numeric values in supplied tests are valid signed 32-bit integers.
- Supporting helpers may exist, but their design is out of scope for this functional specification.
- Performance, packaging, CI, Docker, and linting mentioned in `requirements/code-test.md` are exercise delivery objectives, not behavioural requirements of the core function.

---

## Traceability

| Acceptance criterion | Source test case | Fixture input | Fixture expected |
| -------------------- | ---------------- | ------------- | ---------------- |
| AC-001 | Test case 1 | `acceptance/ac-01-input.txt` | `acceptance/ac-01-expected.txt` |
| AC-002 | Test case 2 | `acceptance/ac-02-input.txt` | `acceptance/ac-02-expected.txt` |
| AC-003 | Test case 3 | `acceptance/ac-03-input.txt` | `acceptance/ac-03-expected.txt` |
| AC-004 | Test case 4 | `acceptance/ac-04-input.txt` | `acceptance/ac-04-expected.txt` |
| AC-005 | Test case 5 | `acceptance/ac-05-input.txt` | `acceptance/ac-05-expected.txt` |
| AC-006 | Test case 6 | `acceptance/ac-06-input.txt` | `acceptance/ac-06-expected.txt` |
| AC-007 | Test case 7 | `acceptance/ac-07-input.txt` | `acceptance/ac-07-expected.txt` |
| AC-008 | Test case 8 | `acceptance/ac-08-input.txt` | `acceptance/ac-08-expected.txt` |
| AC-009 | Test case 9 | `acceptance/ac-09-input.txt` | `acceptance/ac-09-expected.txt` |
| AC-010 | Test case 10 | `acceptance/ac-10-input.txt` | `acceptance/ac-10-expected.txt` |
| AC-011 | Test case 11 | `acceptance/ac-11-input.txt` | `acceptance/ac-11-expected.txt` |
