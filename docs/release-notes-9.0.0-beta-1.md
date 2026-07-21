# Kontent.ai Management SDK for .NET 9.0.0-beta-1

First public beta of the **ground-up modernized Management SDK**, targeting the [Management API v2](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/). This is a major, breaking rewrite of the `8.x` line: a [Refit](https://github.com/reactiveui/refit)-backed transport, `System.Text.Json` serialization, a result-based return type instead of thrown exceptions for API errors, materialized listings, immutable record DTOs, and two new entry points (DI registration and a fluent builder) alongside the existing constructor.

> [!WARNING]
> This is a **prerelease**. Install it with `--prerelease` — without that flag you get the stable `8.x` API, which these notes do **not** describe. Breaking changes may still land between prereleases until the first stable `9.x` ships; pin an exact version if you need stability during the beta. For production today, stay on the latest stable `8.x`.

> [!IMPORTANT]
> **Upgrading from `8.x`?** Read the [upgrade guide](upgrade-guide.md) before you start — it covers every breaking change with before/after examples. The sections below are a summary.

## Highlights

- **Result pattern instead of exceptions.** Methods no longer throw `ManagementException` on `4xx`/`5xx`. Every call returns `IManagementResult` / `IManagementResult<T>` — inspect `IsSuccess`, `Value`, `Error`, `StatusCode`, `RequestUrl`. Opt back into throwing with `EnsureSuccess()`, or use `TryGetValue(out var value)`. Branch on specific failures via the `ManagementErrorCodes` catalog. Only programmer errors, invalid configuration, and network/serialization failures still throw.
- **Three ways to create a client.** The `new ManagementClient(options)` constructor still works (now `IDisposable` / `IAsyncDisposable` — `await using` it). New: `services.AddManagementClient(...)` for DI (with keyed/named clients via `IManagementClientFactory`) and a fluent `ManagementClientBuilder` for non-DI customization.
- **Materialized listings.** `List…Async` walks every continuation page, merges them, and returns the whole set in one result (all-or-nothing — a failed page short-circuits, never a silently truncated set). Large listings (content items, assets, items-with-variants) also expose a streaming `Enumerate…PagesAsync` that yields one page at a time and lets you stop early.
- **Immutable, strongly-typed models.** Generated models are records; an element property *is* its value (`string Title`, `decimal? Price`, `IEnumerable<Reference>` for linked items) or a small companion record — `RichTextValue`, `DateTimeValue`, `UrlSlugValue`, `CustomValue` — each with an implicit conversion for the common case. Edit with a `with` expression. Date/time properties take a `DateTimeOffset` (stored as a UTC instant).
- **Typed element authoring without a generated model.** `ElementBuilder` and `dynamic[]` are gone. Build a typed `IReadOnlyList<BaseElement>` (one record per element kind), with `DynamicElement` as the escape hatch for unmodeled kinds or raw JSON.
- **`RichTextBuilder`.** Composes rich-text HTML and keeps the inline `<object>` placeholders and the `components` array in sync — it mints the shared GUIDs for you.
- **Built-in resilience.** A `Microsoft.Extensions.Http.Resilience` (Polly) pipeline is on by default — retries on transient failures and `429`, exponential backoff with jitter, `Retry-After` handling. Set `EnableResilience = false` to opt out, or replace it via the `configureResilience` hook / `WithResilience(...)`. Note: unlike Delivery/Sync, there is **no default per-attempt timeout** (uploads can run long); add one if you need it.
- **Streamlined asset creation.** `AssetCreateModel` is no longer generic; a one-call `CreateAssetAsync(FileContentSource, fileReference => …)` extension uploads the file and builds the asset around the resulting reference. The matching `UpsertAssetAsync` overload does create-or-update.

## Breaking changes (from `8.x`)

All detailed in the [upgrade guide](upgrade-guide.md); the ones you're most likely to hit:

- **Error handling** → result pattern ([§2](upgrade-guide.md#2-response-handling-exceptions--result-pattern)).
- **Serialization** → `System.Text.Json`; Newtonsoft is gone. Custom Newtonsoft converters and `[JsonProperty]` against SDK models no longer apply ([§8](upgrade-guide.md#8-serialization-newtonsoft--systemtextjson)).
- **Listings** → materialized `List…Async` / streaming `Enumerate…PagesAsync`; the `IListingResponseModel<T>` paging surface is gone ([§3](upgrade-guide.md#3-listings-manual-paging--materialized-results)).
- **Strongly-typed models** → immutable records; element properties are values / `*Value` records, not mutable element wrappers ([§4](upgrade-guide.md#4-language-variants-and-strongly-typed-models)).
- **Untyped authoring** → typed `BaseElement` records; `ElementBuilder.GetElementsAsDynamic(...)` and `dynamic[]` removed ([§5](upgrade-guide.md#5-authoring-elements-without-a-generated-model)).
- **Assets** → non-generic `AssetCreateModel`; the two asset-reference types collapsed onto `AssetReference` (a rendition is a `RenditionReference`; `Renditions = null` keeps them, `[]` removes them) ([§7](upgrade-guide.md#7-assets)).
- **DTO contracts** → widespread `required` members, nullability corrections, and a focused set of renames/retypes to match the Management API v2 wire contract — including response/request collections standardized on `IReadOnlyList<T>` and the environment id typed as `Guid`. The compiler surfaces each one ([§10](upgrade-guide.md#10-model-and-dto-changes)).

## Removed

| Removed | Replacement |
|---------|-------------|
| `ManagementException` as control flow | Result pattern; `EnsureSuccess()` opts back into throwing |
| `ElementBuilder` / `GetElementsAsDynamic(...)` / `dynamic[]` | Typed `IReadOnlyList<BaseElement>`; `DynamicElement` |
| `IListingResponseModel<T>` paging | `List…Async` + `Enumerate…PagesAsync` |
| Generic `AssetCreateModel<T>` | Non-generic `AssetCreateModel` |
| Newtonsoft.Json | `System.Text.Json` |
| Web Spotlight activation (`ActivateWebSpotlightAsync` & co., `WebSpotlightModel`) | Live preview (preview-configuration endpoints); the Spaces `root_item` field remains |
| `Kontent.Ai.Management.Helpers` (`EditLinkBuilder`, …) | No replacement — keep equivalent edit-link logic in application code |
| Legacy hand-rolled HTTP (`ActionInvoker`, `ManagementHttpClient`, `EndpointUrlBuilder`) | Refit transport + resilience pipeline |

## Known limitations

- **Prerelease.** The public API may still change between betas.
- **Management-model generation is in active development.** The [model generator](https://github.com/kontent-ai/model-generator-net)'s Management output is not yet released. Until it ships, hand-write the strongly-typed records or use the typed raw `BaseElement` authoring path ([§5](upgrade-guide.md#5-authoring-elements-without-a-generated-model)).

## Installation

```bash
dotnet add package Kontent.Ai.Management --prerelease
```

## Requirements

- .NET 8.0
- A Kontent.ai environment with Management API v2 access (a Management API key)
