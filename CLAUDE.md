# CLAUDE.md

Guidance for Claude Code agents working in this repository.

## Overview

Official Kontent.ai **Management SDK for .NET** — a client for the [Management API v2](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/) (a *write* API: content items, variants, assets, content model, workflows, environment administration). The `9.x` line is a ground-up modernization of `8.x`; the architecture below is settled — treat it as canonical.

The pillars:

- **Result pattern.** Public calls return `IManagementResult<T>` (success flag, value, `IError` with the API's error detail, status code, request URL) instead of throwing on 4xx/5xx. Transport-level failures throw by design — the result wraps the HTTP response only. `EnsureSuccess()` / `TryGetValue()` / `AsFailure<T>()` are the opt-in conveniences.
- **Refit transport.** The public `ManagementClient` (partial per domain) wraps the internal `IManagementApi` Refit interface (partial per domain under `Api/`); `ISubscriptionApi` covers the subscription scope. Everything public funnels through `RefitApiResponseExtensions.ToManagementResultAsync` — no endpoint bypasses it, no hand-rolled HTTP.
- **`System.Text.Json`** with a small set of converters encoding real MAPI quirks (polymorphic elements, codename-out/id-in mapping, string-encoded numbers). Newtonsoft is gone; do not reintroduce it.
- **Materialized listings.** Every listing is `List{Plural}Async` → `IManagementResult<IReadOnlyList<T>>`, drained internally via `PageEnumerator` (all-or-nothing: first failed page short-circuits). Unbounded listings additionally expose `Enumerate{X}PagesAsync` page streams.
- **Resilience by default** (`Microsoft.Extensions.Http.Resilience`/Polly), outermost in the handler chain, with **idempotency-aware retries**: 429 retries every method; transient failures/5xx retry idempotent methods only. This is a write API — never weaken that invariant. Auth and tracking are `DelegatingHandler`s under it.
- **Two entry points**: `services.AddManagementClient(...)` (DI, keyed/named clients, options validation) and `ManagementClientBuilder` (fluent, container-free — owns its `HttpClient`s). The plain constructor also works and is disposable.

## Current phase

Late `9.0` beta. GA is planned together with a coordinated `net8.0` → `net10.0` bump across the whole Kontent.ai .NET stack (see `net-10-updates.md` while it exists). **The window for casual breaking changes is closing**: a break now needs a real defect or a clearly better architecture behind it, an entry in the release notes *and* the upgrade guide, and an approval-snapshot update. Renaming stable, sensible API purely to modernize naming does not clear the bar — familiarity has value.

## Sibling repos are canonical references

`../delivery-sdk-net` (primary) and `../sync-sdk-net` (secondary) sit next to this repo. Read them freely for architecture, style, or convention questions; when a design question comes up, **read the sibling first** and only diverge with a stated reason (documented divergences: no default per-attempt timeout, idempotency-aware retries — both because this SDK writes).

## Target framework and language

- **.NET 8 today; .NET 10 at GA** (skip 9, LTS→LTS), coordinated across management + delivery + sync before the .NET 8 EOL window. Until that bump, keep `net8.0`.
- `LangVersion=latest`, `Nullable=enable`, `ImplicitUsings=enable`, analyzers, deterministic build — centralized in `Directory.Build.props`. Central Package Management via `Directory.Packages.props`; SDK pinned via `global.json`.
- **Use modern C# actively**: primary constructors, file-scoped namespaces, `sealed` by default, `record` for DTOs, `required` members, collection expressions, `init`-only setters, pattern matching over `if`/cast chains, `ArgumentNullException.ThrowIfNull`.

## API surface conventions

- **Verbs**: `Get` (single or envelope model), `List{Plural}` (materialized `IReadOnlyList`), `Enumerate{X}PagesAsync` (page stream), `Create` (POST), `Upsert`/`Update` (PUT), `Modify` (PATCH — only PATCH), `Delete`. Parameter order is `(identifier, payload, cancellationToken = default)` everywhere.
- **The interface is one method per API operation** (plus typed `<T>` projections of the same operation). Conveniences that *compose or adapt* — fetched-model→request adapters, multi-call helpers — live in the extensions tier (`Extensions/ManagementClientExtensions` and friends), never on `IManagementClient`.
- **Identifiers**: `Reference` is factory-only (`ById`/`ByCodename`/`ByExternalId`) and stays explicit — no implicit conversions (decided; `ToReference()`/`ToIdentifier()` extensions are the sanctioned ergonomic path). URL segments are left **raw** in `ToUrlSegment` — Refit's `{**}` catch-all percent-encodes exactly once; pre-escaping double-encodes.
- **Patch factories** are the curated way through patch grammars: `ContentTypePatch`/`ContentTypeSnippetPatch` (JSON-pointer paths), `LanguagePatch`/`SpacePatch`/`TaxonomyGroupPatch`/`CustomAppPatch` (property-name enums). Raw operation records remain the escape hatch.
- **Experimental surface** carries `[Experimental("KAIM001")]` (currently the content-model snapshot). New not-yet-contractual features follow the same pattern.

## Model conventions

- Sealed, immutable `record`s; `required` for what the API always returns/demands; nullability mirrors the wire contract exactly ("encode API learnings in the type system, not in prose").
- Explicit `[JsonPropertyName]` on **every** property — the serializer options deliberately have no naming policy.
- **All collection properties are `IReadOnlyList<T>`** (never `IEnumerable`, `ISet`, or concrete types). Method *parameters* may accept `IEnumerable<T>`.
- Names mirror Kontent.ai API terminology; request and response shapes are separate records when the wire shapes differ (a response model with a fake-`required` field forced into a request body is a defect — see `UserRolesUpdateModel`).
- Models must stay **generator-friendly** (record-based, immutable, STJ-serializable) — the shape is coordinated with `../model-generator-net`.
- Generated/typed content models map via `KontentTypeAttribute`/`KontentElementAttribute`/`KontentEnumValueAttribute`; the converters read them at runtime (typed *reads* match by id — environment-bound; *writes* key by codename — portable).

## Adding or changing an endpoint — the playbook

1. **Refit method** in the matching `Api/IManagementApi.{Domain}.cs` partial (`internal`, suffix `InternalAsync`, returns `IApiResponse<T>`; identifiers travel as pre-rendered `{**segment}` catch-all strings).
2. **Implementation** in `ManagementClient.{Domain}.cs`: null-check args, `identifier.ToUrlSegment()`, `.ToManagementResultAsync()`. Listings go through `PageEnumerator.CollectAsync`/`EnumerateAsync`.
3. **Declaration + XML docs** in `IManagementClient.cs` (docs describe the operation and the result; typed overloads cross-reference the environment-bound caveat).
4. **Tests** in `Kontent.Ai.Management.Tests/ManagementClientTests/{Domain}Tests.cs`: MockHttp `Expect` on the exact URL, JSON fixture under `Data/{Domain}/`, `CaptureBody` + `ShouldMatchSerialized` for write bodies, `PagedFixtures.ConcatPages` for listings, null-guard tests.
5. **Approval snapshot**: the Verify test fails on any public-surface change; review the `.received.txt` diff line-by-line, then copy it over `.verified.txt` — only for intended changes.
6. **Docs**: README section for the new surface, release-notes entry, upgrade-guide entry if breaking. *A public-surface change without a README touch is an incomplete change.*
7. Wire contract in doubt? Verify against the OpenAPI reference or the JS SDK's contracts (`kontent-ai/management-sdk-js`, `lib/models`/`lib/contracts`) — and say what you verified against.

## Testing conventions

- xUnit + `RichardSzalay.MockHttp` (wired as the primary handler through `ManagementApiFactory`, so tests exercise the real Refit + handler chain) + Verify for the public-API approval snapshot. JSON fixtures per domain under `Data/`.
- `MockClientFactory.Create()` for domain tests; inject a scoped `ContentItemEnvelopeConverter` for typed-model tests (auto-scan trips the deliberate test-assembly codename collision).
- **CodeSamples are tests** (`CodeSamples/*.cs`) and double as documentation-grade example code — keep them modern and idiomatic (result handling via `EnsureSuccess()`, identifier factories, patch facades). `CreateForSample`'s fallback returns an empty 200, which maps to a *failure* for value-returning calls — samples that unwrap need a real fixture, and listing fixtures must carry a `null` continuation token or pagination loops forever.
- Wire-level guarantees get explicit serialization tests (e.g. `RequestDefaultsSerializationTests` pins that ergonomic defaults keep the payload byte-identical). Zero-regression on the wire is the standing bar for "ergonomics" changes.

## Coding and commenting standards

- **KISS — favor the simplest thing that works.** No speculative abstraction layers, no extensibility hooks nobody asked for. A clever solution a future reader has to decode loses to a plain one.
- **Avoid reflection unless genuinely necessary.** The attribute-driven wire mapping in `Conversion/` is the sanctioned exception; cache anything reflective.
- **Good code is self-explanatory.** XML docs belong on the public consumer-facing surface (IntelliSense); private implementation does not need narration.
- **Only comment the non-obvious**: a hidden API constraint, a subtle invariant, a server-side quirk workaround, a decision whose rationale would surprise. If removing the comment would confuse nobody, don't write it.
- **Do not annotate corrections or history** in code — no "changed from X" comments; that belongs in the commit message at most.
- **No dead code, no drifting `// TODO`s, no commented-out blocks. No references to dev-only notes/planning files from committed code or comments.**
- **Do not add defensive code for impossible scenarios.** Validate at external boundaries only (public API entry points, deserialized network payloads).

## Collaboration stance

- **Be critical, not compliant.** If a request conflicts with these principles, the siblings' conventions, or good design, push back with reasoning before implementing.
- **No flattery.** Acknowledge, analyze, recommend.
- **Surface trade-offs the user may not have weighed** — public API breakage, sibling divergence, `model-generator-net` coupling, wire-contract risk.
- When uncertain about a convention, read the sibling repo before asking or assuming.

## Project structure

- `Kontent.Ai.Management/` — `Api/` (Refit partials), `Configuration/` (options, builder, Refit settings), `Extensions/` (DI registration, client conveniences, `PageEnumerator`, result mapping), `Handlers/` (auth, tracking), `Conversion/` + `Serialization/` (typed-model envelope converter, STJ converters), `Annotations/` (generated-model mapping attributes), `Models/` (per-domain DTOs), root-level result types and error catalog (`ManagementErrorCodes` — curated, not exhaustive; MAPI codes are not unique).
- `Kontent.Ai.Management.Tests/` — mirrors the above; `Base/` holds the shared test infrastructure.
- Root: `Directory.Build.props`, `Directory.Packages.props`, `global.json`, solution. No Abstractions or Helpers project — considered and dropped; public contracts live with the implementation.
- `docs/` — per-release notes and the `8.x` → `9.x` upgrade guide. Keep both current with every user-visible change.

## Development commands

```bash
dotnet build
dotnet test
```

Prefer commands without explicit paths so they keep working if layout shifts.

## Commits and versioning

- Commit messages follow `TICKET-ID - Description` when a ticket exists (e.g. `EN-713 - Add component_types filter`); otherwise a concise lowercase summary matching the branch history. Branch names follow `TICKET-ID_Short_description`.
- Keep each PR scoped: infra separate from per-domain work separate from model changes.
- Package version comes from the release git tag: `.github/workflows/release.yml` strips the `v` prefix and passes it as `/p:Version` and `/p:PackageVersion` at build/pack time — no version property in any project file.

## Open questions (do not invent answers)

- **Coordination with `model-generator-net`** for Management-model generation — the generated model shape (records, mapping attributes, collection types) must be agreed jointly before DTOs are declared final. This is the main remaining cross-repo dependency.
- **Webhook trigger switches** (`Enabled`/`Events`/`Slot` nullability) and the **webhook update endpoint** need live-API verification before changing.

When you hit these, ask rather than guess.
