# Kontent Management .NET SDK

[![Build & Test](https://github.com/Kentico/kontent-management-sdk-net/actions/workflows/integrate.yml/badge.svg)](https://github.com/Kentico/kontent-management-sdk-net/actions/workflows/integrate.yml)
[![codecov](https://codecov.io/gh/Kentico/kontent-management-sdk-net/branch/master/graph/badge.svg?token=xhM2JrUuA4)](https://codecov.io/gh/Kentico/kontent-management-sdk-net)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico-kontent)
[![Discord](https://img.shields.io/discord/821885171984891914?color=%237289DA&label=Kontent%20Discord&logo=discord)](https://discord.gg/SKCxwPtevJ)

| Package                       |                                                                         Version                                                                         |                                                                       Downloads                                                                       |                        Compatibility                         |           Documentation           |
| ----------------------------- | :-----------------------------------------------------------------------------------------------------------------------------------------------------: | :---------------------------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------: | :-------------------------------: |
| Management SDK                |         [![NuGet](https://img.shields.io/nuget/vpre/Kentico.Kontent.Management.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management)         |         [![NuGet](https://img.shields.io/nuget/dt/kentico.kontent.Management.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management)         | [`net6.0`](https://dotnet.microsoft.com/download/dotnet/6.0) [`netstandard2.0`\*](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) | [📖](#using-the-managementclient) |
| Content Item Edit-URL Builder | [![NuGet](https://img.shields.io/nuget/vpre/Kentico.Kontent.Management.Helpers.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management.Helpers) | [![NuGet](https://img.shields.io/nuget/dt/kentico.kontent.Management.Helpers.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management.Helpers) | [`net6.0`](https://dotnet.microsoft.com/download/dotnet/6.0) [`netstandard2.0`\*](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) |       [📖](#helper-methods)       |

> \* We highly recommend to target [`net6.0`](https://dotnet.microsoft.com/download/dotnet/6.0) in your projects. [`netstandard2.0`](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) is supported to allow older projects to iterativelly upgrade.

## Summary

> ℹ This is **the BETA version of the SDK** which uses [Management API version 2](https://docs.kontent.ai/reference/management-api-v2) ℹ.

The Kontent Management .NET SDK is a client library used for managing content in Kontent by Kentico. It provides read/write access to your Kontent projects.

You can use the SDK in the form of a [NuGet package](https://www.nuget.org/packages/Kentico.Kontent.Management) to migrate existing content into your Kontent project or update your content model.

The Management SDK does not provide any content filtering options and is not optimized for content delivery. If you need to deliver larger amounts of content we recommend using the [Delivery SDK](https://github.com/Kentico/delivery-sdk-net) instead.

💡 If you want to see all .NET related resources including REST API reference with .NET code samples for every endpoind, check out the ["Develop .NET apps" overview page](https://docs.kontent.ai/tutorials/develop-apps/overview?tech=dotnet).

## Prerequisites

To manage content in a Kontent project via the Management API, you first need to activate the API for the project. See our documentation on how you can [activate the Management API](https://docs.kontent.ai/tutorials/set-up-projects/migrate-content/importing-to-kentico-kontent#a-enabling-the-api-for-your-project).

## Using the ManagementClient

The `ManagementClient` class is the main class of the SDK. Using this class, you can import, update, view and delete content items, language variants, and assets in your Kentico Kontent projects.

To create an instance of the class, you need to provide a [project ID](https://docs.kontent.ai/tutorials/develop-apps/get-content/getting-content#a-getting-content-items) and a valid [Management API Key](https://docs.kontent.ai/tutorials/set-up-projects/migrate-content/importing-to-kentico-kontent#a-enabling-the-api-for-your-project).

```csharp
// Initializes an instance of the ManagementClient client with specified options.
var client = new ManagementClient(new ManagementOptions
{
    ProjectId = "cbbe2d5c-17c6-0128-be26-e997ba7c1619",
    ApiKey = "ew0...1eo"
});
```

Once you create a `ManagementClient`, you can start managing content in your project by calling methods on the client instance.

### Codename vs. ID vs. External ID

The SDK uses an _Reference_ object representation identifying an entity you want to perform the given operation on. There are 3 types of identification you can use to create the identifier:

```csharp
Reference codenameIdentifier = Reference.ByCodename("on_roasts");
Reference idIdentifier = Reference.ById(Guid.Parse("9539c671-d578-4fd3-aa5c-b2d8e486c9b8"));
Reference externalIdIdentifier = Reference.ByExternalId("Ext-Item-456-Brno");
```

- **Codenames** are generated automatically by Kontent based on the object's name. They can make your code more readable but are not guaranteed to be unique. Use them only when there is no chance of naming conflicts.
  - Unless set while creating a content item, the codename is initially generated from the item's name. When updating an item without specifying its codename, the codename gets autogenerated based on the name's value.
- (internal) **IDs** are random [GUIDs](https://en.wikipedia.org/wiki/Universally_unique_identifier) assigned to objects by Kontent at the moment of import/creation. They are unique and generated by the system for existing objects. This means you cannot use them to refer to content that is not imported yet. **This identification is used for all responses from Management API**
- **External IDs** are string-based custom identifiers defined by you. Use them when importing a batch of cross-referencing content. See [Importing linked content](#importing-linked-content) for more details.

> The the set of identification types varies based on the entity. The SDK does not checks whether i.e. webhooks allows only ID for identification. This is being handled by the API itself. To check what identification types are allowed for a given entity, see the [API documentation](https://docs.kontent.ai/reference/management-api-v2/).

### Handling Kontent **errors**

You can catch Kontent errors (more in [error section in Management API reference](https://docs.kontent.ai/reference/management-api-v2#section/Errors))) by usin `try-catch` block and catching `Kentico.Kontent.Management.Exceptions.ManagementException`.

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

The `ManagementClient` supports working with strongly-typed models. You can generate strongly-typed models from your content types using the Kentico Kontent [model generator utility](https://github.com/Kentico/kontent-generators-net) and then be able to retrieve the data in a strongly typed form.

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

Or you can construct instance of strongly type model without necessity to retrieve the data from Kontent. You just provide the elements you want to change. If the property is not initialized (is `null`) the SDK won't include to the payload.

```csharp
// Defines the content elements to update
ArticleModel stronglyTypedElements = new ArticleModel
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
            // You can use `Reference.ById` if you don't have the model
            id = typeof(ArticleModel).GetProperty(nameof(ArticleModel.PostDate)).GetKontentElementId()
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
        // You can use `Reference.ById` if you don't have the model
        Element = Reference.ById(typeof(ArticleModel).GetProperty(nameof(ArticleModel.PostDate)).GetKontentElementId()),
        Value = new DateTime(2018, 7, 4)
    },
});

var upsertModel = new LanguageVariantUpsertModel() { Elements = elements };

// Upserts a language variant of a content item
var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);
```

## Quick start

### Importing content items

Importing content items is a 2 step process, using 2 separate methods:

1. Creating an empty content item which serves as a wrapper for your content.
1. Adding content inside a language variant of the content item.

Each content item can consist of several localized variants. **The content itself is always part of a specific language variant, even if your project only uses one language**. See our tutorial on [Importing to Kentico Kontent](https://docs.kontent.ai/tutorials/set-up-projects/migrate-content/importing-to-kentico-kontent#a-importing-your-content) for a more detailed explanation.

#### 1. Creating a content item

```csharp
// Creates an instance of the Management client
var client = new ManagementClient(options);

var item = new ContentItemCreateModel
{
    Codename = "on_roasts",
    Name = "On Roasts",
    Type = Reference.ByCodename("article")
};

var responseItem = await client.CreateContentItemAsync(item);
```

Kentico Kontent will generate an internal ID for the (new and empty) content item and include it in the response. If you do not specify a codename, it will be generated based on name. In the next step, we will add the actual (localized) content.

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
ArticleModel stronglyTypedElements = new ArticleModel
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

Methods for building links to content items and their elements in Kentico Kontent. Available as a [separate NuGet package](https://www.nuget.org/packages/Kentico.Kontent.Management.Helpers/).

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

## How to use [SourceLink](https://github.com/dotnet/sourcelink/) for debugging

This repository is configured to generate SourceLink tag in the Nuget package that allows to debug this repository source code when it is referenced as a Nuget package. Source code is downloaded directly from github to the Visual Studio.

### How to configure Source Link

1. Open a solution with a project referencing the Kentico.Kontent.Management Nuget package.
1. Open Tools -> Options -> Debugging -> General.

   - Clear **Enable Just My Code**.
   - Select **Enable Source Link Support**.
   - (Optional) Clear **Require source files to exactly match the original version**.

1. Build your solution.
1. [Add a symbol server `https://symbols.nuget.org/download/symbols`](https://blog.nuget.org/20181116/Improved-debugging-experience-with-the-NuGet-org-symbol-server-and-snupkg.html)

   - ![Add a symbol server in VS](/.github/assets/vs-nuget-symbol-server.PNG)

1. Run a debugging session and try to step into the Kentico.Kontent.Management code.
1. Allow Visual Studio to download the source code from GitHub.

- ![SourceLink confirmation dialog](/.github/assets/allow_sourcelink_download.png)

**Now you are able to debug the source code of our library without needing to download the source code manually!**

## Further information

For more developer resources, visit the Kentico Kontent Developer Hub at <https://docs.kontent.ai>.

### Building the sources

Prerequisites:

**Required:**
[.NET Core SDK](https://www.microsoft.com/net/download/core).

Optional:

- [Visual Studio 2017](https://www.visualstudio.com/vs/) for full experience
- or [Visual Studio Code](https://code.visualstudio.com/)

#### Tests

Tests can run against Live endpoint or mocked filesystem. `TestUtils.TestRunType` specifies target environemnt for tests. Commit always with TestRunType.MockFromFileSystem.

> _Following section is meant to be used by maintainers and people with access to the live endpoint project._
>
> For updating mocked data use `TestUtils.TestRunType.LiveEndPoint_SaveToFileSystem`. When using `TestRunType.MockFromFileSystem`, at the build time, data from `Data` directory are being used as a mocked data.

### Creating a new release

[Release & version management](https://github.com/Kentico/kontent-management-sdk-net/wiki/Release-&-version-management)

## Feedback & Contributing

Check out the [contributing](https://github.com/Kentico/content-management-sdk-net/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.
