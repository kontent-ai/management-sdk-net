# Kentico Cloud Content Management .NET SDK

## Summary

The Kentico Cloud Content Management .NET SDK is a client library used for managing content in Kentico Cloud. It provides read/write access to your Kentico Cloud projects.  
You can use the SDK in the form of a [NuGet package](https://www.nuget.org/packages/KenticoCloud.Delivery) to migrate existing content into your Kentico Cloud project or update content in your content items. You can import content items, their language variants, and assets.

The Content Management SDK does not provide any content filtering options and is not optimized for content delivery. If you need to deliver larger amounts of content we recommend using the [Delivery SDK](https://github.com/Kentico/delivery-sdk-net) instead.

You can head over to our Developer Hub for the complete [Content Management API Reference](https://developer.kenticocloud.com/v1/reference#content-management-api). 

## Prerequisites

To manage content in a Kentico Cloud project via the Content Management API, you first need to activate the API for the project. See our documentation on how you can [activate the Content Management API](https://developer.kenticocloud.com/v1/docs/importing-to-kentico-cloud#section-enabling-the-api-for-your-project).

You also need to prepare the structure of your Kentico Cloud project before importing your content. At minimum, that means defining the [Content types](https://help.kenticocloud.com/define-content-structure/structure/creating-and-deleting-content-types) of the the items you want to impoort. You might also need to set up your [Languages](https://help.kenticocloud.com/multilingual-content#managing-languages), [Taxonomy](https://help.kenticocloud.com/categorize-content/working-with-taxonomy) or [Sitemap locations](https://help.kenticocloud.com/categorize-content/using-a-sitemap) (if you are using them). 

## Using the ContentManagementClient

The `ContentManagementClient` class is the main class of the SDK. Using this class, you can import, update, view and delete content items, language variants, and assets in your Kentico Cloud projects. 

To create an instance of the class, you need to provide a [project ID](https://developer.kenticocloud.com/docs/using-delivery-api#section-getting-project-id) and a valid [Content Management API Key](https://developer.kenticocloud.com/v1/docs/importing-to-kentico-cloud#importing-content-items).

```csharp
var options = new ContentManagementOptions() { 
    ProjectId = "bb6882a0-3088-405c-a6ac-4a0da46810b0",
    ApiKey = "ew0...1eo" 
}; 
// Initialize an instance of the ContentManagementClient client
var client = new ContentManagementClient(options);
```

Once you create a `ContentManagementClient`, you can start managing content in your project by calling methods on the client instance. See [Importing content items](#importing-content-items) for details.

### Codename vs. ID vs External ID

Most methods of the SDK accept an *Identifier* object that specifies which content item, language variant or asset you want to perform the given operation on. There are 3 types of identification you can use to create the identifier: 

```csharp
var identifier = ContentItemIdentifier.ByCodename("on_roasts");
var identifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");
var identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
```

* **Codenames** are generated automatically by Kentico Cloud based on the object's name. They can make your code more readable but are not guaranteed to be unique. They should only be used in circumstances with no chance of naming conflicts. 
* (internal) **IDs** are random [GUIDs](https://en.wikipedia.org/wiki/Universally_unique_identifier) assigned to objects by Kentico Cloud at the moment of import/creation. They are unique, but only objects that are already in the system have them. You can't use them to refer to content that hasn't yet been imported. 
* **External IDs** are string-based custom identifiers defined by you. This is useful when importing a batch of cross-referencing content. See [Importing modular and linked content](#importing-modular-and-linked-content). 

## Quick start

### Importing content items

Importing content items is a 2 step process, using 2 separate methods:

1. Creating an empty content item which serves as a wrapper for your content.
2. Adding content inside a language variant of the content item.

Each content item can consist of several localized variants. **The content itself is always part of a specific language variant, even if your project only uses one language**. See our [Importing to Kentico Cloud](https://developer.kenticocloud.com/v1/docs/importing-to-kentico-cloud#section-importing-your-content) tutorial for a more detailed explanation. 

#### 1. Creating a content item

```csharp
// Create an instance of the Content Management client
var client = new ContentManagementClient(options);

// Define a content type of the imported item by its codename
var contentType = ContentTypeIdentifier.ByCodename("article");
// Define the imported content item
var item = new ContentItemCreateModel() { 
    Name = "On Roasts",
    Type = contentType,
    SitemapLocations = new[] { SitemapNodeIdentifier.ByCodename("articles") }
};

// Add your content item to your project in Kentico Cloud
var responseItem = await client.CreateContentItemAsync(item);
);
```

Kentico Cloud will generate an internal ID and codename for the (new and empty) content item and include it in the response. In the next step, we will add the actual (localized) content.


#### 2. Adding language variants

To add localized content, you have to specify: 

* The content item you are importing into.
* The language variant of the content item.
* The content elements of the language variant you want to insert or update. Omitted elements will remain unchanged. 

```csharp
var client = new ContentManagementClient(options);

protected static dynamic ELEMENTS = new {
    title = "On Roasts",
    post_date = new DateTime(2017, 7, 4),
    body_copy = @"
        <h1>Light Roasts</h1>
        <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color. This method
        is used for milder coffee  varieties and for coffee tasting. This type of roasting allows the natural
        characteristics of each coffee to show. The aroma of coffees produced from light roasts is usually
        more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p>
    ",
    related_articles = new [] { ContentItemIdentifier.ByCodename("which_brewing_fits_you_") },
    url_pattern = "on-roasts",
    personas = new [] { TaxonomyTermIdentifier.ByCodename("barista") },
};
var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = ELEMENTS };

// Specify the content item and the language varaint 
var itemIdentifier = ContentItemIdentifier.ByCodename("on_roasts");
var languageIdentifier = LanguageIdentifier.ByLanguageCodename("en-US");
var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

// Upsert a language variant of your content item
var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpsertModel);
```

### Importing assets

Importing assets using Content Management SDK is a 2-step process:

1. Uploading a file to Kentico Cloud.
2. Creating a new asset using the given file reference.

#### 1. Uploading a file 

```csharp
var client = new ContentManagementClient(options);

var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
var fileName = "Hello.txt";
var contentType = "text/plain";

var fileResult = await client.UploadFileAsync(stream, fileName, contentType);
```
Kentico Cloud will generate an internal id that serves as a pointer to your file. You will use it in the next step to create the actual asset. 

#### 2. Creating an asset 

Use the file reference you have obtained in the previous step to link the asset with your file. You can specify the asset description for each language in your project.

```csharp

var englishDescription = "Description of the asset in English Language";
var languageIdentifier = LanguageIdentifier.ByCodename("en-US");
var assetDescription = new AssetDescription { Description = englishDescription, Language = languageIdentifier };
var descriptions = new [] { assetDescription };

var asset = new AssetUpsertModel {
    FileReference = fileResult,
    Descriptions = descriptions
};
var externalId = "Ext-Asset-7483-txt";

var assetResult = await client.UpsertAssetByExternalIdAsync(externalId, asset);
```

### Importing modular and linked content

The content you are importing will often contain references to other pieces of imported content. A content item can reference assets or point to other content items used as modular content or links. To avoid having to import objects in a specific order (and solve problems with cyclical dependencies), you can use **external IDs** to reference non-existent (not-yet-imported) content: 

1. Define external IDs for all content you want to import in advance. 
2. When referencing another piece of content, use its external ID. 
3. Import your content (use upsert methods with external ID). All references will resolve at the end.

This way, you can import your content in any order and run the import process repeatedly to keep your project up to date. In the example below we import an asset and a content item that uses it: 

```csharp
// Upsert an asset, assuming you already have the fileResult reference
var asset = new AssetUpsertModel {
    FileReference = fileResult,
    Descriptions = new List<AssetDescription>();
};
var assetExternalId = "Ext-Asset-123-png";
var assetResult = await client.UpsertAssetByExternalIdAsync(assetExternalId, asset);

// Upsert a content item
var type = ContentTypeIdentifier.ByCodename("cafe");
var item = new ContentItemUpsertModel() { 
    Name = "Brno", 
    Type = type 
};
var itemExternalId = "Ext-Item-456-Brno";
var contentItemResponse = await client.UpsertContentItemByExternalIdAsync(itemExternalId, item);

//Upsert a language variant
var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = {
    picture = new ObjectIdentifier() { externalID = "Ext-Asset-123-png" },
    city = "Brno",
    country = "Czech Republic"
} };

var itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
var languageIdentifier = LanguageIdentifier.ByCodename("en-US");
var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpsertModel);
```

### Content item methods

#### Upserting a content item by external ID

```csharp
var type = ContentTypeIdentifier.ByCodename("cafe");
var item = new ContentItemUpsertModel() { 
    Name = "New or updated name",
    SitemapLocations = new[] { SitemapNodeIdentifier.ByCodename("cafes") },
    Type = type 
};

var contentItemResponse = await client.UpsertContentItemByExternalIdAsync("Ext-Item-456-Brno", item);
```

#### Adding a content item 

```csharp
var type = ContentTypeIdentifier.ByCodename("cafe");
var item = new ContentItemCreateModel() { 
    Name = "Brno",
    Type = type,
    SitemapLocations = new[] { SitemapNodeIdentifier.ByCodename("cafes") }
};

var responseItem = await client.CreateContentItemAsync(item);
```

#### Updating a content item

```csharp
var identifier = ContentItemIdentifier.ByCodename("brno");
// var identifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");

var item = new ContentItemUpdateModel() { 
    Name = "New name",
    SitemapLocations = new[] { 
        SitemapNodeIdentifier.ByCodename("cafes"),
        SitemapNodeIdentifier.ByCodename("europe")
    }
};

var contentItemReponse = await client.UpdateContentItemAsync(identifier, item);
```

#### Viewing a content item

```csharp
var identifier = ContentItemIdentifier.ByCodename("brno");
// var identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
// var identifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");

var contentItemReponse = await client.GetContentItemAsync(identifier);
```

#### Listing content items

All at once:
```csharp
 var response = await client.ListContentItemsAsync();
 ```

With continuation:
```csharp
var response = await client.ListContentItemsAsync();
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

#### Deleting a content item

```csharp
var identifier = ContentItemIdentifier.ByCodename("brno");
// var identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");
// var identifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");

client.DeleteContentItemAsync(identifier);
```
 
### Language variant methods

#### Upserting a language variant

```csharp
 var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = new {
    street = "Nove Sady 25",
    city = "Brno",
    country = "Czech Republic"
} };

var itemIdentifier = ContentItemIdentifier.ByCodename("brno");
// var itemIdentifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");
// var itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

var languageIdentifier = LanguageIdentifier.ByCodename("en-US");
// var languageIdentifier = LanguageIdentifier.ById("00000000-0000-0000-0000-000000000000");

var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpsertModel);
```

#### Viewing a language variant

```csharp
var itemIdentifier = ContentItemIdentifier.ByCodename("brno");
// var itemIdentifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");
// var itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

var languageIdentifier = LanguageIdentifier.ByCodename("en-US");
// var languageIdentifier = LanguageIdentifier.ById("00000000-0000-0000-0000-000000000000");

var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

var response = await client.GetContentItemVariantAsync(identifier);
```

#### Listing language variants

```csharp
var identifier = ContentItemIdentifier.ByCodename("brno");
// var identifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");
// var identifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

var responseVariants = await client.ListContentItemVariantsAsync(identifier);
```

#### Deleting a language variant

```csharp
var itemIdentifier = ContentItemIdentifier.ByCodename("brno");
// var itemIdentifier = ContentItemIdentifier.ById("8ceeb2d8-9676-48ae-887d-47ccb0f54a79");
// var itemIdentifier = ContentItemIdentifier.ByExternalId("Ext-Item-456-Brno");

var languageIdentifier = LanguageIdentifier.ByCodename("en-US");
// var languageIdentifier = LanguageIdentifier.ById("00000000-0000-0000-0000-000000000000");

await client.DeleteContentItemVariantAsync(identifier);
```


### Asset methods

#### Uploading a file 

```csharp
var client = new ContentManagementClient(options);

var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
var fileName = "Hello.txt";
var contentType = "text/plain";

var fileResult = await client.UploadFileAsync(stream, fileName, contentType);
```

#### Upserting an asset using external ID 
```csharp
var englishDescription = "Description of the asset in English Language";
var languageIdentifier = LanguageIdentifier.ByCodename("en-US");
var assetDescription = new AssetDescription { Description = englishDescription, Language = languageIdentifier };
var descriptions = new [] { assetDescription };

var asset = new AssetUpsertModel
{
    FileReference = fileResult,
    Descriptions = descriptions;
};
var externalId = "Ext-Asset-123-png";

var assetResult = await client.UpsertAssetByExternalIdAsync(externalId, asset);
```

#### Uploading an asset from a file system in a single step

```csharp 
var descriptions = new List<AssetDescription>();

var filePath = "‪C:\Users\Kentico\Desktop\puppies.png";
var contentType = "image/png";

var assetResult = await client.CreateAssetAsync(new FileContentSource(filePath, contentType), descriptions);
```

#### Listing assets

All at once:

```csharp 
var response = await client.ListAssetsAsync();
```

With continuation:

```csharp
var response = await client.ListAssetsAsync();

while (true)
{
    foreach (var asset in response)
    {
        // Use your asset
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
client.DeleteAssetAsync(AssetIdentifier.ById("fcbb12e6-66a3-4672-85d9-d502d16b8d9c"));
// client.DeleteAssetAsync(AssetIdentifier.ByExternalId("Ext-Asset-123-png"));
```

## Further information

For more developer resources, visit the Kentico Cloud Developer Hub at <https://developer.kenticocloud.com>.

### Building the sources

Prerequisites:

**Required:**
[.NET Core SDK](https://www.microsoft.com/net/download/core).

Optional:
* [Visual Studio 2017](https://www.visualstudio.com/vs/) for full experience
* or [Visual Studio Code](https://code.visualstudio.com/)

## Feedback & Contributing

Check out the [contributing](https://github.com/Kentico/content-management-sdk-net/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions, and begin contributing.
