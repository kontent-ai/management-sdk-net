# Kontent.ai Management SDK for .NET

![Last commit][last-commit-shield]
[![Issues][issues-shield]][issues-url]
[![Contributors][contributors-shield]][contributors-url]
[![MIT License][license-shield]][license-url]
[![codecov][codecov-shield]][codecov-url]
[![NuGet][nuget-shield]][nuget-url]
[![Stack Overflow][stack-shield]](https://stackoverflow.com/tags/kontent-ai)

The official .NET SDK for the [Kontent.ai Management API](https://kontent.ai/learn/docs/apis/openapi/management-api-v2/) — programmatic read/write access to your Kontent.ai projects and environments: content items, language variants, content models, assets, taxonomies, workflows, and more.

> [!IMPORTANT]
> The Management SDK is undergoing a ground-up modernization. This README documents the **modernized API** — a result-based return type for every operation, `IAsyncEnumerable` pagination, and `System.Text.Json` serialization. **Breaking changes are expected** until the next major version is released; the current stable NuGet package still exposes the previous API.

## Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [Creating the Client](#creating-the-client)
  - [With Dependency Injection](#with-dependency-injection)
  - [Standalone](#standalone)
  - [From Configuration](#from-configuration)
  - [Multiple Named Clients](#multiple-named-clients)
- [Configuration Options](#configuration-options)
- [The Result Pattern](#the-result-pattern)
- [Identifiers](#identifiers)
- [Content Items](#content-items)
- [Language Variants](#language-variants)
- [Pagination](#pagination)
- [Publishing](#publishing)
- [Assets](#assets)
- [Content Model](#content-model)
- [Strongly-Typed Models](#strongly-typed-models)
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

```csharp
var client = new ManagementClient(new ManagementOptions
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

### With Dependency Injection

Register the client on your `IServiceCollection`:

```csharp
services.AddManagementClient(options =>
{
    options.EnvironmentId = "<YOUR_ENVIRONMENT_ID>";
    options.ApiKey = "<YOUR_API_KEY>";
});
```

`IManagementClient` is then resolvable from the container — inject it into your own services. This is the recommended approach: the container owns the client's lifetime and its underlying `HttpClient` pipeline.

### Standalone

For scripts and simple consumers, construct the client directly:

```csharp
var client = new ManagementClient(new ManagementOptions
{
    EnvironmentId = "<YOUR_ENVIRONMENT_ID>",
    ApiKey = "<YOUR_API_KEY>"
});
```

A standalone `ManagementClient` owns its `HttpClient` instances — dispose it when you are done (it implements both `IDisposable` and `IAsyncDisposable`).

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

## Configuration Options

| Option | Required | Default | Description |
|--------|----------|---------|-------------|
| `EnvironmentId` | Yes | — | The GUID of your Kontent.ai environment. |
| `ApiKey` | Yes | — | A Management API key, or a Subscription API key for subscription-scoped endpoints. |
| `SubscriptionId` | No | — | The subscription GUID. Required only for subscription-scoped endpoints (such as user management). |
| `EnableResilience` | No | `true` | Toggles the built-in retry/backoff pipeline without uninstalling it. |
| `Endpoint` / `EndpointV2` | No | Production URLs | Override only when targeting non-production endpoints. |

## The Result Pattern

Every `IManagementClient` method returns an `IManagementResult` (for void operations) or an `IManagementResult<T>` (for operations that yield a value). The SDK **does not throw** on `4xx`/`5xx` responses or on transport failures — inspect the result instead:

```csharp
var result = await client.CreateContentItemAsync(new ContentItemCreateModel
{
    Name = "On Roasts",
    Codename = "on_roasts",
    Type = Reference.ByCodename("article")
});

if (!result.IsSuccess)
{
    Console.WriteLine($"Request failed ({result.StatusCode}): {result.Error?.Message}");

    foreach (var validationError in result.Error?.ValidationErrors ?? [])
    {
        Console.WriteLine(validationError.Message);
    }

    return;
}

var item = result.Value;
```

A result carries:

- `IsSuccess` — whether the operation succeeded.
- `Value` — the returned value, on success (`IManagementResult<T>` only).
- `Error` — the failure detail, on failure: `Message`, `RequestId`, `ErrorCode`, `ValidationErrors`, and the underlying `Exception` for transport-level failures.
- `StatusCode`, `RequestUrl`, `ResponseHeaders` — response diagnostics.

Exceptions are reserved for programmer errors (for example, a `null` argument) and invalid configuration — not for API errors.

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

A language variant is identified by the pairing of its content item and its language:

```csharp
var variantIdentifier = new LanguageVariantIdentifier(
    Reference.ByCodename("on_roasts"),
    Reference.ByCodename("en-US"));
```

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
    Type = Reference.ByCodename("article")
});

// Create or update
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

## Language Variants

A language variant holds the actual content for one language of a content item. Provide elements as anonymous objects — each element is located by its `codename`, `id`, or `external_id`; omitted elements are left unchanged:

```csharp
var identifier = new LanguageVariantIdentifier(
    Reference.ByCodename("on_roasts"),
    Reference.ByCodename("en-US"));

var result = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
{
    Elements = new object[]
    {
        new
        {
            element = new { codename = "title" },
            value = "On Roasts"
        },
        new
        {
            element = new { codename = "post_date" },
            value = new DateTime(2018, 7, 4)
        }
    }
});
```

Retrieve a single variant, or list every variant of an item:

```csharp
var variant = await client.GetLanguageVariantAsync(identifier);

var allVariants = await client.ListLanguageVariantsByItemAsync(Reference.ByCodename("on_roasts"));
// allVariants.Value is an IReadOnlyList<LanguageVariantModel>
```

For a strongly-typed alternative, see [Strongly-Typed Models](#strongly-typed-models).

## Pagination

Listing endpoints that page through a continuation token are exposed as `EnumerateXPagesAsync` methods returning an `IAsyncEnumerable` of pages. Each iteration is one HTTP request, and each page is itself a result:

```csharp
var items = new List<ContentItemModel>();

await foreach (var page in client.EnumerateContentItemPagesAsync())
{
    if (!page.IsSuccess)
    {
        Console.WriteLine($"Failed to fetch a page: {page.Error?.Message}");
        break;
    }

    items.AddRange(page.Value);
}
```

The next page is fetched only when you iterate past the current one, so breaking early leaves later pages unrequested.

## Publishing

```csharp
var identifier = new LanguageVariantIdentifier(
    Reference.ByCodename("on_roasts"),
    Reference.ByCodename("en-US"));

// Publish now
await client.PublishLanguageVariantAsync(identifier);

// Schedule publishing
await client.SchedulePublishingOfLanguageVariantAsync(identifier, new ScheduleModel
{
    ScheduleTo = DateTime.Parse("2038-01-19T04:14:08"),
    DisplayTimeZone = "Europe/London"
});

// Unpublish, and create a new draft version of a published variant
await client.UnpublishLanguageVariantAsync(identifier);
await client.CreateNewVersionOfLanguageVariantAsync(identifier);
```

## Assets

Creating an asset is a two-step process: upload the binary file, then create the asset that references it.

```csharp
var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world"));

// Upload the binary file
var fileResult = await client.UploadFileAsync(new FileContentSource(stream, "hello.txt", "text/plain"));

// Create the asset, optionally assigning taxonomy terms
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

## Content Model

Content types, snippets, and taxonomy groups can all be created and modified through the SDK:

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
            Codename = "title"
        },
        new RichTextElementMetadataModel
        {
            Name = "Body",
            Codename = "body"
        }
    }
});
```

Existing types are changed with a list of patch operations via `ModifyContentTypeAsync`, and the full set is enumerated with `EnumerateContentTypePagesAsync`.

## Strongly-Typed Models

Instead of anonymous element objects, you can work with strongly-typed records that mirror your content types. Pass a generated model directly to `UpsertLanguageVariantAsync` — only the properties you set are sent:

```csharp
var identifier = new LanguageVariantIdentifier(
    Reference.ByCodename("on_roasts"),
    Reference.ByCodename("en-US"));

var article = new Article
{
    Title = "On Roasts",
    PublishingDate = new DateTime(2018, 7, 4)
};

var result = await client.UpsertLanguageVariantAsync(identifier, article);
```

Retrieve a variant the same way with the generic overload:

```csharp
var result = await client.GetLanguageVariantAsync<Article>(identifier);
Article variant = result.Value;
```

> [!TIP]
> You don't have to hand-write these models. The [**Kontent.ai model generator**](https://github.com/kontent-ai/model-generator-net) generates strongly-typed records from your content model. Management-model generation is currently in active development — watch the repository for its release.

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
