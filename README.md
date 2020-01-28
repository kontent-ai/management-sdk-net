# Kentico Kontent Management .NET SDK

[![Build status](https://ci.appveyor.com/api/projects/status/6b3tt1kc3ogmcav3/branch/master?svg=true)](https://ci.appveyor.com/project/kentico/content-management-sdk-net/branch/master)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico-kontent)

| Package        | Version  | Downloads | Documentation |
| ------------- |:-------------:| :-------------:|  :-------------:|
| Management SDK | [![NuGet](https://img.shields.io/nuget/v/Kentico.Kontent.Management.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management) | [![NuGet](https://img.shields.io/nuget/dt/kentico.kontent.Management.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management) | [📖](#using-the-managementclient) |
| Content Item Edit-URL Builder  | [![NuGet](https://img.shields.io/nuget/v/Kentico.Kontent.Management.Helpers.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management.Helpers) | [![NuGet](https://img.shields.io/nuget/dt/kentico.kontent.Management.Helpers.svg)](https://www.nuget.org/packages/Kentico.Kontent.Management.Helpers) | [📖](#helper-methods) |

## Summary

The Kentico Kontent Management .NET SDK is a client library used for managing content in Kentico Kontent. It provides read/write access to your Kentico Kontent projects.

You can use the SDK in the form of a [NuGet package](https://www.nuget.org/packages/Kentico.Kontent.Management/) to migrate existing content into your Kentico Kontent project or update content in your content items. You can import content items, their language variants, and assets.

The Management SDK does not provide any content filtering options and is not optimized for content delivery. If you need to deliver larger amounts of content we recommend using the [Delivery SDK](https://github.com/Kentico/delivery-sdk-net) instead.

You can head over to our Developer Hub for the complete [Management API Reference](https://docs.kontent.ai/reference/content-management-api-v1).

## Prerequisites

To manage content in a Kentico Kontent project via the Management API, you first need to activate the API for the project. See our documentation on how you can [activate the Management API](https://docs.kontent.ai/tutorials/set-up-projects/migrate-content/importing-to-kentico-kontent#a-enabling-the-api-for-your-project).

You also need to prepare the structure of your Kentico Kontent project before importing your content. This means defining the [Content types](https://docs.kontent.ai/tutorials/set-up-projects/define-content-models/creating-and-deleting-content-types) of the items you want to import. You might also need to set up your [Languages](https://docs.kontent.ai/tutorials/set-up-projects/set-up-languages/localization-in-kentico-kontent), [Taxonomy](https://docs.kontent.ai/tutorials/set-up-projects/define-content-models/organizing-your-content-with-taxonomies#a-getting-organized-with-taxonomies-for-release-publishing) or [Sitemap locations](https://docs.kontent.ai/tutorials/develop-apps/optimize-for-the-web/optimizing-content-for-search-engines#a-sitemaps) (if you plan to use them).

## Using the ManagementClient

The `ManagementClient` class is the main class of the SDK. Using this class, you can import, update, view and delete content items, language variants, and assets in your Kentico Kontent projects.

To create an instance of the class, you need to provide a [project ID](https://docs.kontent.ai/tutorials/develop-apps/get-content/getting-content#a-getting-content-items) and a valid [Management API Key](https://docs.kontent.ai/tutorials/set-up-projects/migrate-content/importing-to-kentico-kontent#a-enabling-the-api-for-your-project).

```csharp
ManagementOptions options = new ManagementOptions
{
    ProjectId = "bb6882a0-3088-405c-a6ac-4a0da46810b0",
    ApiKey = "ew0...1eo"
};

// Initializes an instance of the ManagementClient client
ManagementClient client = new ManagementClient(options);
```

Once you create a `ManagementClient`, you can start managing content in your project by calling methods on the client instance. See [Importing content items](#importing-content-items) for details.

### Codename vs. ID vs. External ID

Most methods of the SDK accept an *Identifier* object that specifies which content item, language variant, or asset you want to perform the given operation on. There are 3 types of identification you can use to create the identifier:

```csharp
ContentItemIdentifier identifier = ContentItemIdentifier.ByCodename("on_roasts");
ContentItemIdentifier identifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));
ContentItemIdentifier identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
```

* **Codenames** are generated automatically by Kentico Kontent based on the object's name. They can make your code more readable but are not guaranteed to be unique. Use them only when there is no chance of naming conflicts.
  * Unless set while creating a content item, the codename is initially generated from the item's name. When updating an item without specifying its codename, the codename gets autogenerated based on the name's value.
* (internal) **IDs** are random [GUIDs](https://en.wikipedia.org/wiki/Universally_unique_identifier) assigned to objects by Kentico Kontent at the moment of import/creation. They are unique and generated by the system for existing objects. This means you cannot use them to refer to content that is not imported yet.
* **External IDs** are string-based custom identifiers defined by you. Use them when importing a batch of cross-referencing content. See [Importing linked content](#importing-linked-content) for more details.

### Strongly-typed models of your content

The `ManagementClient` also supports working with strongly-typed models. You can generate strongly-typed models from your content types using the Kentico Kontent [model generator utility](https://github.com/Kentico/kontent-generators-net).

```csharp
// Elements to update
ArticleModel stronglyTypedElements = new ArticleModel
{
    Title = "On Roasts",
    PostDate = new DateTime(2017, 7, 4)
};

// Upserts a language variant of a content item
ContentItemVariantModel<ArticleModel> response = await client.UpsertContentItemVariantAsync<ArticleModel>(identifier, stronglyTypedElements);
```

You can also use anonymous objects to achieve the same result:

```csharp
// Elements to update
var elements = new
{
    title = "On Roasts",
    post_date = new DateTime(2017, 7, 4)
};
ContentItemVariantUpsertModel upsertModel = new ContentItemVariantUpsertModel() { Elements = elements };

// Upserts a language variant of a content item
ContentItemVariantModel<CafeModel> response = await client.UpsertContentItemVariantAsync<CafeModel>(identifier, upsertModel);
```

However, we encourage you to use strongly-typed models for their convenience and type safety. Examples in this document use strongly-typed models where possible.

## Quick start

### Importing content items

Importing content items is a 2 step process, using 2 separate methods:

1. Creating an empty content item which serves as a wrapper for your content.
1. Adding content inside a language variant of the content item.

Each content item can consist of several localized variants. **The content itself is always part of a specific language variant, even if your project only uses one language**. See our tutorial on [Importing to Kentico Kontent](https://docs.kontent.ai/tutorials/set-up-projects/migrate-content/importing-to-kentico-kontent#a-importing-your-content) for a more detailed explanation.

#### 1. Creating a content item

```csharp
// Creates an instance of the Management client
ManagementClient client = new ManagementClient(options);

// Defines the content item to import
ContentItemCreateModel item = new ContentItemCreateModel()
{
    CodeName = "on_roasts", // When not specified, the codename gets autogenerated based on the name's value
    Name = "On Roasts",
    Type = ContentTypeIdentifier.ByCodename("article"),
    SitemapLocations = new[] { SitemapNodeIdentifier.ByCodename("articles") }
};

// Adds the content item to your project in Kentico Kontent
ContentItemModel response = await client.CreateContentItemAsync(item);
```

Kentico Kontent will generate an internal ID for the (new and empty) content item and include it in the response. If you do not specify a codename, it will be generated based on name. In the next step, we will add the actual (localized) content.

#### 2. Adding language variants

To add localized content, you have to specify:

* The content item you are importing into.
* The language variant of the content item.
* The content elements of the language variant you want to add or update. Omitted elements will remain unchanged.

```csharp
// Defines the content elements to update
ArticleModel stronglyTypedElements = new ArticleModel
{
    Title = "On Roasts",
    PostDate = new DateTime(2017, 7, 4),
    BodyCopy = @"
        <h1>Light Roasts</h1>
        <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color. This method
        is used for milder coffee  varieties and for coffee tasting. This type of roasting allows the natural
        characteristics of each coffee to show. The aroma of coffees produced from light roasts is usually
        more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p>
    ",
    RelatedArticles = new [] { ContentItemIdentifier.ByCodename("which_brewing_fits_you_") },
    UrlPattern = "on-roasts",
    Personas = new [] { TaxonomyTermIdentifier.ByCodename("barista") }
};

// Specifies the content item and the language variant
ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByCodename("on_roasts");
LanguageIdentifier languageIdentifier = LanguageIdentifier.ByCodename("en-US");
ContentItemVariantIdentifier identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

// Upserts a language variant of your content item
ContentItemVariantModel<ArticleModel> response = await client.UpsertContentItemVariantAsync<ArticleModel>(identifier, stronglyTypedElements);
```

### Importing assets

Importing assets using the Management API is usually a 2-step process:

1. Uploading a file to Kentico Kontent.
1. Creating a new asset using the given file reference.

This SDK, however, simplifies the process and allows you to upload assets using a single method:

```csharp
// Defines the description of an asset
AssetDescription assetDescription = new AssetDescription
{
    Description = "Description of the asset in English Language",
    Language = LanguageIdentifier.ByCodename("en-US")
};

IEnumerable<AssetDescription> descriptions = new [] { assetDescription };

// Title of a new asset
string title = "Asset title";

// Defines the asset to upsert
AssetUpdateModel asset = new AssetUpdateModel
{
    Descriptions = descriptions,
    Title = title
};

string filePath = "‪C:\\Users\\Kentico\\Desktop\\puppies.png";
string contentType = "image/png";

// Uploads the file and links it to a new asset
AssetModel response = await client.CreateAssetAsync(new FileContentSource(filePath, contentType), asset);
```

### Importing linked content

The content you are importing will often contain references to other pieces of imported content. A content item can reference assets, link to other content items in the *Linked items* or *Rich Text* element, and contain hypertext links in the rich text editor. To avoid having to import objects in a specific order (and solve problems with cyclical dependencies), you can use **external IDs** to reference non-existent (not imported yet) content:

1. Define external IDs for all content items and assets you want to import in advance.
1. When referencing another content item or asset, use its external ID.
1. Import your content using the upsert methods with external ID. The system will resolve all references.

This way, you can import your content in any order and run the import process repeatedly to keep your project up to date. In the example below, we import an asset and a content item that uses it:

```csharp
// Upserts an asset, assuming you already have the fileResult reference to the uploaded file
AssetUpsertModel asset = new AssetUpsertModel
{
    FileReference = fileResult
};

string assetExternalId = "Ext-Asset-123-png";
AssetModel assetResponse = await client.UpsertAssetByExternalIdAsync(assetExternalId, asset);

// Upserts a content item
ContentItemUpsertModel item = new ContentItemUpsertModel
{
    CodeName = "brno", // When not specified, the codename gets autogenerated based on the name's value
    Name = "Brno",
    Type = ContentTypeIdentifier.ByCodename("cafe")
};

string itemExternalId = "Ext-Item-456-Brno";
ContentItemModel itemResponse = await client.UpsertContentItemByExternalIdAsync(itemExternalId, item);

// Upsert a language variant which references the asset using external ID
CafeModel stronglyTypedElements = new CafeModel
{
    Picture = new [] { AssetIdentifier.ByExternalId(assetExternalId) },
    City = "Brno",
    Country = "Czech Republic"
};

ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByExternalId(itemExternalId);
LanguageIdentifier languageIdentifier = LanguageIdentifier.ByCodename("en-US");
ContentItemVariantIdentifier identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

ContentItemVariantModel<CafeModel> variantResponse = await client.UpsertContentItemVariantAsync<CafeModel>(identifier, stronglyTypedElements);
```

### Content item methods

#### Upserting a content item by external ID

```csharp
// Defines a content item to upsert
ContentItemUpsertModel item = new ContentItemUpsertModel
{
    CodeName = "new_or_updated_codename" // When not specified, the codename gets autogenerated based on the name's value
    Name = "New or updated name",
    Type = ContentTypeIdentifier.ByCodename("cafe"),
    SitemapLocations = new[] { SitemapNodeIdentifier.ByCodename("cafes") }
};

string itemExternalId = "Ext-Item-456-Brno";

// Upserts a content item by external ID
ContentItemModel response = await client.UpsertContentItemByExternalIdAsync(itemExternalId, item);
```

#### Adding a content item

```csharp
// Defines a content item to add
ContentItemCreateModel item = new ContentItemCreateModel
{
    CodeName = "brno", // When not specified, the codename gets autogenerated based on the name's value
    Name = "Brno",
    Type = ContentTypeIdentifier.ByCodename("cafe"),
    SitemapLocations = new[] { SitemapNodeIdentifier.ByCodename("cafes") }
};

// Creates a content item
ContentItemModel response = await client.CreateContentItemAsync(item);
```

#### Viewing a content item

```csharp
ContentItemIdentifier identifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
// ContentItemIdentifier identifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));

// Retrieves a content item
ContentItemModel response = await client.GetContentItemAsync(identifier);
```

#### Listing content items

```csharp
// Retrieves a list of content items
ListingResponseModel<ContentItemModel> response = await client.ListContentItemsAsync();

while (true)
{
    foreach (var item in response)
    {
        // use your content item
    }

    if (!response.HasNextPage())
    {
        break;
    }

    response = await response.GetNextPage();
}
 ```
 
#### Updating a content item

```csharp
ContentItemIdentifier identifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier identifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));

// Defines new properties of a content item
ContentItemUpdateModel item = new ContentItemUpdateModel
{
    CodeName = "new_codename", // When not specified, the codename gets autogenerated based on the name's value
    Name = "New name",
    SitemapLocations = new[] {
        SitemapNodeIdentifier.ByCodename("cafes"),
        SitemapNodeIdentifier.ByCodename("europe")
    }
};

// Updates a content item
ContentItemModel reponse = await client.UpdateContentItemAsync(identifier, item);
```

#### Deleting a content item

```csharp
ContentItemIdentifier identifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
// ContentItemIdentifier identifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));

// Deletes a content item
client.DeleteContentItemAsync(identifier);
```

### Language variant methods

#### Upserting a language variant

```csharp
// Defines the elements to update
CafeModel stronglyTypedElements = new CafeModel
{
    Street = "Nove Sady 25",
    City = "Brno",
    Country = "Czech Republic"
};

ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));
// ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

LanguageIdentifier languageIdentifier = LanguageIdentifier.ByCodename("en-US");
// LanguageIdentifier languageIdentifier = LanguageIdentifier.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"));

// Combines item and language identifiers into one
ContentItemVariantIdentifier identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);
// Upserts a language variant of a content item
ContentItemVariantModel<CafeModel> response = await client.UpsertContentItemVariantAsync<CafeModel>(identifier, stronglyTypedElements);
```

#### Viewing a language variant

```csharp
ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));
// ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

LanguageIdentifier languageIdentifier = LanguageIdentifier.ByCodename("en-US");
// LanguageIdentifier languageIdentifier = LanguageIdentifier.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"));

// Combines item and language identifiers into one
ContentItemVariantIdentifier identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);
// Retrieves a language variant of a content item
ContentItemVariantModel<CafeModel> response = await client.GetContentItemVariantAsync<CafeModel>(identifier);
```

#### Listing language variants

```csharp
ContentItemIdentifier identifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier identifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79"));
// ContentItemIdentifier identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

// Retrieves language variants of a content item
IEnumerable<ContentItemVariantModel<CafeModel>> response = await client.ListContentItemVariantsAsync<CafeModel>(identifier);
```

#### Deleting a language variant

```csharp
ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByCodename("brno");
// ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ById(Guid.Parse("8ceeb2d8-9676-48ae-887d-47ccb0f54a79")));
// ContentItemIdentifier itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

LanguageIdentifier languageIdentifier = LanguageIdentifier.ByCodename("en-US");
// LanguageIdentifier languageIdentifier = LanguageIdentifier.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"));

// Combines item and language identifiers into one
ContentItemVariantIdentifier identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);
// Deletes a language variant of a content item
await client.DeleteContentItemVariantAsync(identifier);
```

### Asset methods

#### Uploading a file

Upload a file to Kentico Kontent.

```csharp
MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
string fileName = "Hello.txt";
string contentType = "text/plain";

// Returns a reference that you can later use to create an asset
FileReference fileResult = await client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));
```

#### Upserting an asset using external ID

Update or create an asset using a `fileResult` reference to a previously uploaded file. You can specify an asset description for each language in your Kentico Kontent project. Specifying title of a new asset is optional. Use the asset title to better identify and filter your assets in the UI.

```csharp
AssetDescription assetDescription = new AssetDescription
{
    Description = "Description of the asset in English Language",
    Language = LanguageIdentifier.ByCodename("en-US")
};
IEnumerable<AssetDescription> descriptions = new [] { assetDescription };

// Title of a new asset
string title = "Asset title";

// Defines the asset to upsert
AssetUpsertModel asset = new AssetUpsertModel
{
    FileReference = fileResult,
    Descriptions = descriptions,
    Title = title
};

string externalId = "Ext-Asset-123-png";

// Upserts an asset by external ID
AssetModel response = await client.UpsertAssetByExternalIdAsync(externalId, asset);
```

#### Uploading an asset from a file system in a single step

Import the asset file and its descriptions using a single method. Specifying title of an asset is optional. You can use the asset title to better identify and filter your assets in the UI.

```csharp
AssetDescription assetDescription = new AssetDescription
{
    Description = "Description of the asset in English Language",
    Language = LanguageIdentifier.ByCodename("en-US")
};

IEnumerable<AssetDescription> descriptions = new [] { assetDescription };

// Title of a new asset
string title = "Asset title";

// Defines the asset to update
AssetUpdateModel asset = new AssetUpdateModel
{
    Descriptions = descriptions,
    Title = title
};

string filePath = "‪C:\Users\Kentico\Desktop\puppies.png";
string contentType = "image/png";

// Creates a new asset using the given file and its descriptions
AssetModel response = await client.CreateAssetAsync(new FileContentSource(filePath, contentType), asset);
```

#### Viewing an asset

```csharp
AssetIdentifier identifier = AssetIdentifier.ByExternalId("Ext-Asset-123-png");
// AssetIdentifier identifier = AssetIdentifier.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));

// Retrieves an asset
AssetModel response = await client.GetAssetAsync(identifier);
```

#### Listing assets

```csharp
// Retrieves a list of all assets
ListingResponseModel<AssetModel> response = await client.ListAssetsAsync();

while (true)
{
    foreach (var asset in response)
    {
        // use your asset
    }

    if (!response.HasNextPage())
    {
        break;
    }

    response = await response.GetNextPage();
}
```

#### Deleting an asset

```csharp
AssetIdentifier identifier = AssetIdentifier.ByExternalId("Ext-Asset-123-png");
// AssetIdentifier identifier = AssetIdentifier.ById(Guid.Parse("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));

// Deletes an asset
client.DeleteAssetAsync(identifier);
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

1. Open a solution with a project referencing the Kentico.Kontent.Delivery (or Kentico.Kontent.Delivery.RX) Nuget package.
2. Open Tools -> Options -> Debugging -> General.
    * Clear **Enable Just My Code**.
    * Select **Enable Source Link Support**.
    * (Optional) Clear **Require source files to exactly match the original version**.
3. Build your solution.
4. [Add a symbol server `https://symbols.nuget.org/download/symbols`](https://blog.nuget.org/20181116/Improved-debugging-experience-with-the-NuGet-org-symbol-server-and-snupkg.html)
  * ![Add a symbol server in VS](/.github/assets/vs-nuget-symbol-server.PNG)
5. Run a debugging session and try to step into the Kentico.Kontent.Delivery code.
6. Allow Visual Studio to download the source code from GitHub.
  * ![SourceLink confirmation dialog](/.github/assets/allow_sourcelink_download.png)

**Now you are able to debug the source code of our library without needing to download the source code manually!**

## Further information

For more developer resources, visit the Kentico Kontent Developer Hub at <https://docs.kontent.ai>.

### Building the sources

Prerequisites:

**Required:**
[.NET Core SDK](https://www.microsoft.com/net/download/core).

Optional:
* [Visual Studio 2017](https://www.visualstudio.com/vs/) for full experience
* or [Visual Studio Code](https://code.visualstudio.com/)

### Creating a new release

[Release & version management](https://github.com/Kentico/kontent-management-sdk-net/wiki/Release-&-version-management)

## Feedback & Contributing

Check out the [contributing](https://github.com/Kentico/content-management-sdk-net/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.

![Analytics](https://kentico-ga-beacon.azurewebsites.net/api/UA-69014260-4/Kentico/content-management-sdk-net?pixel)
