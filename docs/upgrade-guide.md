# Upgrade Guide: Migrating to the Modernized Kontent.ai Management SDK for .NET

This guide covers migrating from the legacy Management SDK (the `8.x` line) to the modernized SDK shipped during the **vnext** rewrite. The new SDK is a ground-up modernization: a [Refit](https://github.com/reactiveui/refit)-backed transport, `System.Text.Json` serialization, a result-based return type on every method instead of thrown exceptions for API errors, materialized listings, immutable record DTOs, and two new entry points (DI registration and a fluent builder) alongside the existing constructor.

> [!WARNING]
> The modernized API ships as a **prerelease** during the beta. Install it with the `--prerelease` flag — without it you get the legacy `8.x` API, which this guide does **not** describe. Breaking changes may still land between prereleases until the first stable major ships; pin an exact version if you need stability during the beta. For production today, stay on the latest stable `8.x` release.

```bash
dotnet add package Kontent.Ai.Management --prerelease
```

## Table of Contents

- [Overview](#overview)
- [Quick Migration Checklist](#quick-migration-checklist)
- [1. Creating the Client](#1-creating-the-client)
- [2. Response Handling: Exceptions → Result Pattern](#2-response-handling-exceptions--result-pattern)
- [3. Listings: Manual Paging → Materialized Results](#3-listings-manual-paging--materialized-results)
- [4. Language Variants and Strongly-Typed Models](#4-language-variants-and-strongly-typed-models)
- [5. Authoring Elements Without a Generated Model](#5-authoring-elements-without-a-generated-model)
- [6. Rich Text and Inline Components](#6-rich-text-and-inline-components)
- [7. Assets](#7-assets)
- [8. Serialization: Newtonsoft → System.Text.Json](#8-serialization-newtonsoft--systemtextjson)
- [9. Resilience and the HTTP Pipeline](#9-resilience-and-the-http-pipeline)
- [10. Model and DTO Changes](#10-model-and-dto-changes)
- [11. Removed Features and Types](#11-removed-features-and-types)
- [Troubleshooting](#troubleshooting)

## Overview

### What changed, at a glance

| Area | Change type | Description |
|------|-------------|-------------|
| Error handling | Breaking | Methods no longer throw `ManagementException` on `4xx`/`5xx`. Every method returns `IManagementResult` / `IManagementResult<T>` — inspect `IsSuccess` / `Value` / `Error`. |
| Serialization | Breaking | Newtonsoft.Json replaced by `System.Text.Json`. Custom `JsonConverter`s and `[JsonProperty]` usage against SDK models no longer apply. |
| Transport | Mostly internal | Hand-rolled HTTP (`ActionInvoker` / `ManagementHttpClient` / `EndpointUrlBuilder`) replaced by a Refit interface. Visible only if you customized the HTTP layer. |
| Listings | Breaking | `IListingResponseModel<T>` with `HasNextPage()` / `GetNextPage()` / `GetAllAsync()` replaced by materialized `List…Async` methods, plus a streaming `Enumerate…PagesAsync` option for large continuation-paged sets. |
| Strongly-typed models | Breaking | Element properties are plain values or `*Value` records (e.g. `string Title`, `RichTextValue Content`) instead of mutable element wrappers (`TextElement { Value }`). Models are immutable records. |
| Untyped element authoring | Breaking | `ElementBuilder.GetElementsAsDynamic(...)` and anonymous `dynamic[]` removed. Author with typed `BaseElement` records; `DynamicElement` is the escape hatch. |
| Rich text | New helper | `RichTextBuilder` keeps inline `<object>` placeholders and the `components` array in sync. |
| DI & bootstrap | Additive | New `services.AddManagementClient(...)` and `ManagementClientBuilder`, alongside the existing `new ManagementClient(options)` constructor. |
| DTO contracts | Breaking | Widespread `required` members, nullability corrections, and a set of renames/removals — see [§10](#10-model-and-dto-changes). |
| Web Spotlight | Removed | Activation endpoints removed (superseded by live preview). |
| Target framework | — | `net8.0` only. |

### Package

The package name is unchanged — `Kontent.Ai.Management`. The modernized API is the prerelease stream; the `8.x` releases remain the legacy stable line.

## Quick Migration Checklist

- [ ] Update the package with `--prerelease`.
- [ ] Replace every `try { … } catch (ManagementException)` around a client call with an `if (!result.IsSuccess)` branch on the returned result (or opt into `EnsureSuccess()` / `TryGetValue(...)`).
- [ ] Update content access from `response.Property` to `result.Value.Property`.
- [ ] Replace listing paging loops (`HasNextPage()` / `GetNextPage()` / `GetAllAsync()`) with a single `List…Async` call, or `Enumerate…PagesAsync` for large sets.
- [ ] Regenerate or hand-update strongly-typed models: element properties are now plain values / `*Value` records, and the model is an immutable record.
- [ ] Replace `ElementBuilder.GetElementsAsDynamic(...)` and `dynamic[]` element arrays with typed `BaseElement` records (or `DynamicElement` for unmodeled kinds).
- [ ] Replace hand-written rich-text component placeholders with `RichTextBuilder`.
- [ ] Update asset creation from `AssetCreateModel<T>` + `ElementBuilder` to `AssetCreateModel` with a typed `Elements` list (and consider the one-call `CreateAssetAsync(FileContentSource, factory)` extension).
- [ ] Remove any custom Newtonsoft `JsonConverter`s / `[JsonProperty]` attributes targeting SDK models.
- [ ] Remove Web Spotlight activation calls.
- [ ] Add the now-`required` members to your object initializers, and add null checks where reads became nullable ([§10](#10-model-and-dto-changes)).
- [ ] Dispose standalone clients (`await using`) — `IManagementClient` is now `IDisposable` / `IAsyncDisposable`.

---

## 1. Creating the Client

The constructor still works exactly as before, so the simplest migration is a no-op here. What's new is **two additional entry points** and **client disposability**.

### 1.1 Standalone constructor (unchanged, but now disposable)

**Legacy:**
```csharp
var client = new ManagementClient(new ManagementOptions
{
    EnvironmentId = "<YOUR_ENVIRONMENT_ID>",
    ApiKey = "<YOUR_API_KEY>"
});
```

**New:**
```csharp
await using var client = new ManagementClient(new ManagementOptions
{
    EnvironmentId = "<YOUR_ENVIRONMENT_ID>",
    ApiKey = "<YOUR_API_KEY>"
});
```

A standalone client now owns its `HttpClient` instances and implements both `IDisposable` and `IAsyncDisposable` — dispose it when you're done (hence `await using`).

### 1.2 Dependency injection (new, recommended for apps)

```csharp
services.AddManagementClient(options =>
{
    options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
    options.ApiKey = "<YOUR_API_KEY>";
});
```

`IManagementClient` is then resolvable from the container, which owns its lifetime and `HttpClient` pipeline via `IHttpClientFactory`. Bind from configuration instead with `services.AddManagementClient(configuration)` (default section `ManagementOptions`).

> [!NOTE]
> A DI-resolved client is owned by the container — do **not** dispose it yourself. Disposal is a no-op on DI-managed instances.

Register multiple named clients and resolve them through `IManagementClientFactory`:

```csharp
services.AddManagementClient("production", o => { o.EnvironmentId = "…"; o.ApiKey = "…"; });
services.AddManagementClient("staging",    o => { o.EnvironmentId = "…"; o.ApiKey = "…"; });

// var production = clientFactory.Get("production");
```

### 1.3 Fluent builder (new, non-DI customization)

When you are not using DI but need to customize the resilience pipeline or Refit settings:

```csharp
await using var client = ManagementClientBuilder
    .WithOptions(options =>
    {
        options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
        options.ApiKey = "<YOUR_API_KEY>";
    })
    .WithResilience(pipeline => pipeline.AddTimeout(TimeSpan.FromSeconds(30)))
    .Build();
```

### 1.4 Options changes

`ManagementOptions` keeps `EnvironmentId`, `ApiKey`, and `SubscriptionId`. The new `EnableResilience` flag (default `true`) toggles the built-in retry pipeline. Options now validate on use — a missing/malformed `EnvironmentId` or `ApiKey` surfaces as a `ValidationException` from the constructor/builder, or an `OptionsValidationException` when DI options validation runs during host startup.

---

## 2. Response Handling: Exceptions → Result Pattern

This is the single largest break. The SDK **no longer throws** on Management API errors (`4xx`/`5xx`). Every method returns a result you inspect. Network-level and serialization failures still propagate as exceptions.

**Legacy:**
```csharp
try
{
    var variant = await client.UpsertLanguageVariantAsync(identifier, elements);
    Console.WriteLine(variant.Item.Id);
}
catch (ManagementException ex)
{
    Console.WriteLine(ex.StatusCode);
    Console.WriteLine(ex.Message);
}
```

**New:**
```csharp
var result = await client.UpsertLanguageVariantAsync(identifier, model);

if (result.IsSuccess)
{
    Console.WriteLine(result.Value.Item.Id);
}
else
{
    Console.WriteLine($"{result.StatusCode}: {result.Error?.Message}");
    Console.WriteLine($"Request ID: {result.Error?.RequestId}");   // quote when reporting an issue

    foreach (var validationError in result.Error?.ValidationErrors ?? [])
    {
        Console.WriteLine(validationError.Message);
    }
}
```

A result carries `IsSuccess`, `Value` (on success, `IManagementResult<T>` only), `Error` (on failure), and `StatusCode` / `RequestUrl` diagnostics from the HTTP response.

### 2.1 Opt-in conveniences

If you'd rather not branch, two helpers live on the result:

```csharp
// Throw on failure (ManagementException carries the IError) and get the value:
var item = (await client.GetContentItemAsync(Reference.ByCodename("on_roasts"))).EnsureSuccess();

// Or the Try pattern:
if ((await client.GetContentItemAsync(Reference.ByCodename("on_roasts"))).TryGetValue(out var value))
{
    Console.WriteLine(value.Name);
}
```

`EnsureSuccess()` is the closest thing to the old throwing behavior — but it's opt-in, and the exception type is `ManagementException`, not the legacy one. The SDK itself never throws it.

### 2.2 Branching on a specific error

`ManagementException`-catch-and-inspect-`ErrorCode` becomes an `ErrorCode` comparison against the `ManagementErrorCodes` catalog:

```csharp
var result = await client.UpsertLanguageVariantAsync(identifier, article);

if (!result.IsSuccess && result.Error?.ErrorCode == ManagementErrorCodes.PublishedOrScheduledVariantCannotBeUpdated)
{
    await client.CreateNewVersionOfLanguageVariantAsync(identifier);
    result = await client.UpsertLanguageVariantAsync(identifier, article);
}
```

> [!NOTE]
> The codes are **not unique** — the API reuses some across unrelated conditions — so inspect `Message` as well when the distinction matters.

> [!IMPORTANT]
> Exceptions are now reserved for **programmer errors** (e.g. a `null` argument), **invalid configuration**, and **network/serialization failures** — not for API errors. A `404` or an API validation rejection comes back as `IsSuccess == false`, never as a thrown exception.

---

## 3. Listings: Manual Paging → Materialized Results

Continuation-token paging is handled internally. The legacy `IListingResponseModel<T>` paging surface is gone.

**Legacy:**
```csharp
var items = new List<ContentItemModel>();
var response = await client.ListContentItemsAsync();

while (true)
{
    items.AddRange(response);
    if (!response.HasNextPage()) break;
    response = await response.GetNextPage();
}

// …or the all-at-once helper:
var all = await client.ListContentItemsAsync().GetAllAsync();
```

**New:**
```csharp
var result = await client.ListContentItemsAsync();

if (!result.IsSuccess)
{
    Console.WriteLine($"Failed: {result.Error?.Message}");
    return;
}

foreach (var item in result.Value)   // result.Value is IReadOnlyList<ContentItemModel>
{
    Console.WriteLine(item.Name);
}
```

`List…Async` walks every page, merges them, and returns the whole set in one result. It is **all-or-nothing**: if any page fails, that first failure short-circuits and is returned, so you never get a silently truncated set.

### 3.1 Streaming large listings

The endpoints whose results can grow large — content items, assets, and the items-with-variants filter / bulk-get — expose an `Enumerate…PagesAsync` overload that streams one continuation-token page at a time (and lets you stop early), replacing the old manual `GetNextPage()` loop where memory matters:

```csharp
await foreach (var page in client.EnumerateContentItemPagesAsync())
{
    if (!page.IsSuccess)
    {
        Console.WriteLine($"A page failed: {page.Error?.Message}");
        break;
    }

    foreach (var item in page.Value)   // one page's worth
    {
        Console.WriteLine(item.Name);
    }
}
```

The next page is fetched only when you iterate past the current one. Reach for this only when a listing is genuinely large; everywhere else, `List…Async` is simpler.

---

## 4. Language Variants and Strongly-Typed Models

Strongly-typed models still mirror your content types, but their **shape changed** and they're now **immutable records**.

### 4.1 Element properties are values, not wrappers

In the legacy model, each element property was a mutable wrapper (`TextElement`, `DateTimeElement`, `RichTextElement`, …) carrying a `.Value`. In the new model, the property *is* the value — a plain type for simple elements, or a small `*Value` record for the few that carry a companion field.

**Legacy:**
```csharp
var response = await client.GetLanguageVariantAsync<ArticleModel>(identifier);

response.Elements.Title = new TextElement { Value = "On Roasts - changed" };
response.Elements.PostDate = new DateTimeElement { Value = new DateTime(2018, 7, 4) };

var saved = await client.UpsertLanguageVariantAsync(identifier, response.Elements);
```

**New:**
```csharp
var result = await client.GetLanguageVariantAsync<Article>(identifier);

// The model is an immutable record — produce a new one with `with` rather than mutating in place.
var edited = result.Value.Elements with
{
    Title = "On Roasts - changed",
    PublishingDate = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero)
};

var saved = await client.UpsertLanguageVariantAsync(identifier, edited);
```

Key differences:

- **Property types.** `string` for text, `decimal?` for number, `IEnumerable<Reference>` for linked items / taxonomy / subpages, `IEnumerable<AssetReference>` for assets. The ones that carry an extra field use a record: `RichTextValue` (HTML + inline `Components`), `DateTimeValue` (instant + `DisplayTimeZone`), `UrlSlugValue` (slug + `Mode`), `CustomValue` (value + `SearchableValue`). Each has an implicit conversion for the common case, so `Slug = "on-roasts"` and `PublishingDate = new DateTimeOffset(...)` work directly.
- **Immutability.** Properties are `init`-only. Use a `with` expression (or construct a fresh record) instead of assigning to `result.Value.Elements.X`.
- **`RichTextElement` → `RichTextValue`** as the strongly-typed property type. (The *raw* element kind named `RichTextElement` still exists for the untyped path — see [§5](#5-authoring-elements-without-a-generated-model).)

> [!IMPORTANT]
> Date & time properties take a `DateTimeOffset`, not a `DateTime`. The value is stored as a UTC instant; whatever offset you supply is normalized to UTC on the wire. `DisplayTimeZone` is only a hint for how the editor renders it.

### 4.2 The generic get/upsert return shape

The generic overloads return `IManagementResult<LanguageVariantModel<T>>`. The strongly-typed elements live on `.Value.Elements`; the variant metadata (item, language, workflow, schedule, last-modified, contributors, …) sits beside it on `.Value`, mirroring the untyped `LanguageVariantModel`.

```csharp
var result = await client.GetLanguageVariantAsync<Article>(identifier);
LanguageVariantModel<Article> variant = result.Value;

Article elements = variant.Elements;
Reference item = variant.Item;
DateTime lastModified = variant.LastModified;
```

> [!TIP]
> The model generator's Management-model output is in active development. Until it ships, hand-write the records or use the typed raw elements in [§5](#5-authoring-elements-without-a-generated-model).

---

## 5. Authoring Elements Without a Generated Model

When you don't have a generated content type, you previously had anonymous `dynamic[]` payloads or `ElementBuilder.GetElementsAsDynamic(...)`. Both — and `ElementBuilder` itself — are **removed**. The replacement is a typed set of `BaseElement` records.

**Legacy:**
```csharp
var elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
{
    new TextElement
    {
        Element = Reference.ByCodename("title"),
        Value = "On Roasts - changed"
    },
    new DateTimeElement
    {
        Element = Reference.ByCodename("post_date"),
        Value = new DateTime(2018, 7, 4)
    },
});

var upsertModel = new LanguageVariantUpsertModel { Elements = elements };
await client.UpsertLanguageVariantAsync(identifier, upsertModel);
```

**New:**
```csharp
using Kontent.Ai.Management.Models.LanguageVariants.Elements;

await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
{
    Elements =
    [
        new TextElement { Element = Reference.ByCodename("title"), Value = "On Roasts - changed" },
        new DateTimeElement
        {
            Element = Reference.ByCodename("post_date"),
            Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero)
        }
    ]
});
```

`Elements` is now typed as `IReadOnlyList<BaseElement>` — no `ElementBuilder` wrapping step, no `dynamic`. There's one record per element kind: `TextElement`, `NumberElement`, `DateTimeElement`, `MultipleChoiceElement`, `AssetElement`, `LinkedItemsElement`, `TaxonomyElement`, `SubpagesElement`, `UrlSlugElement`, `CustomElement`, `RichTextElement`. Each pairs an `Element` reference with a value typed for that kind. Omitted elements are left unchanged.

> [!NOTE]
> These raw `*Element` records (which carry their own `Element` reference) are a **separate family** from the strongly-typed `*Value` records in [§4](#4-language-variants-and-strongly-typed-models) (which are just the value, because a generated record's property already identifies the element). Pick the family that matches your authoring path; you don't mix them.

For an element kind the SDK doesn't model — or to replay raw JSON you fetched — use `DynamicElement`, whose `Value` is written to the wire as-is:

```csharp
new DynamicElement { Element = Reference.ByCodename("widget"), Value = "<opaque payload>" }
```

---

## 6. Rich Text and Inline Components

Rich text is still authored as an HTML string. Previously you hand-wrote each inline `<object>` placeholder and kept it in sync with a matching entry in the `Components` array — error-prone because both sides shared a GUID you minted yourself.

**Legacy:**
```csharp
var componentId = "04bc8d32-97ab-431a-abaa-83102fc4c198";

BodyCopy = new RichTextElement
{
    Value = $"<p>Rich Text</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{componentId}\"></object>",
    Components = new ComponentModel[]
    {
        new ComponentModel
        {
            Id = Guid.Parse(componentId),
            Type = Reference.ByCodename("article"),
            Elements = new dynamic[] { /* … */ }
        }
    }
};
```

**New** — `RichTextBuilder` mints the GUID, emits the placeholder, and records the component for you:
```csharp
using Kontent.Ai.Management.Models.Content;

var rt = new RichTextBuilder();

var content = rt.Build($"""
    <h1>On Roasts</h1>
    <p>See {rt.ItemLink(Reference.ByCodename("intro"), "the introduction")} first.</p>
    {rt.Component(new Callout { Type = [CalloutType.Warning] })}
    {rt.Asset(new AssetReference { Codename = "roasting_chart" })}
    """);

var article = new Article { Title = "On Roasts", Content = content };
await client.UpsertLanguageVariantAsync(identifier, article);
```

`Build` returns a `RichTextValue` (the verbatim HTML plus the recorded components). The helpers — `Component(...)`, `LinkedItem(...)`, `ItemLink(...)`, `Asset(...)` — emit the right markup and keep the `components` array consistent; interpolation records them left-to-right.

> [!NOTE]
> The HTML is passed through verbatim — the builder does not sanitize or validate markup; it is intended for trusted, code-authored content such as migration scripts. Helper attribute values and link text are HTML-encoded.

---

## 7. Assets

Asset creation kept the two-step shape (upload file → create asset) but dropped the generic `AssetCreateModel<T>` and `ElementBuilder`, and gained a one-call convenience extension.

**Legacy:**
```csharp
var fileResult = await client.UploadFileAsync(new FileContentSource(stream, "hello.txt", "text/plain"));

var taxonomyElements = ElementBuilder.GetElementsAsDynamic(
    new TaxonomyElement
    {
        Element = Reference.ByCodename("taxonomy-categories"),
        Value = new[] { Reference.ByCodename("hello"), Reference.ByCodename("sdk") }
    });

var asset = new AssetCreateModel<AssetMetadataModel> { FileReference = fileResult, Elements = /* … */ };
await client.CreateAssetAsync(asset);
```

**New** — one call uploads the file and builds the asset around the resulting `FileReference`:
```csharp
using Kontent.Ai.Management.Extensions;

var result = await client.CreateAssetAsync(
    new FileContentSource(stream, "hello.txt", "text/plain"),
    fileReference => new AssetCreateModel
    {
        FileReference = fileReference,
        Title = "Hello",
        Elements = new[]
        {
            new AssetTaxonomyElement
            {
                Element = Reference.ByCodename("taxonomy-categories"),
                Value = new[] { Reference.ByCodename("hello"), Reference.ByCodename("sdk") }
            }
        }
    });
```

`AssetCreateModel` is no longer generic; `Elements` is a typed list. If you need to separate the steps (e.g. reuse one uploaded file across assets), call `UploadFileAsync` then `CreateAssetAsync(AssetCreateModel)` yourself. The matching `UpsertAssetAsync(identifier, FileContentSource, AssetUpsertModel)` overload does create-or-update; if the upload fails, no asset is created and the upload's failure is returned.

> [!NOTE]
> The two former asset-reference types were collapsed onto a single `AssetReference`. To leave an element's renditions unchanged, leave `Renditions` `null`; `[]` removes them. See [§10](#10-model-and-dto-changes).

---

## 8. Serialization: Newtonsoft → System.Text.Json

The SDK now serializes with `System.Text.Json`. Newtonsoft.Json is gone from the main SDK package. This is invisible for ordinary use, but breaks any code that:

- Implemented a custom `Newtonsoft.Json.JsonConverter` for an SDK model.
- Annotated your own models with `[JsonProperty]` / `[JsonIgnore]` from `Newtonsoft.Json` expecting the SDK to honor them — use the `System.Text.Json.Serialization` equivalents (`[JsonPropertyName]`, `[JsonIgnore]`) instead.
- Round-tripped SDK DTOs through `JsonConvert` and relied on Newtonsoft-specific behavior.

The DTOs are now immutable records with `[JsonPropertyName]` attributes, designed to be friendly to the Management-model generator. If you persisted SDK models as JSON with Newtonsoft, re-verify the serialized shape after upgrading.

---

## 9. Resilience and the HTTP Pipeline

The legacy hand-rolled HTTP composition was replaced by a Refit interface plus a `Microsoft.Extensions.Http.Resilience` (Polly) pipeline. For most consumers this is internal. What's visible:

- **Built-in resilience is on by default** — retries on transient failures and `429`, exponential backoff with jitter, `Retry-After` handling. Set `EnableResilience = false` to make it a passthrough.
- **Replace the pipeline** via the `configureResilience` hook on the DI overload, or `WithResilience(...)` on the builder:

```csharp
services.AddManagementClient(
    options => { options.EnvironmentId = "…"; options.ApiKey = "…"; },
    configureHttpClient: null,
    configureResilience: pipeline => pipeline
        .AddRetry(new HttpRetryStrategyOptions { MaxRetryAttempts = 5 })
        .AddTimeout(TimeSpan.FromSeconds(30)));
```

> [!NOTE]
> Unlike the sibling Delivery and Sync SDKs, the Management pipeline has **no default per-attempt timeout** — asset and file uploads can legitimately run long, and a blind retry would just re-upload. Add one via the hooks above if you need it.

If you had a custom Polly v7 policy or `Polly.Extensions.Http` setup wrapping the legacy client, port it to the `configureResilience` callback (Polly v8 `ResiliencePipelineBuilder`).

---

## 10. Model and DTO Changes

The DTO modernization brought every public DTO into agreement with the actual Management API v2 contract. Three kinds of change affect callers:

- **`required` members** — properties the API always returns or always demands now carry `required`. Object-initializer construction must set them, or you get a compile error.
- **Nullability** — properties whose wire value is genuinely optional changed from `T` to `T?`. Existing writes are unaffected; reads may now need a null check.
- **Renames / removals / type changes** — a focused set, each with a rationale.

The highlights you're most likely to hit:

### 10.1 Renames

| Old | New | Notes |
|-----|-----|-------|
| `ContentGroupModel.CodeName` | `ContentGroupModel.Codename` | Wire key unchanged. |
| `CollectionRemovePatchModel.CollectionIdentifier` | `CollectionRemovePatchModel.Reference` | Aligns with every other patch op. |
| `RichTextElementMetadataModel.ImageWidth` / `.ImageHeight` | `.ImageWidthLimit` / `.ImageHeightLimit` | Wire keys unchanged. |
| `UserModel.CollectionGroup` / `UserInviteModel.CollectionGroup` | `…CollectionGroups` | Plural matches the `IEnumerable<>`. |
| `WorkflowPublishedStepUpsertModel.RoleCreateNewVersionIds` | `.CreateNewVersionRoleIds` | Word-order fix. |
| `WorkflowPublishedStepUpsertModel.RolesUnpublishArchivedCancelSchedulingIds` | `.UnpublishRoleIds` | Name was inaccurate. |
| `ContentTypeSnippetPatchReplaceModel` / `…MoveModel` / `…RemoveModel` (and the content-type equivalents) | `ContentModelReplacePatchModel` / `…MovePatchModel` / `…RemovePatchModel` / `…AddIntoPatchModel` | Content-type and snippet patch ops now share one `ContentModelOperationBaseModel` hierarchy; build them via the `ContentTypePatch` / `ContentTypeSnippetPatch` factories rather than constructing the records directly. |
| `SpaceModel.WebSpotlightRootItem` / `SpaceCreateModel.WebSpotlightRootItem` | `.RootItem` | Wire key `web_spotlight_root_item` → `root_item`. |
| `SubscriptionColletionGroupModel` | `SubscriptionCollectionGroupModel` | Misspelling. |
| `SubscriptionUserRoleLangaugeModel` | `SubscriptionUserRoleLanguageModel` | Misspelling. |

### 10.2 Removals

- **`AssetWithRenditionsReference`** (and its converter) — collapsed onto `Models.Content.AssetReference`. A rendition is now a `RenditionReference` (`id` / `external_id` only — renditions have no codename). Migrate `new AssetWithRenditionsReference(Reference.ById(asset), Reference.ById(rendition))` to `new AssetReference { Id = asset, Renditions = [new RenditionReference { Id = rendition }] }`. Leave `Renditions` `null` to keep existing renditions, `[]` to remove them.
- **`ContentItemUpsertModel.ExternalId`** — the upsert addresses the item by external ID in the URL path; the body field was silently ignored.

### 10.3 Default-value and type-change footguns

- `ElementDefaultValue<TContainer, TValue>` collapsed to `ElementDefaultValue<TValue>`, and `Global` / `ElementDefaultValueEnvelope.Value` became `required`. Constructing an empty default-value object used to silently produce `{ "global": { "value": 0 } }`; the API now rejects an empty/null default. To express "no default", leave the parent element's `DefaultValue` as `null` — don't construct an empty default-value model.
- Several `bool` flags became `bool?` so that omission lets the server apply its default instead of silently sending `false`: `WebhookCreateModel.Enabled` and `MarkAsProductionModel.EnableWebhooks`.
- `SubscriptionUserRoleLanguageModel.IsActive` changed from `string` to `bool` (the API sends it as a quoted string; an internal converter now exposes a real `bool`).
- `last_modified` is now `required DateTime` (was `DateTime?`) across response models — it is always populated.

> [!TIP]
> If you build DTOs with object initializers, the compiler is your migration tool here: turn the project on the new package and fix each "required member must be set" and nullability warning the compiler surfaces.

---

## 11. Removed Features and Types

| Removed | Replacement |
|---------|-------------|
| `ManagementException` as control flow (thrown on `4xx`/`5xx`) | Result pattern — `IsSuccess` / `Value` / `Error`. `EnsureSuccess()` opts back into throwing ([§2](#2-response-handling-exceptions--result-pattern)). |
| `ElementBuilder.GetElementsAsDynamic(...)` | Typed `IReadOnlyList<BaseElement>`; `DynamicElement` for unmodeled kinds ([§5](#5-authoring-elements-without-a-generated-model)). |
| Anonymous `dynamic[]` element arrays | Same as above. |
| `IListingResponseModel<T>.HasNextPage()` / `GetNextPage()` / `GetAllAsync()` | Materialized `List…Async` + streaming `Enumerate…PagesAsync` ([§3](#3-listings-manual-paging--materialized-results)). |
| Generic `AssetCreateModel<T>` | Non-generic `AssetCreateModel` with typed `Elements` ([§7](#7-assets)). |
| Newtonsoft.Json (custom converters, `[JsonProperty]`) | `System.Text.Json` ([§8](#8-serialization-newtonsoft--systemtextjson)). |
| Web Spotlight activation: `ActivateWebSpotlightAsync`, `DeactivateWebSpotlightAsync`, `GetWebSpotlightStatusAsync`, `WebSpotlightModel`, `WebSpotlightActivateModel` | Live preview (preview-configuration endpoints). The `root_item` field on Spaces is retained. |
| `Kontent.Ai.Management.Helpers` (`EditLinkBuilder`, `IEditLinkBuilder`, `ElementIdentifier`) | No replacement in the modernized package. Keep equivalent edit-link logic in application code if you still need it. |
| Legacy hand-rolled HTTP (`ActionInvoker`, `ManagementHttpClient`, `EndpointUrlBuilder`) | Refit transport + `Microsoft.Extensions.Http.Resilience` ([§9](#9-resilience-and-the-http-pipeline)). |

---

## Troubleshooting

**`ManagementException` is never caught any more.**
Methods don't throw on API errors. Replace the `try/catch` with an `if (!result.IsSuccess)` branch, or call `.EnsureSuccess()` to opt back into throwing ([§2](#2-response-handling-exceptions--result-pattern)).

**`response.Item.Title` no longer compiles.**
Content moved behind the result: `result.Value.…`. For strongly-typed variants, element properties are now plain values / `*Value` records, so `result.Value.Elements.Title` is a `string`, not a `TextElement` ([§4](#4-language-variants-and-strongly-typed-models)).

**`ElementBuilder` / `GetElementsAsDynamic` doesn't exist.**
Build a typed `BaseElement[]` and assign it to `LanguageVariantUpsertModel.Elements` directly; use `DynamicElement` for kinds the SDK doesn't model ([§5](#5-authoring-elements-without-a-generated-model)).

**`HasNextPage()` / `GetNextPage()` / `GetAllAsync()` are gone.**
`List…Async` returns the whole set already merged. For large sets, stream pages with `Enumerate…PagesAsync` ([§3](#3-listings-manual-paging--materialized-results)).

**"Required member must be set" on an object initializer.**
The DTO modernization added `required` to properties the API always demands. Set them — the compiler lists exactly which ([§10](#10-model-and-dto-changes)).

**A read that used to be non-null is now `T?`.**
Nullability was corrected to match the wire contract; add the null check. The wire format is unchanged ([§10](#10-model-and-dto-changes)).

**Custom Newtonsoft converter no longer runs.**
Serialization is `System.Text.Json`. Port converters and replace `[JsonProperty]` with `[JsonPropertyName]` ([§8](#8-serialization-newtonsoft--systemtextjson)).

**`ActivateWebSpotlightAsync` doesn't exist.**
Web Spotlight activation is removed in favor of live preview (preview-configuration endpoints). The Spaces `root_item` field remains ([§11](#11-removed-features-and-types)).
