# CLAUDE.md

Guidance for Claude Code agents working in this repository.

## Overview

Official Kontent.ai **Management SDK for .NET**. This repository is undergoing a ground-up modernization that mirrors the architecture already shipped in two sibling SDKs:

- `../delivery-sdk-net` — the primary modernization reference.
- `../sync-sdk-net` — the secondary reference, same patterns at smaller scale.

The current code still reflects the legacy architecture (hand-rolled HTTP, partial `ManagementClient`, Newtonsoft.Json, no Abstractions split, no DI extensions). Do not treat the existing shape as authoritative when designing new code — it will be replaced. The sections below describe where the SDK is going.

## Modernization mandate

- **This is a major rewrite. Breaking changes are expected.** Do not add backward-compat shims, do not keep legacy code paths "just in case", do not preserve obsolete types beyond their natural lifetime.
- **However, breaking changes on the public API surface must be justified.** Renaming a stable, sensible type or method purely to "modernize naming" is not justification. The bar is: does the change enable a clearly better architecture, fix a real defect, or remove real friction? If not, leave it alone. Familiarity has value.
- **The SDK will be built on top of [Refit](https://github.com/reactiveui/refit).** Hand-rolled HTTP composition (`ActionInvoker`, `ManagementHttpClient`, `EndpointUrlBuilder`) goes away. The Refit interface is the transport layer; everything public wraps it.
- **Model generation is coordinated.** `../model-generator-net` currently emits Delivery models only (v19+ records). A paired update there will re-introduce Management-model generation. Request/response model shapes in this SDK must stay friendly to that generator (record-based, immutable, `System.Text.Json`-serializable).

## Sibling repos are canonical references

`../delivery-sdk-net` and `../sync-sdk-net` sit next to this repo in the filesystem. You may read anything in them freely — source, tests, build files, `CLAUDE.md` — to answer architecture, style, or convention questions. When a design question comes up, **read the sibling first** and only diverge with a stated reason.

Concrete anchors to consult:

| Concern                          | Canonical file(s)                                                                  |
|----------------------------------|------------------------------------------------------------------------------------|
| Refit interface layout           | `../delivery-sdk-net/Kontent.Ai.Delivery/Api/IDeliveryApi.*.cs` (partial per-domain) |
| DI extensions + keyed clients    | `../delivery-sdk-net/Kontent.Ai.Delivery/Extensions/ServiceCollectionExtensions*.cs` |
| Fluent non-DI builder            | `../delivery-sdk-net/Kontent.Ai.Delivery/DeliveryClientBuilder.cs`, `../sync-sdk-net/Kontent.Ai.Sync/Configuration/SyncClientBuilder.cs` |
| Refit settings + `System.Text.Json` | `ServiceCollectionExtensions.HttpClient.cs` in delivery-sdk-net (`SystemTextJsonContentSerializer`) |
| Resilience pipeline, auth handler | `../sync-sdk-net/Kontent.Ai.Sync/Handlers/`, its `ServiceCollectionExtensions.cs`   |
| Abstractions split               | `../delivery-sdk-net/Kontent.Ai.Delivery.Abstractions/`                            |
| Verify-based API approval tests  | `../delivery-sdk-net/Kontent.Ai.Delivery.Abstractions.Tests/`                       |
| Build infra                      | `Directory.Build.props`, `Directory.Packages.props`, `global.json` in either sibling |

If a sibling's convention and your instinct disagree, defer to the sibling unless you can articulate why this SDK genuinely needs to differ.

## Target framework and language

- **.NET 8 only.** No multi-targeting. No `netstandard`.
- Centralize `LangVersion=latest`, `Nullable=enable`, `ImplicitUsings=enable`, analyzers, and deterministic-build settings in a root `Directory.Build.props` — not per-project.
- Central Package Management via `Directory.Packages.props`. SDK pinned via `global.json`.
- **Use modern C# actively**, not grudgingly: primary constructors, file-scoped namespaces, `sealed` by default on non-abstract types, `record` / `readonly record struct` for DTOs and value objects, `required` members, collection expressions, `init`-only setters, `ArgumentNullException.ThrowIfNull`.
- **Prefer pattern matching** over `if`/cast chains: switch expressions for mapping, property patterns for shape checks, `is not null`, list patterns where they apply. Reach for `if` only when pattern matching would be strained.
- Serialization is `System.Text.Json` (matching the siblings' `SystemTextJsonContentSerializer` Refit configuration). Newtonsoft is out.

## Target architecture

- **Refit-backed HTTP.** Public client wraps an internal `IManagementApi` Refit interface, split into partial files per domain (`IManagementApi.Asset.cs`, `IManagementApi.ContentItem.cs`, `IManagementApi.Language.cs`, …), mirroring `IDeliveryApi.*.cs`.
- **Abstractions project split.** Public contracts (`IManagementClient`, options, result types, model interfaces) live in `Kontent.Ai.Management.Abstractions` with no dependencies beyond the BCL. Implementation lives in `Kontent.Ai.Management`.
- **Two entry points, same as the siblings:**
  - `ManagementClientBuilder` — fluent, no-DI bootstrap for scripts and simple consumers.
  - `services.AddManagementClient(...)` — DI extension with keyed-services support for multiple named clients.
- **Result pattern for API calls.** Mirror `IDeliveryResult<T>` (status / success flag / value / request URL) rather than throwing on 4xx/5xx. Use exceptions only for programmer errors and unrecoverable failures.
- **Resilience via `Microsoft.Extensions.Http.Resilience`** (Polly pipelines). Authentication via a `DelegatingHandler` that attaches the Management API key. Copy the pipeline shape from `sync-sdk-net` rather than inventing one.
- **Testing.** xUnit + `RichardSzalay.MockHttp` for HTTP, Verify for public-API approval snapshots. JSON fixtures stay for request/response bodies.

## Coding and commenting standards

- **Good code is self-explanatory. Do not document every bit of code.** XML doc comments belong on the public Abstractions surface so consumers get IntelliSense. Private implementation does not need method-by-method narration.
- **Only comment the non-obvious.** A hidden API constraint, a subtle invariant, a workaround for a specific server-side quirk, a decision whose rationale would surprise a future reader. If removing the comment would not confuse anyone, do not write it.
- **Do not annotate corrections.** When the user corrects your approach, fix the code and move on. Do not leave comments like `// changed from X because Y` or `// previously used Z` — that history belongs in the commit message at most, usually nowhere.
- **Do not add defensive code for impossible scenarios.** Trust framework and internal guarantees. Validate only at external boundaries (public API entry points, deserialized payloads from the network).
- **No referencing of development markdown files in the comments.** There will be many markdown files with plans, notes and other meta information for development purposes. Those are not going to be commited and must not be referenced by any code comments.
- **No dead code, no `// TODO` drifting across PRs, no commented-out blocks.**
- **Prefer pattern matching.** See above.

## Collaboration stance

- **Be critical, not compliant.** If a request conflicts with the modernization principles above, the sibling SDKs' conventions, or good design in general, push back with reasoning **before** implementing. "The user asked for it" is not sufficient justification for a poor design.
- **No flattery.** Do not open responses with "great question", "excellent point", or similar. Acknowledge, analyze, recommend. The user does not want praise; they want correct, considered work.
- **Surface trade-offs the user may not have weighed** — especially around public API breakage, divergence from sibling SDKs, and coupling with `model-generator-net`.
- **When uncertain about a convention, read the sibling repo** before asking or assuming.

## Target project structure (post-migration)

- `Kontent.Ai.Management.Abstractions/` — public contracts, zero non-BCL dependencies.
- `Kontent.Ai.Management/` — Refit interface, builder, DI extensions, handlers, options, default implementations.
- `Kontent.Ai.Management.Tests/` — xUnit + MockHttp + Verify.
- Root: `Directory.Build.props`, `Directory.Packages.props`, `global.json`.
- `Kontent.Ai.Management.Helpers/` and its test project — fate pending (keep / fold in / deprecate). See open questions.

## Development commands

```bash
# Build
dotnet build

# Test
dotnet test

# Run one test project
dotnet test Kontent.Ai.Management.Tests/Kontent.Ai.Management.Tests.csproj
```

Project / solution paths will change during migration. Prefer commands without explicit paths so they keep working.

## Commits and versioning

- Commit messages follow `TICKET-ID - Description` (e.g. `EN-713 - Add component_types filter`). Branch names follow `TICKET-ID_Short_description`.
- During modernization, keep each PR scoped: framework/infra changes separate from per-domain rewrites separate from model regeneration.
- Package version: move off the per-csproj `<Version>` property to a central property in `Directory.Build.props` once the new structure lands, matching the siblings.

## Open questions (do not invent answers)

- Exact Refit interface name: `IManagementApi` is the natural fit, but confirm during the first migration PR.
- Future of `Kontent.Ai.Management.Helpers`: keep as a separate package, fold into the main package, or deprecate. No decision yet.
- Coordination point with `model-generator-net` for Management-model regeneration — the generated model shape must be agreed jointly before DTOs stabilize in this repo.

When you hit one of these, ask rather than guess.
