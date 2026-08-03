# AI-Assisted Development

## Tools

- Cursor
- GitHub Spec Kit
- GitHub Actions
- .NET CLI

## Workflow

1. Extracted the public contract and 11 acceptance cases from the supplied files.
2. Created a functional specification before implementation.
3. Clarified unspecified input behaviour.
4. Created a technical plan and task sequence.
5. Added the 11 supplied evaluator tests before production code.
6. Implemented the minimum O(n) solution.
7. Added supplementary edge-case tests.
8. Validated formatting, build, tests, coverage, Docker and CI.

## Important Requirement Decision

The challenge uses the term "subsequence", but the supplied expected outputs
demonstrate a contiguous strictly increasing run.

## AI Controls

Cursor was used for:

- requirement analysis;
- specification review;
- test generation;
- implementation suggestions;
- code review;
- CI and Docker review.

AI-generated changes were manually reviewed and validated through:

- compilation;
- automated tests;
- formatting;
- static analysis;
- code coverage;
- Docker execution;
- GitHub Actions.

## Tie-Breaking Rule

The implementation updates the best sequence only when the current sequence is
strictly longer. Equal-length later sequences do not replace the earlier result.