# Coding Exercise

[![Build and Test](https://github.com/manjulaperera/4072c526-95a1-451d-816a-53cf61cf9e34/actions/workflows/ci.yml/badge.svg)](https://github.com/manjulaperera/4072c526-95a1-451d-816a-53cf61cf9e34/actions/workflows/ci.yml)

## Problem
Develop a function that takes one string input of any number of integers separated by single whitespace. The function then outputs the longest increasing subsequence (increased by any number) present in that sequence. If more than 1 sequence exists with the longest length, output the earliest one. You may develop supporting functions as many as you find reasonable. Your function should pass the test cases provided below.

## Solution
Finds the longest **contiguous strictly increasing run** in a whitespace-separated integer string. When multiple runs share the maximum length, the earliest run is returned.

The solution parses the whitespace-separated input into integers and scans the values once while tracking both the current contiguous increasing sequence and the longest sequence found so far. A sequence continues only when the current value is strictly greater than the previous value; otherwise, a new sequence begins at the current position. The longest result is updated only when a strictly longer sequence is found, which ensures that the earliest sequence is retained when multiple sequences have the same maximum length. The selected values are then returned as a single-space-separated string. This approach has O(n) time complexity and uses only constant additional tracking state apart from the parsed input and returned result.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- (Optional) [Docker](https://www.docker.com/) for container verification
- Git

## Build

From the repository root:

```bash
dotnet restore CodingTest.sln
dotnet build CodingTest.sln --configuration Release
```

## Format check

```bash
dotnet format CodingTest.sln --verify-no-changes
```

## Test

All tests:

```bash
dotnet test CodingTest.sln --configuration Release
```

Supplied evaluator tests only (11 cases):

```bash
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --configuration Release --filter "Category=Unit_Tests"
```

Coverage collection:

```bash
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj --configuration Release --collect:"XPlat Code Coverage"   --results-directory ./TestResults
```

## CLI

Command-line arguments take precedence over stdin. When arguments are present, they are joined with a single ASCII space. When no arguments are provided, one line is read from stdin.

```bash
dotnet run --project src/CodingTestCli/CodingTestCli.csproj -- "6 1 5 9 2"
```

Expected output: `1 5 9`

Stdin example:

```bash
echo "6 1 5 9 2" | dotnet run --project src/CodingTestCli/CodingTestCli.csproj --no-build
```

## Docker

Build from the repository root:

```bash
docker build -f src/CodingTestCli/Dockerfile -t coding-test-cli .
```

Run:

```bash
docker run --rm coding-test-cli "6 1 5 9 2"
```

Expected output: `1 5 9`

## Continuous integration

GitHub Actions runs on every push and pull request:

1. Restore dependencies
2. Verify formatting (`dotnet format --verify-no-changes`)
3. Release build
4. Test with code coverage collection
5. Docker build and smoke test

Workflow file: [`.github/workflows/ci.yml`](.github/workflows/ci.yml)

## AI-assisted development log

Significant AI-assisted decisions and verification steps are recorded in [`docs/ai-development.md`](docs/ai-development.md) and [`docs/ai-development-log.md`](docs/ai-development-log.md).

## Public API

```csharp
CodingTest.Handlers.SubSequenceHandler.GetLongestIncreasingSubSequence(string input)
```

See [`specs/001-contiguous-increasing-run/contracts/public-api.md`](specs/001-contiguous-increasing-run/contracts/public-api.md) for the full behavioural contract.
