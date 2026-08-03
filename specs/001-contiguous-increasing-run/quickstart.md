# Quickstart: Verification Guide

**Feature**: `001-contiguous-increasing-run`  
**Date**: 2026-08-03

Runnable steps to verify the solution locally, in CI, and via Docker. Assumes .NET 8 SDK installed.

For behavioural rules and acceptance criteria see [spec.md](./spec.md). For API details see [contracts/public-api.md](./contracts/public-api.md).

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- (Optional) Docker Desktop or compatible engine
- Git

---

## 1. Restore and build

From repository root:

```bash
dotnet restore CodingTest.sln
dotnet build CodingTest.sln --configuration Release
```

**Expected**: Build succeeds with zero warnings (warnings treated as errors).

---

## 2. Format check

```bash
dotnet format CodingTest.sln --verify-no-changes
```

**Expected**: No formatting diffs reported.

---

## 3. Run tests

### All tests

```bash
dotnet test CodingTest.sln --configuration Release --no-build
```

**Expected**:

- All 11 supplied evaluator tests pass (`Test_Case_One` … `Test_Case_Eleven`).
- Supplementary invalid-input tests pass.

### Supplied evaluator tests only

```bash
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj \
  --configuration Release \
  --filter "Category=Unit_Tests"
```

**Expected**: 11 passed, 0 failed.

---

## 4. Manual smoke test (CLI)

After implementation:

```bash
dotnet run --project src/CodingTestCli/CodingTestCli.csproj -- "6 1 5 9 2"
```

**Expected stdout**:

```text
1 5 9
```

Additional spot checks:

| Input | Expected output |
| ----- | --------------- |
| `6 2 4 6 1 5 9 2` | `2 4 6` |
| `6 2 4 3 1 5 9` | `1 5 9` |

---

## 5. Coverage (after Phase F)

```bash
dotnet test tests/CodingTestUnitTests/CodingTestUnitTests.csproj \
  --configuration Release \
  --settings coverlet.runsettings \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

**Expected**: OpenCover report under `TestResults/**/coverage.opencover.xml`; CI uploads the same format and publishes to SonarCloud when `SONAR_ENABLED` is `true`.

---

## 5b. SonarCloud

The SonarCloud project **must be created first** on [sonarcloud.io](https://sonarcloud.io) (*Analyze new project* → this repo). Copy the assigned **Project key** into the GitHub `SONAR_PROJECT_KEY` variable. Full steps: [README.md § SonarCloud](../../README.md#sonarcloud).

**Expected**: After the first successful CI run with `SONAR_ENABLED=true`, coverage appears on your SonarCloud project dashboard (URL is shown in SonarCloud after import — it depends on your organization and project key).

---

## 6. Docker (after Phase F)

Build from repository root:

```bash
docker build -f src/CodingTestCli/Dockerfile -t coding-test-cli .
```

Run:

```bash
docker run --rm coding-test-cli "6 1 5 9 2"
```

**Expected stdout**: `1 5 9`

---

## 7. GitHub Actions

Push to remote; open Actions tab.

**Expected pipeline stages**:

1. Restore
2. Format verification
3. Release build
4. Test (11/11 supplied + supplementary)
5. (Later) Coverage upload
6. (Later) Docker build

---

## 8. Acceptance fixture reference

Large evaluator inputs/outputs are stored verbatim:

```text
specs/001-contiguous-increasing-run/acceptance/
├── ac-01-input.txt / ac-01-expected.txt
├── …
└── ac-11-input.txt / ac-11-expected.txt
```

Tests MUST preserve exact strings when copied to embedded resources.

---

## 9. AI development log

Record significant AI-assisted decisions in:

```text
docs/ai-development-log.md
```

Each entry should include date, change summary, verification command, and outcome.

---

## 10. README checklist

Ensure root `README.md` includes:

- [x] Problem summary (contiguous strictly increasing run)
- [x] Prerequisites (.NET 8, Docker optional)
- [x] Build commands
- [x] Test commands (all + supplied-only filter)
- [x] CLI usage example
- [x] Docker build/run example
- [ ] CI badge or link (optional)
- [x] Note on AI-assisted development log location

---

## Troubleshooting

| Symptom | Likely cause | Action |
| ------- | ------------ | ------ |
| AC-010 fails with longer subsequence | Non-contiguous LIS implemented | Use contiguous O(n) scan; see [plan.md](./plan.md) algorithm |
| Tie-break wrong | Using `>=` to update best | Update best only when `currentLength > bestLength` |
| Large test mismatch | Fixture altered | Diff test resource against `acceptance/ac-NN-*.txt` |
| Format CI failure | Missing `.editorconfig` | Run `dotnet format` locally and commit |

---

## Definition of done

- [x] `dotnet build` / `dotnet test` pass locally
- [x] `dotnet format --verify-no-changes` pass
- [x] 11 supplied evaluator tests pass
- [x] Supplementary FR-C01–FR-C03 tests pass
- [x] GitHub Actions green
- [x] Coverage reporting enabled
- [x] Docker CLI runs successfully
- [x] README verification section complete
- [x] AI development log maintained
