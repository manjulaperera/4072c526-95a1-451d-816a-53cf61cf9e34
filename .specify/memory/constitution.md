<!--
Sync Impact Report
==================
Version change: (none) → 1.0.0
Modified principles: Initial adoption — all principles newly defined
Added sections:
  - Core Principles (11 principles)
  - Additional Constraints — Modern .NET Quality Controls
  - Development Workflow — CI, Completion Criteria, AI-Assisted Development, Repository Anonymity
  - Governance
Removed sections: None (initial ratification from template)
Deferred TODOs: None
Follow-up templates: No dependent template updates required by this command
-->

# Coding Exercise Constitution

## Core Principles

### I. Authoritative Behavioural Contract

The supplied requirements and all 11 supplied evaluator test cases are the authoritative
behavioural contract for this project. All design, implementation, and verification
decisions MUST align with these artefacts unless an approved specification change
explicitly requires otherwise.

**Rationale**: The evaluator tests define what "correct" means for this exercise; deviating
without an approved spec change breaks compatibility and invalidates the submission.

### II. Specification-First Development

All observable behaviour MUST be defined before production code is written. Assumptions,
ambiguities, and supplementary behaviour MUST be clearly documented. Evaluator
compatibility MUST be preserved unless an approved specification change explicitly
requires otherwise.

**Rationale**: Defining behaviour upfront prevents rework, clarifies edge cases, and
ensures the implementation matches the contract before code is committed.

### III. Test-First Development (NON-NEGOTIABLE)

Development MUST follow test-first practices:

- Use xUnit and FluentAssertions.
- Add the 11 supplied evaluator cases before implementing business logic.
- Use clear, human-readable test names that describe behaviour and expected outcomes.
- Structure tests using Given, When, and Then sections where appropriate.
- Keep supplied acceptance tests separate from supplementary edge-case tests.
- Ensure all documented production behaviour is covered by meaningful automated tests.

**Rationale**: Tests encode the contract; writing them first enforces the specification
and provides immediate feedback during implementation.

### IV. Clean, Idiomatic C#

Production code MUST be clean, idiomatic, and appropriately simple C#:

- Prefer readability and maintainability over clever or compressed code.
- Follow current C# and .NET best practices.
- Use meaningful names for classes, methods, and variables.
- Add short comments only where they clarify non-obvious business rules or important
  decisions.
- Avoid comments that merely repeat what the code already expresses.
- Add XML documentation only to public APIs where it improves clarity for callers.

**Rationale**: Readable code is easier to review, test, and maintain — especially in a
coding exercise evaluated for clarity of thought.

### V. Evaluator-Compatible Public Contract

The evaluator-compatible public contract defined by the supplied tests MUST be preserved,
including the required namespace, class name, and method signature. Breaking changes to
this contract are forbidden without an approved specification change.

**Rationale**: The evaluator invokes a fixed entry point; altering the public contract
causes automated evaluation to fail regardless of internal correctness.

### VI. Contiguous Strictly Increasing Subsequence Semantics

The term "subsequence" MUST be interpreted as a **contiguous strictly increasing run**
because this behaviour is demonstrated by the supplied expected outputs. This is not the
classical dynamic-programming longest increasing subsequence over non-contiguous elements.

**Rationale**: The supplied test outputs (e.g., `6 1 5 9 2` → `1 5 9`, not a longer
non-contiguous subsequence) establish the intended algorithm semantics.

### VII. Sequence Evaluation Rules

The sequence evaluation algorithm MUST apply these rules:

- A value continues the current run only when it is strictly greater than the
  immediately preceding value.
- Equal or smaller values terminate the current run.
- When multiple maximum-length runs exist, return the earliest run.
- Preserve the earliest run by replacing the current best only when a strictly longer
  run is found.

**Rationale**: These rules are derived from the problem statement and confirmed by the
11 evaluator test cases; they define the core business logic.

### VIII. Single-Pass O(n) Algorithm

Sequence evaluation MUST use a single-pass O(n) algorithm. Multi-pass or super-linear
approaches are not permitted when a single linear scan satisfies the specification.

**Rationale**: The input may contain large sequences (see evaluator test case 2); a
single linear pass is sufficient, efficient, and appropriately simple.

### IX. Separation of Responsibilities

The solution MUST maintain clear separation of responsibilities:

- Input parsing and validation.
- Sequence evaluation.
- Output formatting or presentation.

Business logic MUST NOT be duplicated across projects or entry points.

**Rationale**: Separating concerns improves testability and readability without
introducing unnecessary architectural layers.

### X. Invalid-Input Behaviour Documentation

Invalid-input behaviour MUST be explicitly documented, including handling of:

- null input;
- empty or whitespace-only input;
- malformed tokens;
- values outside the supported integer range;
- leading, trailing, or repeated whitespace.

Documented behaviour MUST be covered by supplementary automated tests.

**Rationale**: Input validation is part of the observable contract; undocumented or
untested edge cases create ambiguity and evaluation risk.

### XI. Simplicity Over Architecture

Unnecessary complexity MUST be avoided:

- Do not introduce dependency injection, repositories, CQRS, MediatR, databases, web
  APIs, or architectural layers unless they provide a clear requirement-driven benefit.
- Prefer the simplest design that fully satisfies the specification and tests.
- Do not create abstractions for hypothetical future requirements.

**Rationale**: This is a focused coding exercise; simplicity demonstrates judgment and
keeps the solution easy to verify.

## Additional Constraints

### Modern .NET Quality Controls

The solution MUST enable modern .NET quality controls:

- Use .NET 8 or the approved target framework.
- Enable nullable reference types.
- Enable implicit usings where appropriate.
- Treat compiler warnings as errors.
- Enable current .NET analyzers.
- Enforce code style and formatting during the build.
- Use deterministic builds where appropriate.

**Rationale**: These settings catch defects early and enforce consistent, professional
code quality throughout development.

## Development Workflow

### Continuous Integration

Establish continuous integration immediately after the solution skeleton is created:

- Add GitHub Actions once the solution and test project exist.
- Run restore, formatting verification, build, and tests for every push and pull
  request.
- Keep the pipeline passing throughout development.
- Add coverage collection and Docker validation after the primary functionality is
  complete.

**Rationale**: CI provides continuous verification and demonstrates professional delivery
practices expected by the exercise secondary objectives.

### Completion Criteria

The completed solution MUST:

- build and run locally;
- pass all 11 supplied evaluator tests;
- pass all supplementary tests;
- build and test successfully through GitHub Actions;
- provide code coverage reporting;
- be executable through Docker;
- include clear verification instructions in the README.

**Rationale**: These criteria define "done" for the exercise and align with both primary
and secondary objectives in the requirements.

### Controlled AI-Assisted Development

AI tools MAY be used for requirement analysis, test generation, implementation
suggestions, review, and documentation support. However:

- Do not accept AI-generated changes automatically.
- Review every generated change before acceptance.
- Validate changes through compilation, automated tests, formatting, static analysis,
  and manual inspection.
- Keep the specification, tests, and source-controlled code as the source of truth.
- Record significant AI-assisted decisions and verification steps in the project
  documentation.

**Rationale**: AI accelerates development but human review ensures correctness,
compliance, and accountability.

### Repository Anonymity

The organisation name, company name, or other identifying details MUST NOT appear
anywhere in source code, namespaces, documentation, repository metadata, workflow names,
Docker metadata, or commit messages.

**Rationale**: The exercise requires an anonymous public repository identified only by
UUID; identifying details violate submission requirements.

## Governance

This constitution supersedes ad-hoc practices for this project. All design, implementation,
and review decisions MUST comply with these principles.

**Amendment procedure**:

1. Propose the change with rationale and impact on evaluator compatibility.
2. Update the specification if the change affects observable behaviour.
3. Update tests to reflect approved behaviour changes before changing production code.
4. Increment the constitution version according to semantic versioning:
   - **MAJOR**: Backward-incompatible governance or principle removals/redefinitions.
   - **MINOR**: New principles or materially expanded guidance.
   - **PATCH**: Clarifications, wording, or non-semantic refinements.
5. Record the amendment date and sync impact in the constitution header comment.

**Compliance review**: Every change MUST be verified against this constitution through
automated tests, CI pipeline results, and manual review before acceptance.

**Runtime guidance**: Feature specifications, plans, and tasks derived from this
constitution live under `.specify/` and `requirements/`. When in doubt, the requirements
document and 11 evaluator test cases take precedence for behavioural decisions.

**Version**: 1.0.0 | **Ratified**: 2026-08-03 | **Last Amended**: 2026-08-03
