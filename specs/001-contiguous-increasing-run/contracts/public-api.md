# Public API Contract

**Feature**: `001-contiguous-increasing-run`  
**Version**: 1.0.0  
**Target**: .NET 8 (`net8.0`)

---

## Production entry point

**Assembly**: `CodingTest`  
**Namespace**: `CodingTest.Handlers`

```csharp
namespace CodingTest.Handlers;

/// <summary>
/// Finds the longest contiguous strictly increasing run in a whitespace-separated integer string.
/// </summary>
public static class SubSequenceHandler
{
    /// <summary>
    /// Returns the longest contiguous strictly increasing run, formatted with single-space delimiters.
    /// When multiple runs share the maximum length, the earliest run is returned.
    /// </summary>
    /// <param name="input">Whitespace-separated base-10 integers.</param>
    /// <returns>Selected run as a space-delimited string, or empty string when input is empty.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="input"/> is null.</exception>
    /// <exception cref="FormatException">When a token is not a valid base-10 integer.</exception>
    /// <exception cref="OverflowException">When a token exceeds the signed 32-bit range.</exception>
    public static string GetLongestIncreasingSubSequence(string input);
}
```

---

## Behavioural contract

| ID | Requirement |
| -- | ----------- |
| API-001 | Parse tokens left-to-right in original order |
| API-002 | Continue run only when current value **>** previous value |
| API-003 | Terminate run on equal or smaller value |
| API-004 | Return longest contiguous strictly increasing run |
| API-005 | On length tie, return earliest run (do not replace best when lengths equal) |
| API-006 | Format output with exactly one ASCII space between tokens |
| API-007 | Null input → `ArgumentNullException` |
| API-008 | Empty input → `""` |
| API-009 | Invalid token → `FormatException` (first failure) |
| API-010 | Overflow token → `OverflowException` (first failure) |

---

## Evaluator test surface

**Test assembly**: `CodingTestUnitTests`  
**Supplied test class**: `SuppliedEvaluatorTests` (or `MainBusinessLogicUnitTests` per reference naming — prefer `SuppliedEvaluatorTests` for clarity)

```csharp
[Trait("Category", "Unit_Tests")]
public class SuppliedEvaluatorTests
{
    [Fact]
    public void Test_Case_01()
    {
        // Given
        string input = /* from AC-001 fixture */;
        string expectedSubSequence = /* from AC-001 fixture */;

        // When
        string output = SubSequenceHandler.GetLongestIncreasingSubSequence(input);

        // Then
        output.Should().Be(expectedSubSequence);
    }

    // Test_Case_02 … Test_Case_11 likewise
}
```

**Namespace import**: `using CodingTest.Handlers;`  
**Assertion library**: FluentAssertions (`output.Should().Be(expectedSubSequence)`)

---

## CLI contract (verification)

**Assembly**: `CodingTestCli`

| Input channel | Behaviour |
| ------------- | --------- |
| Command-line argument(s) | Join args with space OR treat first arg as full input string (document in README) |
| Stdin | Read full line when no args |

| Exit code | Meaning |
| --------- | ------- |
| 0 | Success; result written to stdout |
| non-zero | Unhandled exception / invalid usage |

Stdout contains exactly the handler return value (no extra labels).

---

## Namespace divergence (documented)

| Source | Namespace |
| ------ | --------- |
| Functional spec (`spec.md`) | `String.Handlers` |
| **Implementation (this contract)** | **`CodingTest.Handlers`** |

Implementation and tests MUST use `CodingTest.Handlers` per approved plan.

---

## Compatibility matrix

| Consumer | Depends on |
| -------- | ---------- |
| `CodingTestUnitTests` | `CodingTest` library |
| `CodingTestCli` | `CodingTest` library |
| Docker image | Published `CodingTestCli` |
| External evaluator | Method signature + behaviour (namespace must match deployed assembly) |

---

## Non-goals (not part of public contract)

- HTTP endpoints
- Async overloads
- Dependency injection registration
- Configuration files for algorithm tuning
