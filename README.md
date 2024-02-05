# Kontent.ai Management .NET SDK

[![Build & Test](https://github.com/kontent-ai/management-sdk-net/actions/workflows/integrate.yml/badge.svg)](https://github.com/kontent-ai/management-sdk-net/actions/workflows/integrate.yml)
[![codecov](https://codecov.io/gh/kontent-ai/management-sdk-net/branch/master/graph/badge.svg?token=xhM2JrUuA4)](https://codecov.io/gh/kontent-ai/management-sdk-net)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kontent-ai)
[![Discord](https://img.shields.io/discord/821885171984891914?color=%237289DA&label=Kontent.ai%20Discord&logo=discord)](https://discord.gg/SKCxwPtevJ)

| Package                       |                                                                    Version                                                                    |                                                                  Downloads                                                                  |                        Compatibility                         |           Documentation           |
| ----------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------: | :-------------------------------: |
| Management SDK                |         [![NuGet](https://img.shields.io/nuget/vpre/Kontent.Ai.Management.svg)](https://www.nuget.org/packages/Kontent.Ai.Management)         |         [![NuGet](https://img.shields.io/nuget/dt/Kontent.Ai.Management.svg)](https://www.nuget.org/packages/Kontent.Ai.Management)         | [`net8.0`](https://dotnet.microsoft.com/download/dotnet/8.0) | [📖](#using-the-managementclient) |
| Content Item Edit-URL Builder | [![NuGet](https://img.shields.io/nuget/vpre/Kontent.Ai.Management.Helpers.svg)](https://www.nuget.org/packages/Kontent.Ai.Management.Helpers) | [![NuGet](https://img.shields.io/nuget/dt/Kontent.Ai.Management.Helpers.svg)](https://www.nuget.org/packages/Kontent.Ai.Management.Helpers) | [`net8.0`](https://dotnet.microsoft.com/download/dotnet/8.0) |       [📖](#helper-methods)       |

## Summary

The Kontent.ai Management .NET SDK is a client library used for managing content in Kontent.ai. It provides read/write access to your Kontent.ai projects.

You can use the SDK in the form of a [NuGet package](https://www.nuget.org/packages/Kontent.Ai.Management) to migrate existing content into your Kontent.ai project or update your content model.

The Management SDK does not provide any content filtering options and is not optimized for content delivery. If you need to deliver larger amounts of content we recommend using the [Delivery SDK](https://github.com/kontent-ai/delivery-sdk-net) instead.

💡 If you want to see all .NET related resources including REST API reference with .NET code samples for every endpoint, check out the ["Develop .NET apps" overview page](https://kontent.ai/learn/tutorials/develop-apps/overview?tech=dotnet).

## Prerequisites

To manage content in a Kontent.ai project via the Management API, you first need to activate the API for the project. See our documentation on how you can [activate the Management API](https://kontent.ai/learn/tutorials/set-up-kontent/import-content/overview#a-management-api).

## Using the ManagementClient

The `ManagementClient` class is the main class of the SDK. Using it, you can import, update, view and delete content items, language variants, and other objects in your Kontent.ai projects.

To create an instance of the class, you need to provide:

- [ProjectId](https://kontent.ai/learn/tutorials/develop-apps/get-content/get-content-items#a-1-find-your-project-id): the ID of your Kontent.ai project. This parameter must always be set.
- [SubscriptionId](https://kontent.ai/learn/reference/management-api-v2/#tag/Subscription): the ID of your subscription. Set it up if you need to manage users and their permissions.
- [ApiKey](https://kontent.ai/learn/reference/management-api-v2/#section/Authentication/API-keys): either Management or Subscription API key.
  - Subscription API key can be used for all endpoints but is limited to subscription admins
  - Management API key can be used with project-specific endpoints and is limited to users with the Manage APIs permission.

```csharp
// Initializes an instance of the ManagementClient client with specified options.
var client = new ManagementClient(new ManagementOptions
{
    ProjectId = "cbbe2d5c-17c6-0128-be26-e997ba7c1619",
    SubscriptionId = "a27b9841-fc99-48a7-a46d-65b2549d6c0"
    ApiKey = "ew0...1eo"
});
```

Once you create a `ManagementClient`, you can start managing content in your project by calling methods on the client instance.

### Codename vs. ID vs. External ID

The SDK uses an _Reference_ object representation identifying an entity you want to perform the given operation on. There are 3 types of identification you can use to create the identifier:

```csharp
var codenameIdentifier = Reference.ByCodename("on_roasts");
var idIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
var externalIdIdentifier = Reference.ByExternalId("Ext-Item-456-Brno");
```

- **Codenames** are generated automatically by Kontent.ai based on the object's name. They can make your code more readable but are not guaranteed to be unique. Use them only when there is no chance of naming conflicts.
  - Unless set while creating a content item, the codename is initially generated from the item's name. When updating an item without specifying its codename, the codename gets autogenerated based on the name's value.
- (internal) **IDs** are random [GUIDs](https://en.wikipedia.org/wiki/Universally_unique_identifier) assigned to objects by Kontent.ai at the moment of import/creation. They are unique and generated by the system for existing objects. This means you cannot use them to refer to content that is not imported yet. **This identification is used for all responses from Management API**
- **External IDs** are string-based custom identifiers defined by you. Use them when importing a batch of cross-referencing content. See [Import content items guide](https://kontent.ai/learn/tutorials/set-up-kontent-ai/import-content/content-items/?tech=dotnet) for more details.

> The set of identification types varies based on the entity. The SDK does not check whether, for example, webhooks allows only ID for identification. This is being handled by the API itself. To check what identification types are allowed for a given entity, see the [API documentation](https://kontent.ai/learn/reference/management-api-v2/).

### User identifier

The SDK also supports endpoints that require either user ID or email. _UserIdentifier_ object represents identification of a user. See the following example for more detail:

```csharp
UserIdentifier identifier = UserIdentifier.ById("usr_0vKjTCH2TkO687K3y3bKNS");
UserIdentifier identifier = UserIdentifier.ByEmail("user@email.com");
```

### Handling Kontent.ai **errors**

You can catch Kontent.ai errors (more in [error section in Management API reference](https://kontent.ai/learn/reference/management-api-v2#section/Errors)) by using `try-catch` block and catching `Kontent.Ai.Management.Exceptions.ManagementException`.

```csharp
try
{
    var response = await client.UpsertLanguageVariantAsync(identifier, elements);
}
catch (ManagementException ex)
{
    Console.WriteLine(ex.StatusCode);
    Console.WriteLine(ex.Message);
}
```

### Working with language variants

The `ManagementClient` supports working with strongly-typed models. You can generate strongly-typed models from your content types using the Kontent.ai [model generator utility](https://github.com/kontent-ai/model-generator-net) and then be able to retrieve the data in a strongly typed form.

```csharp
// Retrieve strongly-typed content item
var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
var languageIdentifier = Reference.ByCodename("en-US");
var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

var response = await client.GetLanguageVariantAsync<ArticleModel>(identifier);

response.Elements.Title = new TextElement() { Value = "On Roasts - changed" };
response.Elements.PostDate = new DateTimeElement() { Value = new DateTime(2018, 7, 4) };

var responseVariant = await client.UpsertLanguageVariantAsync(identifier, response.Elements);
```

You can also construct an instance of strongly type model and provide values for the elements you want to change, without the necessity to retrieve the data from Kontent.ai. If a property is not initialized (is `null`) the SDK won't include it in the payload.

```csharp
// Defines the content elements to update
var stronglyTypedElements = new ArticleModel
{
    Title = new TextElement() { Value = "On Roasts - changed" },
    PostDate = new DateTimeElement() { Value = new DateTime(2018, 7, 4) },
};

// Specifies the content item and the language variant
var itemIdentifier = Reference.ByCodename("on_roasts");
var languageIdentifier = Reference.ByCodename("en-US");
var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

// Upserts a language variant of a content item
var response = await client.UpsertLanguageVariantAsync(identifier, stronglyTypedElements);
```

You can also use anonymous dynamic objects to work with language variants. For upsert operations, you need to provide element identification - `element.id`/`element.codename` (optionally load element's ID or codename from generated content model):

```csharp
var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
var languageIdentifier = Reference.ByCodename("en-US");
var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

// Elements to update
var elements = new dynamic[]
{
    new
    {
        element = new
        {
            // You can use `Reference.ById` if you don't have the model
            id = typeof(ArticleModel).GetProperty(nameof(ArticleModel.Title)).GetKontentElementId()
        },
        value = "On Roasts - changed",
    },
    new
    {
        element = new
        {
            // You can use `Reference.ByCodename` if you don't have the model
            codename = typeof(ArticleModel).GetProperty(nameof(ArticleModel.PostDate)).GetKontentElementCodename()
        },
        value = new DateTime(2018, 7, 4),
    }
};

var upsertModel = new LanguageVariantUpsertModel() { Elements = elements };

// Upserts a language variant of a content item
var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
```

You can also build your dynamic object representations of the elements from strongly typed elements models with `ElementBuilder`. That is **recommended approach when you don't need to work with strongly typed models** because it ensures you provided the element identification - `element.id`/`element.codename`.

```csharp
var itemIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
var languageIdentifier = Reference.ByCodename("en-US");
var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

// Elements to update
var elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
{
    new TextElement()
    {
        // You can use `Reference.ById` if you don't have the model
        Element = Reference.ById(typeof(ArticleModel).GetProperty(nameof(ArticleModel.Title)).GetKontentElementId()),
        Value = "On Roasts - changed"
    },
    new DateTimeElement()
    {
        // You can use `Reference.ByCodename` if you don't have the model
        Element = Reference.ByCodename(typeof(ArticleModel).GetProperty(nameof(ArticleModel.PostDate)).GetKontentElementCodename()),
        Value = new DateTime(2018, 7, 4)
    },
});

var upsertModel = new LanguageVariantUpsertModel() { Elements = elements };

// Upserts a language variant of a content item
var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
```

### Working with assets

The Kontent.ai [model generator utility](https://github.com/kontent-ai/model-generators-net) currently does not support generating a strongly-typed model from your asset type, however, you can construct an instance of a strongly-typed model yourself. Simply provide the elements you want to change:

```csharp
var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
var fileName = "Hello.txt";
var contentType = "text/plain";

// Returns a reference that you can later use to create an asset
var fileResult = await client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));

// Defines the content elements to create
var stronglyTypedTaxonomyElements = new AssetMetadataModel
{
    TaxonomyCategories = new TaxonomyElement()
    {
        Value = new[] { "hello", "SDK" }.Select(Reference.ByCodename)
    },
};

// Defines the asset to create
var asset = new AssetCreateModel<AssetMetadataModel>
{
    FileReference = fileResult,
    Elements = stronglyTypedTaxonomyElements
};

// Creates an asset
var response = await client.CreateAssetAsync(asset);
```

You can also build your dynamic object representations of the elements from strongly typed elements models with `ElementBuilder`. This is a **recommended approach when you don't need to work with strongly typed models** because it ensures you provided the element identification - `element.id`/`element.codename`.

```csharp
 // Elements to update
var taxonomyElements = ElementBuilder.GetElementsAsDynamic(
    new TaxonomyElement
    {
        Element = Reference.ByCodename("taxonomy-categories"),
        Value = new[]
        {
            Reference.ByCodename("hello"),
            Reference.ByCodename("SDK"),
        }
    });

// Defines the asset to update
var asset = new AssetUpsertModel
{
    Elements = taxonomyElements
};

var assetReference = Reference.ById(Guid.Parse("6d1c8ee9-76bc-474f-b09f-8a54a98f06ea"));

// Updates asset metadata
var response = await client.UpsertAssetAsync(assetReference, asset);
```

You can also use anonymous dynamic objects to work with assets, same as with language variants.

## Quick start

### Retrieving content items

Responses from Kontent.ai API are paginated. To retrieve all of content items, you need to go page by page. Here's how:

```csharp
var items = new List<ContentItemModel>();

var response = await _client.ListContentItemsAsync();

while (true)
{
    items.AddRange(response);

    if (!response.HasNextPage())
    {
        break;
    }

    response = await response.GetNextPage();
}
```

If you need all content items you can use `GetAllAsync`:

```csharp
var response = await _client.ListContentItemsAsync().GetAllAsync();
```

### Importing content items

Importing content items is a 2-step process, using 2 separate methods:

1. Creating an empty content item which serves as a wrapper for your content.
1. Adding content into a language variant of the content item.

Each content item can consist of several localized variants. **The content itself is always part of a specific language variant, even if your project only uses one language**. See our tutorial on [Importing to Kontent.ai](https://kontent.ai/learn/tutorials/set-up-kontent/import-content/overview) for a more detailed explanation.

#### 1. Creating a content item

```csharp
// Creates an instance of the ManagementClient
var client = new ManagementClient(options);

var item = new ContentItemCreateModel
{
    Codename = "on_roasts",
    Name = "On Roasts",
    Type = Reference.ByCodename("article")
};

var responseItem = await client.CreateContentItemAsync(item);
```

Kontent.ai will generate an internal ID for the (new and empty) content item and include it in the response. If you do not specify a codename, it will be generated based on name. In the next step, we will add the actual (localized) content.

#### 2. Adding language variants

To add localized content, you have to specify:

- The content item you are importing into.
- The language variant of the content item.
- The language variant elements you want to add or update. Omitted elements will remain unchanged.

```csharp
var componentId = "04bc8d32-97ab-431a-abaa-83102fc4c198";
var contentTypeCodename = "article";
var relatedArticle1Guid = Guid.Parse("b4e7bfaa-593c-4ae4-a231-5136b10757b8");
var relatedArticle2Guid = Guid.Parse("6d1c8ee9-76bc-474f-b09f-8a54a98f06ea");
var taxonomyTermGuid1 = Guid.Parse("5c060bf3-ed38-4c77-acfa-9868e6e2b5dd");
var taxonomyTermGuid2 = Guid.Parse("5c060bf3-ed38-4c77-acfa-9868e6e2b5dd");

// Defines the content elements to update
var stronglyTypedElements = new ArticleModel
{
    Title = new TextElement() { Value = "On Roasts" },
    PostDate = new DateTimeElement() { Value = new DateTime(2017, 7, 4) },
    BodyCopy = new RichTextElement
    {
        Value = $"<p>Rich Text</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{componentId}\"></object>",
        Components = new ComponentModel[]
        {
            new ComponentModel
            {
                Id = Guid.Parse(componentId),
                Type = Reference.ByCodename(contentTypeCodename),
                Elements = new dynamic[]
                {
                    new
                    {
                        element = new
                        {
                            id = typeof(ArticleModel).GetProperty(nameof(ArticleModel.Title)).GetKontentElementId()
                        },
                        value = "Article component title",
                    }
                }
            }
        }
    },
    RelatedArticles = new LinkedItemsElement
    {
        Value = new[] { relatedArticle1Guid, relatedArticle2Guid }.Select(Reference.ById)
    },
    Personas = new TaxonomyElement
    {
        Value = new[] { taxonomyTermGuid1, taxonomyTermGuid2 }.Select(Reference.ById)
    },
    UrlPattern = new UrlSlugElement { Value = "on-roasts", Mode = "custom" },
};

// Specifies the content item and the language variant
var itemIdentifier = Reference.ByCodename("on_roasts");
var languageIdentifier = Reference.ByCodename("en-US");
var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

// Upserts a language variant of your content item
var response = await client.UpsertLanguageVariantAsync<ArticleModel>(identifier, stronglyTypedElements);
```

### Helper Methods

Methods for building links to content items and their elements in Kontent.ai. Available as a [separate NuGet package](https://www.nuget.org/packages/Kontent.Ai.Management.Helpers/).

#### Getting an edit link for a content item

```csharp
var options = new ManagementHelpersOptions
{
    ProjectId = "bb6882a0-3088-405c-a6ac-4a0da46810b0",
};

string itemId = "8ceeb2d8-9676-48ae-887d-47ccb0f54a79";
string languageCodename = "en-US";

var linkBuilder = new EditLinkBuilder(options);
var result = linkBuilder.BuildEditItemUrl(languageCodename, itemId);

// Result is "https://app.kontent.ai/goto/edit-item/project/bb6882a0-3088-405c-a6ac-4a0da46810b0/
// variant-codename/en-US/item/8ceeb2d8-9676-48ae-887d-47ccb0f54a79"
```

#### Getting an edit link for a specific content element

```csharp
var options = new ManagementHelpersOptions
{
    ProjectId = "bb6882a0-3088-405c-a6ac-4a0da46810b0",
};

string itemId = "8ceeb2d8-9676-48ae-887d-47ccb0f54a79";
string languageCodename = "en-US";
var elementIdentifier = new ElementIdentifier(itemId, "single-Element-Codename");

var linkBuilder = new EditLinkBuilder(options);
var result = linkBuilder.BuildEditItemUrl(languageCodename, elementIdentifier);

// Result is "https://app.kontent.ai/goto/edit-item/project/bb6882a0-3088-405c-a6ac-4a0da46810b0/
// variant-codename/en-US/item/8ceeb2d8-9676-48ae-887d-47ccb0f54a79/element/single-Element-Codename"
```

#### Getting an edit link for multiple content elements

```csharp
var options = new ManagementHelpersOptions
{
    ProjectId = "bb6882a0-3088-405c-a6ac-4a0da46810b0",
};

string languageCodename = "en-US";
var elements = new ElementIdentifier[]
{
    new ElementIdentifier("76c06b74-bae9-4732-b629-1a59395e893d", "some-Element-Codename-1"),
    new ElementIdentifier("326c63aa-ae71-40b7-a6a8-56455b0b9751", "some-Element-Codename-2"),
    new ElementIdentifier("ffcd0436-8274-40ee-aaae-86fee1966fce", "some-Element-Codename-3"),
    new ElementIdentifier("d31d27cf-ddf6-4040-ab67-2f70edc0d46b", "some-Element-Codename-4"),
};

var linkBuilder = new EditLinkBuilder(options);
var result = linkBuilder.BuildEditItemUrl(languageCodename, elements);

// Result is "https://app.kontent.ai/goto/edit-item/"
//    project/bb6882a0-3088-405c-a6ac-4a0da46810b0/variant-codename/en-US/
//    item/76c06b74-bae9-4732-b629-1a59395e893d/element/some-Element-Codename-1/
//    item/326c63aa-ae71-40b7-a6a8-56455b0b9751/element/some-Element-Codename-2/
//    item/ffcd0436-8274-40ee-aaae-86fee1966fce/element/some-Element-Codename-3/
//    item/d31d27cf-ddf6-4040-ab67-2f70edc0d46b/element/some-Element-Codename-4"
```

## Add source tracking header 

Are you developing a plugin or a tool based on this SDK? Great! Then please include the source tracking header in your code. This way, we'll be able to identify that the traffic to Kontent.ai APIs is originating from your plugin and share its statistics with you!

You can either attach it to the **AssemblyInfo.cs**
```c#
[assembly: SourceTrackingHeaderAttribute()]
```

Or to the **.csproj**:

```xml
  <ItemGroup>
    <AssemblyAttribute Include="Kontent.Ai.Management.Attributes.SourceTrackingHeader" />
  </ItemGroup>
```

By default, it will load the necessary info (package name + version) from your assembly. If you want to customize it, please use one of the constructors:

```c#
// You specify the name, the version is extracted from the assembly
public SourceTrackingHeaderAttribute(string packageName)

// Or you specify the name and the version
public SourceTrackingHeaderAttribute(string packageName, int majorVersion, int minorVersion, int patchVersion, string preReleaseLabel = null)
```

If you use the **.csproj**:
```xml
<AssemblyAttribute Include="Kontent.Ai.Management.Attributes.SourceTrackingHeader">
	<_Parameter1>My.Module</_Parameter1>
	<_Parameter2>1</_Parameter2>
	<_Parameter2_IsLiteral>true</_Parameter2_IsLiteral>
	<_Parameter3>2</_Parameter3>
	<_Parameter3_IsLiteral>true</_Parameter3_IsLiteral>
	<_Parameter4>3</_Parameter4>
	<_Parameter4_IsLiteral>true</_Parameter4_IsLiteral>
	<_Parameter5>beta</_Parameter5>
</AssemblyAttribute>
```

## How to use [SourceLink](https://github.com/dotnet/sourcelink/) for debugging

This repository is configured to generate SourceLink tag in the Nuget package, allowing to debug its source code when it is referenced as a Nuget package. Source code is downloaded directly from github to Visual Studio.

### How to configure SourceLink

1. Open a solution with a project referencing the Kontent.Ai.Management Nuget package.
1. Open Tools -> Options -> Debugging -> General.

   - Clear **Enable Just My Code**.
   - Select **Enable Source Link Support**.
   - (Optional) Clear **Require source files to exactly match the original version**.

1. Build your solution.
1. [Add a symbol server `https://symbols.nuget.org/download/symbols`](https://blog.nuget.org/20181116/Improved-debugging-experience-with-the-NuGet-org-symbol-server-and-snupkg.html)

   - ![Add a symbol server in VS](/.github/assets/vs-nuget-symbol-server.PNG)

1. Run a debugging session and try to step into the Kontent.Ai.Management code.
1. Allow Visual Studio to download the source code from GitHub.

- ![SourceLink confirmation dialog](/.github/assets/allow_sourcelink_download.png)

**Now you are able to debug the source code of our library without having to download it manually!**

## Further information

For more developer resources, visit the [overview of .NET tools](https://kontent.ai/learn/tutorials/develop-apps/overview?tech=dotnet) and [API references](https://kontent.ai/learn/reference) at Kontent.ai Learn.

### Building the sources

Prerequisites:

**Required:**
[.NET](https://dotnet.microsoft.com/en-us/download/dotnet).

Optional:

- [Visual Studio](https://visualstudio.microsoft.com/vs/) for full experience
- or [Visual Studio Code](https://code.visualstudio.com/)

### Creating a new release

- [**Release & version management**](https://github.com/kontent-ai/kontent-ai.github.io/blob/main/docs/articles/Release-%26-version-management-of-.NET-projects.md)
- [Kontent.ai's best practices for .csproj files](https://github.com/kontent-ai/kontent-ai.github.io/blob/main/docs/articles/Kontent.ai-best-practices-for-.csproj-files.md)

## Feedback & Contributing

Check out the [contributing](./CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.
