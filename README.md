# Coding Exercise

## Problem
Develop a function that takes one string input of any number of integers separated by single whitespace. The function then outputs the longest increasing subsequence (increased by any number) present in that sequence. If more than 1 sequence exists with the longest length, output the earliest one. You may develop supporting functions as many as you find reasonable. Your function should pass the test cases provided below.

Finds the longest **contiguous strictly increasing run** in a whitespace-separated integer string.

## CLI

Command-line arguments take precedence over stdin. When arguments are present, they are joined with a single ASCII space to form the input string. When no arguments are provided, one line is read from stdin.

```bash
dotnet run --project src/CodingTestCli/CodingTestCli.csproj -- "6 1 5 9 2"
```

Expected output: `1 5 9`
