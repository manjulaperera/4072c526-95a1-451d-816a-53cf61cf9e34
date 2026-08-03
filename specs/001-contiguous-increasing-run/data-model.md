# Data Model: Longest Contiguous Strictly Increasing Run

**Feature**: `001-contiguous-increasing-run`  
**Date**: 2026-08-03

Logical data shapes used across parsing, evaluation, and formatting. No persistent storage.

---

## Entities

### InputString

| Attribute | Type | Rules |
| --------- | ---- | ----- |
| Raw value | `string` | May be null, empty, or whitespace-separated tokens |
| Token separator | ASCII space `U+0020` | Assumed for all supplied evaluator cases |

**Validation outcomes**:

| Condition | Result |
| --------- | ------ |
| `null` | Throw `ArgumentNullException` (FR-C01) |
| `""` | Empty sequence → output `""` (FR-C02) |
| Invalid token | Throw `FormatException` at first failure (FR-C03) |
| Overflow token | Throw `OverflowException` at first failure (FR-C03) |

---

### IntegerToken

| Attribute | Type | Rules |
| --------- | ---- | ----- |
| Index | `int` | Zero-based position in parse order |
| Value | `int` | Signed 32-bit; strictly compared to predecessor for run continuation |

**Relationships**: Ordered sequence `IntegerToken[0..n-1]` derived from `InputString`.

---

### ContiguousRun

| Attribute | Type | Rules |
| --------- | ---- | ----- |
| StartIndex | `int` | Inclusive start in token sequence |
| Length | `int` | ≥ 1 when sequence non-empty |
| Values | `ReadOnlyMemory<int>` or slice | Contiguous subsequence of tokens |

**Invariants**:

- For all `j` in `1..Length-1`: `Values[j] > Values[j-1]` (strict increase).
- Run is maximal within its slice until broken by `<=` predecessor relationship.

---

### RunTrackerState (evaluation transient)

| Field | Type | Purpose |
| ----- | ---- | ------- |
| `currentStart` | `int` | Start index of active run |
| `currentLength` | `int` | Length of active run |
| `bestStart` | `int` | Start index of best run found so far |
| `bestLength` | `int` | Length of best run found so far |

**State transitions** (per token at index `i`):

```
if i == 0 OR values[i] > values[i-1]:
    extend or start current run
else:
    currentStart ← i; currentLength ← 1

if currentLength > bestLength:
    bestStart ← currentStart; bestLength ← currentLength
```

**Tie-break invariant**: When `currentLength == bestLength`, `bestStart` is unchanged → earliest run retained (FR-006).

---

### ResultString

| Attribute | Type | Rules |
| --------- | ---- | ----- |
| Formatted value | `string` | Tokens joined with exactly one ASCII space |
| Leading/trailing space | forbidden | FR-007 |
| Empty result | `""` | When input sequence empty (FR-C02) |

---

## Entity Relationship (logical)

```text
InputString
    │ parse (IntegerSequenceParser)
    ▼
IntegerToken[0..n-1]
    │ evaluate (ContiguousRunEvaluator)
    ▼
ContiguousRun (best)
    │ format (RunFormatter)
    ▼
ResultString
```

---

## Mapping to acceptance criteria

| Entity / rule | Acceptance criteria |
| ------------- | ------------------- |
| Strict increase | AC-001, AC-010, AC-011 |
| Earliest tie-break | AC-010, AC-011 |
| Large sequences | AC-002, AC-003, AC-009 |
| Output formatting | AC-001…AC-011 |
| Null / empty / invalid | FR-C01…FR-C03 (supplementary tests) |

---

## Out of scope

- Database entities
- DTOs for network transport
- Configuration models
- User identity or session state
