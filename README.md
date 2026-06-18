# Kontent.ai Management SDK for .NET

![Last commit][last-commit-shield]
[![Issues][issues-shield]][issues-url]
[![Contributors][contributors-shield]][contributors-url]
[![MIT License][license-shield]][license-url]
[![codecov][codecov-shield]][codecov-url]
[![NuGet][nuget-shield]][nuget-url]
[![Stack Overflow][stack-shield]](https://stackoverflow.com/tags/kontent-ai)

The official .NET SDK for the [Kontent.ai Management API](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/) — programmatic read/write access to your Kontent.ai projects and environments: content items, language variants, content models, assets, taxonomies, workflows, environments, and more.

> [!IMPORTANT]
> The Management SDK is undergoing a ground-up modernization. This README documents the **modernized API** — a result-based return type for every operation, materialized listings, `System.Text.Json` serialization, and strongly-typed content models. **Breaking changes are expected** until the next major version is released; the current stable NuGet package still exposes the previous API.

## Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [Creating the Client](#creating-the-client)
  - [With Dependency Injection](#with-dependency-injection)
  - [Standalone](#standalone)
  - [Fluent Builder](#fluent-builder)
  - [From Configuration](#from-configuration)
  - [Multiple Named Clients](#multiple-named-clients)
  - [Resilience and the HTTP Pipeline](#resilience-and-the-http-pipeline)
- [Configuration Options](#configuration-options)
- [The Result Pattern](#the-result-pattern)
- [Error Handling](#error-handling)
- [Identifiers](#identifiers)
- [Listings](#listings)
- [Content Items](#content-items)
- [Language Variants](#language-variants)
- [Strongly-Typed Models](#strongly-typed-models)
- [Publishing and Scheduling](#publishing-and-scheduling)
- [Assets](#assets)
- [Content Model](#content-model)
- [Workflows](#workflows)
- [Environment and Administration](#environment-and-administration)
- [Further Information](#further-information)
- [Contributing](#contributing)
- [License](#license)

## Installation

Install the SDK via the NuGet Package Manager:

```bash
dotnet add package Kontent.Ai.Management
```

The SDK targets `net8.0`.

## Quick Start

The fastest path to a first call — a standalone client, ideal for scripts and simple apps. For applications, prefer [dependency injection](#with-dependency-injection).

```csharp
using Kontent.Ai.Management;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Shared;

await using var client = new ManagementClient(new ManagementOptions
{
    EnvironmentId = "<YOUR_ENVIRONMENT_ID>",
    ApiKey = "<YOUR_API_KEY>"
});

var result = await client.GetContentItemAsync(Reference.ByCodename("on_roasts"));

if (result.IsSuccess)
{
    Console.WriteLine(result.Value.Name);
}
else
{
    Console.WriteLine($"{result.StatusCode}: {result.Error?.Message}");
}
```

> [!NOTE]
> The Management API must be activated for your environment, and you need a Management API key. See [Making requests](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/#section/Making-requests).

## Creating the Client

There are three entry points, in order of preference: **dependency injection** for applications, the **standalone constructor** for scripts and simple consumers, and the **fluent builder** when a non-DI consumer needs to customize the HTTP pipeline.

### With Dependency Injection

Register the client on your `IServiceCollection`:

```csharp
services.AddManagementClient(options =>
{
    options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
    options.ApiKey = "<YOUR_API_KEY>";
});
```

`IManagementClient` is then resolvable from the container — inject it into your own services. This is the recommended approach: the container owns the client's lifetime and its underlying `HttpClient` pipeline (via `IHttpClientFactory`), and reacts to configuration reloads.

> [!NOTE]
> A DI-resolved client is owned by the container — do **not** dispose it yourself. Disposal is a no-op on DI-managed instances; the container releases the underlying HTTP resources.

### Standalone

For scripts and simple consumers, construct the client directly:

```csharp
await using var client = new ManagementClient(new ManagementOptions
{
    EnvironmentId = "<YOUR_ENVIRONMENT_ID>",
    ApiKey = "<YOUR_API_KEY>"
});
```

A standalone client owns its `HttpClient` instances — dispose it when you are done (it implements both `IDisposable` and `IAsyncDisposable`, hence the `await using` above).

### Fluent Builder

When you are **not** using DI but still need to customize the resilience pipeline or Refit settings, use `ManagementClientBuilder`:

```csharp
await using var client = ManagementClientBuilder
    .WithOptions(options =>
    {
        options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
        options.ApiKey = "<YOUR_API_KEY>";
    })
    .WithResilience(pipeline => /* customize the Polly pipeline */ pipeline.AddTimeout(TimeSpan.FromSeconds(30)))
    .Build();
```

The builder is a thin wrapper over the resource-owning constructor — it does not spin up a private service provider. The built client owns its HTTP resources, so dispose it as with the standalone constructor.

### From Configuration

Bind the options from an `IConfiguration` section — `ManagementOptions` by default:

```json
{
  "ManagementOptions": {
    "EnvironmentId": "<YOUR_ENVIRONMENT_ID>",
    "ApiKey": "<YOUR_API_KEY>"
  }
}
```

```csharp
services.AddManagementClient(configuration);
// or bind a differently-named section:
services.AddManagementClient(configuration, "MyManagementSection");
```

### Multiple Named Clients

Register more than one client by giving each a unique name:

```csharp
services.AddManagementClient("production", options =>
{
    options.EnvironmentId = "<PRODUCTION_ENVIRONMENT_ID>";
    options.ApiKey = "<PRODUCTION_API_KEY>";
});

services.AddManagementClient("staging", options =>
{
    options.EnvironmentId = "<STAGING_ENVIRONMENT_ID>";
    options.ApiKey = "<STAGING_API_KEY>";
});
```

Resolve a named client through `IManagementClientFactory`:

```csharp
public class ContentMigrator(IManagementClientFactory clientFactory)
{
    public async Task RunAsync()
    {
        var production = clientFactory.Get("production");
        var staging = clientFactory.Get("staging");
        // ...
    }
}
```

### Resilience and the HTTP Pipeline

Every client comes with a built-in resilience pipeline (powered by [`Microsoft.Extensions.Http.Resilience`](https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience)): retries on transient failures and `429` responses, exponential backoff with jitter, and `Retry-After` handling. Set `EnableResilience = false` to turn it into a passthrough.

To replace the pipeline wholesale, use the `configureResilience` hook on the DI overload, or `WithResilience(...)` on the builder:

```csharp
services.AddManagementClient(
    options => { options.EnvironmentId = "..."; options.ApiKey = "..."; },
    configureHttpClient: null,
    configureResilience: pipeline => pipeline
        .AddRetry(new HttpRetryStrategyOptions { MaxRetryAttempts = 5 })
        .AddTimeout(TimeSpan.FromSeconds(30)));
```

> [!NOTE]
> Unlike the sibling Delivery and Sync SDKs, the Management pipeline has **no default per-attempt timeout** — asset and file uploads can legitimately run long, and a blind retry would just re-upload. Add one via the hooks above if you need it.

## Configuration Options

| Option | Required | Default | Description |
|--------|----------|---------|-------------|
| `EnvironmentId` | Yes | — | The GUID of your Kontent.ai environment. |
| `ApiKey` | Yes | — | A Management API key, or a Subscription API key for subscription-scoped endpoints. |
| `SubscriptionId` | No | — | The subscription GUID. Required only for subscription-scoped endpoints (such as user management). |
| `EnableResilience` | No | `true` | Toggles the built-in retry/backoff pipeline without uninstalling it. |
| `EndpointV2` | No | Production URL | Override only when targeting a non-production endpoint. |

`ManagementOptions` validates on use: a missing or malformed `EnvironmentId`/`ApiKey` surfaces as a `ValidationException` from the constructor/builder, or an `OptionsValidationException` at provider-build time in DI.

## The Result Pattern

Every `IManagementClient` method returns an `IManagementResult` (for void operations) or an `IManagementResult<T>` (for operations that yield a value). The SDK **does not throw** on `4xx`/`5xx` responses or on transport failures — inspect the result instead:

```csharp
var result = await client.CreateContentItemAsync(new ContentItemCreateModel
{
    Name = "On Roasts",
    Codename = "on_roasts",
    Type = Reference.ByCodename("article")
});

if (result.IsSuccess)
{
    ContentItemModel item = result.Value;
}
```

A result carries:

- `IsSuccess` — whether the operation succeeded.
- `Value` — the returned value, on success (`IManagementResult<T>` only).
- `Error` — the failure detail, on failure (see [Error Handling](#error-handling)).
- `StatusCode`, `RequestUrl` — response diagnostics. `StatusCode` is `null` when the failure happened before or without an HTTP response (local validation or a transport error).

For call sites that would rather not branch, two opt-in conveniences live on the result:

```csharp
// Throw on failure (the thrown ManagementException carries the IError) and get the value:
var item = (await client.GetContentItemAsync(Reference.ByCodename("on_roasts"))).EnsureSuccess();

// Or the Try pattern:
if ((await client.GetContentItemAsync(Reference.ByCodename("on_roasts"))).TryGetValue(out var value))
{
    Console.WriteLine(value.Name);
}
```

The SDK itself never throws `ManagementException` — it surfaces only when you opt in with `EnsureSuccess()`.

## Error Handling

On failure, `result.Error` (an `IError`) describes what went wrong:

```csharp
var result = await client.CreateContentItemAsync(model);

if (!result.IsSuccess)
{
    Console.WriteLine($"Request failed ({result.StatusCode}): {result.Error?.Message}");
    Console.WriteLine($"Request ID: {result.Error?.RequestId}");   // quote this when reporting an issue

    foreach (var validationError in result.Error?.ValidationErrors ?? [])
    {
        Console.WriteLine(validationError.Message);
    }

    return;
}
```

`IError` exposes `Message`, `RequestId`, `ErrorCode` (Kontent.ai's diagnostic code, not the HTTP status), `ValidationErrors`, and the underlying `Exception` for transport-level failures.

When you need to branch on a specific failure, compare `ErrorCode` against the `ManagementErrorCodes` catalog rather than a magic number:

```csharp
var result = await client.UpsertLanguageVariantAsync(identifier, article);

if (!result.IsSuccess && result.Error?.ErrorCode == ManagementErrorCodes.PublishedOrScheduledVariantCannotBeUpdated)
{
    // The variant is published; create a new version, then retry the edit.
    await client.CreateNewVersionOfLanguageVariantAsync(identifier);
    result = await client.UpsertLanguageVariantAsync(identifier, article);
}
```

`ManagementErrorCodes` is a curated set of the codes callers commonly act on — variant workflow-state conflicts, duplicate external IDs, concurrency, and rate limits. The codes are not unique (the API reuses some across unrelated conditions), so inspect `Message` as well when the distinction matters.

> [!IMPORTANT]
> Exceptions are reserved for **programmer errors** (for example, a `null` argument), **invalid configuration**, and **network/serialization failures** — not for API errors. A `404` or a validation rejection comes back as `IsSuccess == false`, never as a thrown exception.

## Identifiers

Most operations target an entity through a `Reference`, which can be built from a codename, an internal ID, or an external ID:

```csharp
var byCodename = Reference.ByCodename("on_roasts");
var byId = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
var byExternalId = Reference.ByExternalId("Ext-Item-456-Brno");
```

User-management endpoints use a `UserIdentifier`:

```csharp
var byEmail = UserIdentifier.ByEmail("user@example.com");
var byUserId = UserIdentifier.ById("usr_0vKjTCH2TkO687K3y3bKNS");
```

A language variant is identified by the pairing of its content item and its language. The `ByCodenames` / `ByIds` / `ByExternalIds` factories cover the common case; the constructor takes any two `Reference`s when you need to mix kinds:

```csharp
var variantIdentifier = LanguageVariantIdentifier.ByCodenames("on_roasts", "en-US");

// mix identifier kinds via the constructor:
var mixed = new LanguageVariantIdentifier(Reference.ById(itemId), Reference.ByCodename("en-US"));
```

> [!NOTE]
> Not every endpoint accepts every identifier kind — some are ID-only, some forbid external IDs. Passing an unsupported kind throws an `InvalidOperationException` before any request is sent.

## Listings

Listing endpoints return the whole set in one result — `Task<IManagementResult<IReadOnlyList<T>>>`. Continuation-token paging is handled internally: the SDK walks every page and merges them, so you never deal with pages yourself.

```csharp
var result = await client.ListContentItemsAsync();

if (!result.IsSuccess)
{
    Console.WriteLine($"Failed to list content items: {result.Error?.Message}");
    return;
}

foreach (var item in result.Value)   // result.Value is IReadOnlyList<ContentItemModel>
{
    Console.WriteLine(item.Name);
}
```

A listing is **all-or-nothing**: if any page fails, that first failure short-circuits and is returned as the result, so you never receive a silently truncated set.

> [!NOTE]
> Because every page is fetched and buffered before the result returns, a listing materializes the full set in memory. For the Management API's configuration data (types, languages, taxonomies, …) that's a non-issue.

### Streaming large listings

The endpoints whose results can grow large — content items, assets, and the items-with-variants filter and bulk-get — also expose an `EnumerateXPagesAsync` overload that streams one continuation-token page at a time, so you can process the listing without buffering it all in memory (and stop early). Each iteration is one HTTP request and yields a page result; a failed page surfaces as a failed result and ends the stream:

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

The next page is fetched only when you iterate past the current one, so breaking early leaves later pages unrequested. Reach for this only when a listing is genuinely large; everywhere else, `ListXAsync` is simpler.

## Content Items

A content item is the language-agnostic wrapper; the actual content lives in its [language variants](#language-variants).

```csharp
// Get
var item = await client.GetContentItemAsync(Reference.ByCodename("on_roasts"));

// Create
var created = await client.CreateContentItemAsync(new ContentItemCreateModel
{
    Name = "On Roasts",
    Codename = "on_roasts",
    Type = Reference.ByCodename("article"),
    Collection = Reference.ByDefaultCodename()   // optional
});

// Create or update by external ID
var upserted = await client.UpsertContentItemAsync(
    Reference.ByExternalId("59713"),
    new ContentItemUpsertModel
    {
        Name = "On Roasts",
        Type = Reference.ByCodename("article")
    });

// Delete
await client.DeleteContentItemAsync(Reference.ByCodename("on_roasts"));
```

To create an item and set its first variant in one call, use the `CreateContentItemWithVariantAsync` extension (a `<T>` overload takes a strongly-typed model instead):

```csharp
using Kontent.Ai.Management.Extensions;

var result = await client.CreateContentItemWithVariantAsync(
    new ContentItemCreateModel { Name = "On Roasts", Type = Reference.ByCodename("article") },
    Reference.ByCodename("en-US"),
    new LanguageVariantUpsertModel { Elements = [/* … */] });
```

> [!NOTE]
> This is a two-call composite. On a partial failure — the item is created but the variant upsert fails — the item is left in place (no rollback) and the returned failure carries the variant call's detail. Set an `ExternalId` on the item so a retry reuses it rather than creating a duplicate.

## Language Variants

> [!TIP]
> The most type-safe way to author a variant is a **[strongly-typed model](#strongly-typed-models)** — `await client.UpsertLanguageVariantAsync(id, typedModel)`. When you have generated content-type records that's the recommended path; the typed element records shown here cover the same ground without a generator and are the building block underneath.

A language variant holds the actual content for one language of a content item. Set its elements with a typed record per element kind — each locates its target element by `codename`, `id`, or `external_id` and carries a value shaped for that kind; omitted elements are left unchanged:

```csharp
using Kontent.Ai.Management.Models.LanguageVariants.Elements;

var identifier = LanguageVariantIdentifier.ByCodenames("on_roasts", "en-US");

var result = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
{
    Elements =
    [
        new TextElement { Element = Reference.ByCodename("title"), Value = "On Roasts" },
        new DateTimeElement { Element = Reference.ByCodename("post_date"), Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero) }
    ]
});
```

Retrieve a single variant, or list every variant of an item:

```csharp
var variant = await client.GetLanguageVariantAsync(identifier);

var allVariants = await client.ListLanguageVariantsByItemAsync(Reference.ByCodename("on_roasts"));
// allVariants.Value is an IReadOnlyList<LanguageVariantModel>
```

You can also enumerate variants across a whole collection, space, or content type — see the `ListLanguageVariantsByCollectionAsync`, `…BySpaceAsync`, and `…ByTypeAsync` methods.

> [!TIP]
> These typed element records are the type-safe way to author a variant without a generated content type. When you do have generated content-type records, pass one directly — see [strongly-typed models](#strongly-typed-models).

### Element kinds

There is one record per element kind — `TextElement`, `NumberElement`, `DateTimeElement`, `MultipleChoiceElement`, `AssetElement`, `LinkedItemsElement`, `TaxonomyElement`, `SubpagesElement`, `UrlSlugElement`, `CustomElement`, and `RichTextElement` — each pairing an `Element` reference with a value typed for that kind. Some carry more than a bare value:

```csharp
using Kontent.Ai.Management.Models.Content;            // UrlSlugMode
using Kontent.Ai.Management.Models.LanguageVariants.Elements;

Elements =
[
    new DateTimeElement
    {
        Element = Reference.ByCodename("post_date"),
        Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero),
        DisplayTimeZone = "Europe/Prague"
    },
    new UrlSlugElement { Element = Reference.ByCodename("slug"), Value = "on-roasts", Mode = UrlSlugMode.Custom }
]
```

For an element kind the SDK doesn't model — or to replay a variant you fetched as raw JSON — use `DynamicElement`, whose `Value` is written to the wire as-is:

```csharp
new DynamicElement { Element = Reference.ByCodename("widget"), Value = "<opaque payload>" }
```

## Strongly-Typed Models

Instead of anonymous element objects, you can work with strongly-typed records that mirror your content types. Pass a generated model directly to `UpsertLanguageVariantAsync` — only the properties you set are sent (partial update), and the record is validated locally before any HTTP call:

```csharp
var identifier = LanguageVariantIdentifier.ByCodenames("on_roasts", "en-US");

var article = new Article
{
    Title = "On Roasts",
    PublishingDate = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero)
};

var result = await client.UpsertLanguageVariantAsync(identifier, article);
```

Retrieve a variant the same way with the generic overload. The generic get and upsert return a `LanguageVariantModel<T>`: the strongly-typed `Elements` plus the variant metadata it shares with the untyped `LanguageVariantModel` — item, language, workflow, schedule, last-modified, due date, note, contributors (every property except `Elements`):

```csharp
var result = await client.GetLanguageVariantAsync<Article>(identifier);
LanguageVariantModel<Article> variant = result.Value;

Article elements = variant.Elements;   // the strongly-typed element values
// every other property is variant metadata, shared with the untyped LanguageVariantModel:
Reference item = variant.Item;
Reference language = variant.Language;
DateTime lastModified = variant.LastModified;
```

### Element value types

Most elements map to a plain CLR type — `string` for text, `decimal?` for number, `IEnumerable<Reference>` for linked items, taxonomy, and subpages, and `RichTextElement` for rich text. Three element kinds carry more than a bare value and use a small wrapper record:

```csharp
var article = new Article
{
    // Date & time — an instant plus an optional IANA zone for how the UI displays it
    PublishingDate = new DateTimeValue
    {
        Value = new DateTimeOffset(2018, 7, 4, 0, 0, 0, TimeSpan.Zero),
        DisplayTimeZone = "Europe/Prague"
    },

    // URL slug — the slug plus whether it is custom or regenerated from its source element
    Slug = new UrlSlugValue { Value = "on-roasts", Mode = UrlSlugMode.Custom },

    // Custom element — the opaque value plus the plaintext used for search and filtering
    Rating = new CustomValue { Value = "{\"stars\":5}", SearchableValue = "5 stars" }
};
```

Each wrapper has an implicit conversion for the common case, so `Slug = "on-roasts"` (custom mode) and `Rating = "{\"stars\":5}"` work when you don't need the extra field.

> [!IMPORTANT]
> A date & time value is stored as a **UTC instant**; `DisplayTimeZone` is only a hint for how the editor renders it and never changes the instant. The element accepts a `DateTimeOffset` (not a `DateTime`) so the moment is unambiguous — a bare `DateTime` would be resolved against the machine's local zone. Whatever offset you supply is normalized to UTC on the wire.

> [!TIP]
> You don't have to hand-write these models. The [**Kontent.ai model generator**](https://github.com/kontent-ai/model-generator-net) generates strongly-typed records from your content model. Management-model generation is currently in active development — watch the repository for its release.

## Publishing and Scheduling

```csharp
var identifier = LanguageVariantIdentifier.ByCodenames("on_roasts", "en-US");

// Publish now
await client.PublishLanguageVariantAsync(identifier);

// Schedule publishing
await client.SchedulePublishingOfLanguageVariantAsync(identifier, new ScheduleModel
{
    ScheduleTo = new DateTimeOffset(2038, 1, 19, 4, 14, 8, TimeSpan.Zero),
    DisplayTimeZone = "Europe/London"
});

// Unpublish, and create a new draft version of a published variant
await client.UnpublishLanguageVariantAsync(identifier);
await client.CreateNewVersionOfLanguageVariantAsync(identifier);

// Move a variant to a different workflow step
await client.ChangeLanguageVariantWorkflowAsync(identifier, new ChangeLanguageVariantWorkflowModel(
    workflow: Reference.ByDefaultCodename(),
    step: Reference.ByCodename("review")));
```

`SchedulePublishingAndUnpublishingOfLanguageVariantAsync` sets both ends of a publish window in one call.

## Assets

Creating an asset is a two-step process: upload the binary file, then create the asset that references it.

```csharp
var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world"));

// 1. Upload the binary file
var fileResult = await client.UploadFileAsync(new FileContentSource(stream, "hello.txt", "text/plain"));

// 2. Create the asset, optionally assigning taxonomy terms
var result = await client.CreateAssetAsync(new AssetCreateModel
{
    FileReference = fileResult.Value,
    Title = "Hello",
    Elements = new[]
    {
        new AssetElement
        {
            Element = Reference.ByCodename("taxonomy-categories"),
            Value = new[]
            {
                Reference.ByCodename("hello"),
                Reference.ByCodename("sdk")
            }
        }
    }
});
```

Assets are enumerated with `ListAssetsAsync`, updated with `UpsertAssetAsync`, and deleted with `DeleteAssetAsync`. Renditions (`CreateAssetRenditionAsync`, `ListAssetRenditionsAsync`) and the asset-folder hierarchy (`GetAssetFoldersAsync`, `CreateAssetFoldersAsync`, `ModifyAssetFoldersAsync`) are managed through their own methods.

## Content Model

Content types, snippets, and taxonomy groups can all be created and modified through the SDK. Elements of a content type are described by `ElementMetadataBase` subtypes — one per element kind (`TextElementMetadataModel`, `RichTextElementMetadataModel`, `NumberElementMetadataModel`, `AssetElementMetadataModel`, and so on):

```csharp
var result = await client.CreateContentTypeAsync(new ContentTypeCreateModel
{
    Name = "Article",
    Codename = "article",
    Elements = new ElementMetadataBase[]
    {
        new TextElementMetadataModel
        {
            Name = "Title",
            Codename = "title",
            IsRequired = true
        },
        new RichTextElementMetadataModel
        {
            Name = "Body",
            Codename = "body"
        }
    }
});
```

A taxonomy group is created with its terms (terms nest recursively):

```csharp
await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
{
    Name = "Categories",
    Codename = "categories",
    Terms = new[]
    {
        new TaxonomyTermCreateModel { Name = "Coffee", Codename = "coffee" },
        new TaxonomyTermCreateModel { Name = "Brewing", Codename = "brewing" }
    }
});
```

Content type snippets work the same way via `CreateContentTypeSnippetAsync` (a `ContentTypeSnippetCreateModel` carries `Name`, `Codename`, and `Elements`).

Existing types, snippets, and taxonomy groups are changed with a list of **patch operations** rather than a full replace:

```csharp
await client.ModifyContentTypeAsync(Reference.ByCodename("article"), new ContentTypeOperationBaseModel[]
{
    // add, remove, replace, move operations …
});
```

The full set of each is listed with `ListContentTypesAsync`, `ListContentTypeSnippetsAsync`, and `ListTaxonomyGroupsAsync`.

## Workflows

```csharp
// List
var workflows = await client.ListWorkflowsAsync();

// Create / update / delete
var created = await client.CreateWorkflowAsync(new WorkflowUpsertModel { /* steps, scopes … */ });
await client.UpdateWorkflowAsync(Reference.ByDefaultCodename(), updatedWorkflow);
await client.DeleteWorkflowAsync(Reference.ByCodename("editorial"));
```

## Environment and Administration

The client also covers environment-level configuration and administration. These follow the same result-pattern and identifier conventions as the sections above; the full request/response shapes are in the [Management API reference](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/).

| Area | Key methods |
|------|-------------|
| **Languages** | `GetLanguageAsync`, `CreateLanguageAsync`, `ModifyLanguageAsync`, `ListLanguagesAsync` |
| **Collections** | `ListCollectionsAsync`, `ModifyCollectionAsync` |
| **Spaces** | `ListSpacesAsync`, `GetSpaceAsync`, `CreateSpaceAsync`, `ModifySpaceAsync`, `DeleteSpaceAsync` |
| **Webhooks** | `ListWebhooksAsync`, `GetWebhookAsync`, `CreateWebhookAsync`, `EnableWebhookAsync`, `DisableWebhookAsync`, `DeleteWebhookAsync` |
| **Preview** | `GetPreviewConfigurationAsync`, `ModifyPreviewConfigurationAsync` |
| **Custom apps** | `ListCustomAppsAsync`, `GetCustomAppAsync`, `CreateCustomAppAsync`, `ModifyCustomAppAsync`, `DeleteCustomAppAsync` |
| **Roles** | `ListEnvironmentRolesAsync`, `GetEnvironmentRoleAsync` |
| **Environment users** | `InviteUserIntoEnvironmentAsync`, `ModifyUsersRolesAsync` |
| **Environment lifecycle** | `GetEnvironmentInformationAsync`, `CloneEnvironmentAsync`, `GetEnvironmentCloningStateAsync`, `MarkEnvironmentAsProductionAsync`, `ModifyEnvironmentAsync`, `DeleteEnvironmentAsync` |
| **Validation** | `ValidateEnvironmentAsync`, `InitiateEnvironmentAsyncValidationTaskAsync`, `GetAsyncValidationTaskAsync`, `ListAsyncValidationTaskIssuesAsync` |

For example, creating a language:

```csharp
await client.CreateLanguageAsync(new LanguageCreateModel
{
    Name = "German",
    Codename = "de-DE",
    IsActive = true,
    FallbackLanguage = Reference.ByCodename("en-US")
});
```

> [!NOTE]
> Subscription-scoped endpoints — `ListSubscriptionProjectsAsync`, `ListSubscriptionUsersAsync`, `GetSubscriptionUserAsync`, `ActivateSubscriptionUserAsync`, `DeactivateSubscriptionUserAsync` — require `SubscriptionId` to be set in the options and an API key with subscription scope.

## Further Information

For more developer resources, see the [Management API reference](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/) and the [.NET development overview](https://kontent.ai/learn/develop/develop-with-kontent-ai/net) on Kontent.ai Learn.

## Contributing

See the [contributing](./CONTRIBUTING.md) page for the best places to file issues, start discussions, and begin contributing.

## License

Distributed under the MIT License — see [`LICENSE.md`](./LICENSE.md) for details.

[last-commit-shield]: https://img.shields.io/github/last-commit/kontent-ai/management-sdk-net?style=for-the-badge
[issues-shield]: https://img.shields.io/github/issues/kontent-ai/management-sdk-net?style=for-the-badge
[issues-url]: https://github.com/kontent-ai/management-sdk-net/issues
[contributors-shield]: https://img.shields.io/github/contributors/kontent-ai/management-sdk-net?style=for-the-badge
[contributors-url]: https://github.com/kontent-ai/management-sdk-net/graphs/contributors
[license-shield]: https://img.shields.io/github/license/kontent-ai/management-sdk-net?style=for-the-badge
[license-url]: https://github.com/kontent-ai/management-sdk-net/blob/master/LICENSE.md
[codecov-shield]: https://img.shields.io/codecov/c/github/kontent-ai/management-sdk-net?style=for-the-badge
[codecov-url]: https://codecov.io/gh/kontent-ai/management-sdk-net
[nuget-shield]: https://img.shields.io/nuget/v/Kontent.Ai.Management?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/Kontent.Ai.Management
[stack-shield]: https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16?style=for-the-badge&logo=stackoverflow&logoColor=white
