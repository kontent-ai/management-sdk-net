# CLAUDE.md

Guidance for Claude Code agents working in this repository.

## Overview

Official Kontent.ai **Management SDK for .NET**. The ground-up modernization that mirrors the two sibling SDKs has **landed** — the repo is now in **beta hardening**, not mid-migration.

- `../delivery-sdk-net` — the primary architecture reference.
- `../sync-sdk-net` — the secondary reference, same patterns at smaller scale.

What's already in place on `vnext`: Refit-backed transport (`IManagementApi`, split per-domain), `System.Text.Json` serialization, the result pattern (`IManagementResult<T>` over throw-on-4xx/5xx), DI extensions with keyed clients, a fluent non-DI builder, a Polly resilience pipeline, and an auth `DelegatingHandler`. **Treat the current shape as canonical** unless a `beta-improvements*.md` entry explicitly supersedes it. The legacy architecture (hand-rolled `ActionInvoker`/`ManagementHttpClient`/`EndpointUrlBuilder`, Newtonsoft.Json) is gone — do not reintroduce it.

This doc is an **evergreen description of how the SDK is built and the conventions to follow**. It is not a progress tracker. Active refactoring work, decisions, and task state live in the uncommitted `beta-improvements*.md` scratch files — keep them out of here so this doc doesn't rot.

## Working stance (beta hardening)

- **This is still pre-1.0 beta. Breaking changes are acceptable when justified.** No backward-compat shims, no legacy code paths "just in case", no obsolete types kept past their natural lifetime.
- **But public-API breaks must clear a bar:** does the change enable a clearly better architecture, fix a real defect, or remove real friction? Renaming a stable, sensible type or method purely to "modernize naming" does **not** clear it. Familiarity has value.
- **The transport is Refit.** The `IManagementApi` Refit interface is the transport layer; everything public wraps it. Do not hand-roll HTTP composition.
- **Model generation is coordinated.** `../model-generator-net` will re-introduce Management-model generation (it currently emits Delivery models only — v19+ records). Request/response model shapes here must stay generator-friendly: record-based, immutable, `System.Text.Json`-serializable.

## Sibling repos are canonical references

`../delivery-sdk-net` and `../sync-sdk-net` sit next to this repo in the filesystem. You may read anything in them freely — source, tests, build files, `CLAUDE.md` — to answer architecture, style, or convention questions. When a design question comes up, **read the sibling first** and only diverge with a stated reason.

Concrete anchors to consult:

| Concern                          | Canonical file(s)                                                                  |
|----------------------------------|------------------------------------------------------------------------------------|
| Refit interface layout           | `../delivery-sdk-net/Kontent.Ai.Delivery/Api/IDeliveryApi.*.cs` (partial per-domain) · here: `Kontent.Ai.Management/Api/IManagementApi.*.cs` |
| DI extensions + keyed clients    | `../delivery-sdk-net/Kontent.Ai.Delivery/Extensions/ServiceCollectionExtensions*.cs` · here: `Kontent.Ai.Management/Extensions/ServiceCollectionExtensions*.cs` |
| Fluent non-DI builder            | `../delivery-sdk-net/Kontent.Ai.Delivery/DeliveryClientBuilder.cs`, `../sync-sdk-net/Kontent.Ai.Sync/Configuration/SyncClientBuilder.cs` · here: `Kontent.Ai.Management/Configuration/ManagementClientBuilder.cs` |
| Refit settings + `System.Text.Json` | `ServiceCollectionExtensions.HttpClient.cs` in delivery-sdk-net (`SystemTextJsonContentSerializer`) |
| Resilience pipeline, auth handler | `../sync-sdk-net/Kontent.Ai.Sync/Handlers/`, its `ServiceCollectionExtensions.cs` · here: `Kontent.Ai.Management/Handlers/` |
| Verify-based API approval tests  | `../delivery-sdk-net/Kontent.Ai.Delivery.Abstractions.Tests/`                       |
| Build infra                      | `Directory.Build.props`, `Directory.Packages.props`, `global.json` in either sibling |

If a sibling's convention and your instinct disagree, defer to the sibling unless you can articulate why this SDK genuinely needs to differ.

## Target framework and language

- **.NET 8 today.** Single target — no multi-targeting, no `netstandard`.
- **Planned bump to .NET 10 at the production (GA) release.** The whole Kontent.ai .NET stack (management + delivery + sync) moves together: stay on `net8.0` through beta, then bump straight to `net10.0` at GA (skipping 9, LTS→LTS), coordinated across the sibling SDKs and landed before the .NET 8 EOL window. Until that coordinated bump, keep targeting `net8.0` here.
- Centralize `LangVersion=latest`, `Nullable=enable`, `ImplicitUsings=enable`, analyzers, and deterministic-build settings in the root `Directory.Build.props` — not per-project.
- Central Package Management via `Directory.Packages.props`. SDK pinned via `global.json`.
- **Use modern C# actively**, not grudgingly: primary constructors, file-scoped namespaces, `sealed` by default on non-abstract types, `record` / `readonly record struct` for DTOs and value objects, `required` members, collection expressions, `init`-only setters, `ArgumentNullException.ThrowIfNull`.
- **Prefer pattern matching** over `if`/cast chains: switch expressions for mapping, property patterns for shape checks, `is not null`, list patterns where they apply. Reach for `if` only when pattern matching would be strained.
- Serialization is `System.Text.Json` (matching the siblings' `SystemTextJsonContentSerializer` Refit configuration). Newtonsoft is out.

## Architecture (as built)

- **Refit-backed HTTP.** The public `ManagementClient` wraps an internal `IManagementApi` Refit interface, split into partial files per domain under `Api/` (`IManagementApi.Asset.cs`, `IManagementApi.ContentItem.cs`, `IManagementApi.Language.cs`, …), mirroring `IDeliveryApi.*.cs`. `ISubscriptionApi` covers the subscription scope.
- **Two entry points, same as the siblings:**
  - `ManagementClientBuilder` (`Configuration/`) — fluent, no-DI bootstrap for scripts and simple consumers. It is client-first / container-free: it owns its `HttpClient`s directly rather than spinning a private `ServiceCollection`.
  - `services.AddManagementClient(...)` (`Extensions/ServiceCollectionExtensions*.cs`) — DI extension with keyed-services support for multiple named clients.
- **Result pattern for API calls.** Public calls return `IManagementResult<T>` (status / success flag / value / request URL) rather than throwing on 4xx/5xx. The result wraps the **HTTP response only**.
- **Transport errors throw by design.** Network-level / transport failures throw — they are not projected into a failure result (the sibling SDKs do the same). The result pattern covers API-level non-success status codes, not dead sockets.
- **Listings are materialized.** Every listing endpoint is exposed as `List{Plural}Async` returning `IManagementResult<IReadOnlyList<T>>`. There is no streaming/`IAsyncEnumerable` public surface — pagination is collected internally via `PageEnumerator`.
- **Resilience via `Microsoft.Extensions.Http.Resilience`** (Polly pipelines), with resilience as the outermost handler in both the DI and standalone paths. Authentication via a `DelegatingHandler` (`Handlers/`) that attaches the Management API key. Pipeline shape copied from `sync-sdk-net`.
- **Generated-model mapping attributes.** `KontentTypeAttribute` / `KontentElementAttribute` / `KontentEnumValueAttribute` encode the asymmetric wire mapping (codename out / id in) that STJ can't express on its own; they are read at runtime by the converters. Validation/constraint attributes and the local `ContentItemValidator` have been removed — the MAPI is the source of truth for validation.

## Coding and commenting standards

- **KISS — favor the simplest thing that works.** Don't overengineer: no speculative abstraction layers, no extensibility hooks nobody asked for, no patterns applied for their own sake. Keep code concise and readable; the best change is often a smaller one. A clever solution that a future reader has to decode loses to a plain one.
- **Avoid reflection unless genuinely necessary.** Prefer compile-time, statically-checkable code (generics, pattern matching, direct dispatch, source generation). Reflection is acceptable only where there's no static alternative (e.g. the attribute-driven wire mapping the converters already depend on) — not as a convenience shortcut.
- **Good code is self-explanatory. Do not document every bit of code.** XML doc comments belong on the public consumer-facing surface so consumers get IntelliSense. Private implementation does not need method-by-method narration.
- **Only comment the non-obvious.** A hidden API constraint, a subtle invariant, a workaround for a specific server-side quirk, a decision whose rationale would surprise a future reader. If removing the comment would not confuse anyone, do not write it.
- **Encode API learnings in the type system, not in prose.** "Always populated", "verified by live test" and similar are noise once the type (e.g. a non-nullable property) already reflects the fact.
- **Do not annotate corrections.** When the user corrects your approach, fix the code and move on. No `// changed from X because Y` / `// previously used Z` — that history belongs in the commit message at most, usually nowhere.
- **Do not add defensive code for impossible scenarios.** Trust framework and internal guarantees. Validate only at external boundaries (public API entry points, deserialized payloads from the network).
- **No referencing of development markdown files in comments.** The `beta-improvements*.md` and similar planning/notes files are dev-only scratch, not committed, and must not be referenced by any code comment.
- **No dead code, no `// TODO` drifting across PRs, no commented-out blocks.**
- **Prefer pattern matching.** See above.

## Collaboration stance

- **Be critical, not compliant.** If a request conflicts with the principles above, the sibling SDKs' conventions, or good design in general, push back with reasoning **before** implementing. "The user asked for it" is not sufficient justification for a poor design.
- **No flattery.** Do not open responses with "great question", "excellent point", or similar. Acknowledge, analyze, recommend. The user wants correct, considered work, not praise.
- **Surface trade-offs the user may not have weighed** — especially around public API breakage, divergence from sibling SDKs, and coupling with `model-generator-net`.
- **When uncertain about a convention, read the sibling repo** before asking or assuming.

## Project structure

- `Kontent.Ai.Management/` — Refit interface (`Api/`), builder (`Configuration/`), DI extensions (`Extensions/`), handlers (`Handlers/`), converters (`Conversion/`, `Serialization/`), mapping attributes (`Annotations/`), source-tracking attribute (`Attributes/`), and the models (`Models/`).
- `Kontent.Ai.Management.Tests/` — xUnit + `RichardSzalay.MockHttp` for HTTP, Verify for public-API approval snapshots. JSON fixtures stay for request/response bodies.
- Root: `Directory.Build.props`, `Directory.Packages.props`, `global.json`, `Kontent.Ai.Management.sln`.

There is **no** separate Abstractions project and **no** Helpers project — both were considered and dropped. Public contracts live alongside the implementation in `Kontent.Ai.Management`.

## Development commands

```bash
# Build
dotnet build

# Test
dotnet test

# Run one test project
dotnet test Kontent.Ai.Management.Tests/Kontent.Ai.Management.Tests.csproj
```

Prefer commands without explicit paths so they keep working if layout shifts.

## Commits and versioning

- Commit messages follow `TICKET-ID - Description` (e.g. `EN-713 - Add component_types filter`). Branch names follow `TICKET-ID_Short_description`.
- Keep each PR scoped: framework/infra changes separate from per-domain rewrites separate from model regeneration.
- Package version lives in a central property (`Directory.Build.props`), matching the siblings — not in per-csproj `<Version>`.

## Open questions (do not invent answers)

- **Coordination point with `model-generator-net`** for Management-model regeneration — the generated model shape (records, mapping attributes) must be agreed jointly before DTOs stabilize here. This is the main remaining cross-repo dependency.

When you hit this, ask rather than guess.
