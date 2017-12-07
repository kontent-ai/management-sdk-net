using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementClientTests : ClientTestsBase
    {
        private static ContentManagementClient _client;

        /// <summary>
        /// Setup
        /// </summary>
        public ContentManagementClientTests()
        {
            _client = GetContentManagementClient(TestRunType.MockFromFileSystem);
        }

        #region Item Variant

        [Fact]
        public async void UpsertVariantAsync_ById_LanguageId_UpdatesVariant()
        {
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
        }

        [Fact]
        public async void UpsertVariantAsync_ByCodename_LanguageId_UpdatesVariant()
        {
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantAsync_ById_LanguageCodename_UpdatesVariant()
        {
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantAsync_ByCodename_LanguageCodename_UpdatesVariant()
        {
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantAsync_ByExternalId_LanguageCodename_UpdatesVariant()
        {
            var externalId = "348052a5ad8c44ddac1e9683923d74a5";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };
            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.True(responseVariant.Item.Id != Guid.Empty);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void UpsertVariantAsync_ByExternalId_LanguageId_UpdatesVariant()
        {
            // Arrange
            var externalId = "d5e050980baa43b085b909cdea4c6d2b";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void UpsertVariantAsync_UsingResponseModel_UpdatesVariant()
        {
            // Arrange
            var externalId = "4357b71d21eb45369d54a635faf7672b";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var emptyElements = new Dictionary<string, object>();
            var preparedVariant = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, emptyElements, preparedItem);

            // Test
            preparedVariant.Elements = _elements;
            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, preparedVariant);

            Assert.Equal(_elements["title"], responseVariant.Elements["title"]);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void ListContentItemVariantsAsync_ById_ListsVariants()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants.ToList()[1].Item.Id);
        }

        [Fact]
        public async void ListContentItemVariantsAsync_ByCodename_ListsVariants()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants.ToList()[1].Item.Id);
        }

        [Fact]
        public async void ListContentItemVariantsAsync_ByExternalId_ListsVariants()
        {
            // Arrange
            var externalId = "0220e6ec5b77401ea113b5273c8cdd5e";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem); 

            // Test
            var identifier = ContentItemIdentifier.ByExternalId(externalId);
            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);

            Assert.True(responseVariants.ToList().Count > 0);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ById_LanguageId_GetsVariant()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ById_LanguageCodeName_GetsVariant()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ByCodename_LanguageId_GetsVariant()
        {
            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ByCodename_LanguageCodeName_GetsVariant()
        {
            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ByExternalId_LanguageCodename_GetsVariant()
        {
            // Arrange
            var externalId = "f9cfaa3e00f64e22a144fdacf4cba3e5";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.True(response.Item.Id != Guid.Empty);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ByExternalId_ReturnsVariant()
        {
            var externalId = "ad66f70ed9bb4b8694116c9119c4a930";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.True(response.Item.Id != Guid.Empty);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void DeleteContentItemVariant_ByExternalId_LanguageId_DeletesVariant()
        {
            var externalId = "90285b1a983c43299638c8a835f16b81";
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemVariant_ByExternalId_LanguageCodename_DeletesVariant()
        {
            var externalId = "f4fe87222b6b46739bc673f6e5165c12";
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        #endregion

        #region Item

        [Fact]
        public async void AddContentItemAsync_AddsContentItem()
        {
            var type = ContentTypeIdentifier.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);
            var item = new ContentItemCreateModel() { Name = "Hooray!", Type = type };

            var responseItem = await _client.CreateContentItemAsync(item);
            Assert.Equal("Hooray!", responseItem.Name);
        }

        [Fact]
        public async void ListContentItemsAsync_ListsContentItems()
        {
            var response = await _client.ListContentItemsAsync();
            Assert.True(response != null);
        }

        [Fact]
        public async void ListContentItemsAsync_WithContinuation_ListsAllContentItems()
        {
            var response = await _client.ListContentItemsAsync();
            Assert.NotNull(response);

            while (true)
            {
                foreach (var item in response)
                {
                    Assert.NotNull(item);
                }

                if (!response.HasNextPage())
                {
                    break;
                }
                response = await response.GetNextPage();
                Assert.NotNull(response);
            }
        }

        [Fact]
        public async void UpdateContentItemAsync_ByCodename_UpdatesContentItem()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var sitemapLocation = new List<SitemapNodeIdentifier>();
            var item = new ContentItemUpdateModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation };

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemAsynx_ById_UpdatesContentItem()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var sitemapLocation = new List<SitemapNodeIdentifier>();
            var item = new ContentItemUpdateModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation };

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemAsynx_UsingResponseModel_UpdatesContentItem()
        {
            // Arrange
            var externalId = "093afb41b0614a908c8734d2bb840210";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            preparedItem.Name = "EditedItem";
            var identifier = ContentItemIdentifier.ByExternalId(externalId);
            var item = _client.UpdateContentItemAsync(identifier, preparedItem);

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, preparedItem);
            Assert.Equal("EditedItem", contentItemReponse.Name);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void UpsertContentItemByExternalIdAsync_UpdatesContentItem()
        {
            // Arrange
            var externalId = "753f6e965f4d49e5a120ca9a23551b10";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var sitemapLocation = new List<SitemapNodeIdentifier>();
            var type = ContentTypeIdentifier.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);
            var item = new ContentItemUpsertModel() { Name = "Hooray!", SitemapLocations = sitemapLocation, Type = type };

            var contentItemResponse = await _client.UpsertContentItemByExternalIdAsync(externalId, item);
            Assert.Equal("Hooray!", contentItemResponse.Name);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void GetContentItemAsync_ById_GetsContentItem()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID.ToString(), contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void GetContentItemAsync_ByCodename_GetsContentItem()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID.ToString(), contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void GetContentItemAsync_ByExternalId_GetsContentItem()
        {
            // Arrange
            var externalId = "e5a8de5b584f4182b879c78b696dff09";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var identifier = ContentItemIdentifier.ByExternalId(externalId);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal(externalId, contentItemReponse.ExternalId);

            // Cleanup
            var itemToClean = ContentItemIdentifier.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void DeleteContentAsync_ByExternalId_DeletesContentItem()
        {
            var externalId = "341bcf72988d49729ec34c8682710536";
            var itemToDelete = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            var identifier = ContentItemIdentifier.ByExternalId(externalId);

            await _client.DeleteContentItemAsync(identifier);
        }

        #endregion

        #region Assets

        [Fact]
        public async void ListAssetsAsync_ListsAssets()
        {
            var response = await _client.ListAssetsAsync();
            Assert.True(response != null);
        }

        [Fact]
        public async void ListAssets_WithContinuation_ListsAllAssets()
        {
            var response = await _client.ListAssetsAsync();
            Assert.NotNull(response);

            while (true)
            {
                foreach (var asset in response)
                {
                    Assert.NotNull(asset);
                }

                if (!response.HasNextPage())
                {
                    break;
                }
                response = await response.GetNextPage();
                Assert.NotNull(response);
            }
        }

        [Fact]
        public async void UploadFileAsync_WithStream_CreateAssetAsync_UploadsFile_CreatesAssetCorrectly()
        {
            var content = "Hello world from CM API .NET SDK test UploadFileAsync_CreateAssetAsync_UploadsFile_CreatesAssetCorrectly!" + "X".PadLeft((int)new Random().NextDouble() * 100, 'X');

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var fileName = "Hello.txt";
                var contentType = "text/plain";

                var fileResult = await _client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));

                Assert.NotNull(fileResult);
                Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

                Guid fileId;
                Assert.True(Guid.TryParse(fileResult.Id, out fileId));

                Assert.NotEqual(Guid.Empty, fileId);

                var asset = new AssetUpsertModel
                {
                    FileReference = fileResult,
                    Descriptions = new List<AssetDescription>()
                };

                var assetResult = await _client.CreateAssetAsync(asset);

                Assert.NotNull(assetResult);
                Assert.Null(assetResult.ExternalId);
                Assert.Equal(contentType, assetResult.Type);
                Assert.Equal(content.Length, assetResult.Size);
                Assert.NotNull(assetResult.LastModified);
                Assert.Equal(fileName, assetResult.FileName);
                Assert.NotNull(assetResult.Descriptions);

                // Cleanup
                await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
            }
        }

        [Fact]
        public async void UploadFileAsync_WithByteArray_UpsertAssetByExternalIdAsync_UploadsFile_CreatesAssetCorrectly()
        {
            var content = "Hello world from CM API .NET SDK test UploadFileAsync_UpsertAssetByExternalIdAsync_UploadsFile_CreatesAssetCorrectly! " + "X".PadLeft((int)new Random().NextDouble() * 100, 'X');

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var fileResult = await _client.UploadFileAsync(new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType));

            Assert.NotNull(fileResult);
            Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

            Guid fileId;
            Assert.True(Guid.TryParse(fileResult.Id, out fileId));

            Assert.NotEqual(Guid.Empty, fileId);

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new List<AssetDescription>() { assetDescription };

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = descriptions
            };
            var externalId = "99877608d1f6448ebb35778f027c92f6";

            var assetResult = await _client.UpsertAssetByExternalIdAsync(externalId, asset);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        [Fact]
        public async void CreateAssetAsync_WithFile_UploadsFile_CreatesAssetCorrectly()
        {
            var content = "Hello world from CM API .NET SDK test CreateAssetAsync_WithFile_UploadsFile_CreatesAssetCorrectly!" + "X".PadLeft((int)new Random().NextDouble() * 100, 'X');

            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new List<AssetDescription>() { assetDescription };

            var assetResult = await _client.CreateAssetAsync(new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType), descriptions);

            Assert.NotNull(assetResult);
            Assert.Null(assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }


        [Fact]
        public async void CreateAssetAsync_WithFile_FromFileSystem_UploadsFile_CreatesAssetCorrectly()
        {
            var descriptions = new List<AssetDescription>();

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data\\kentico_rgb_bigger.png");
            var contentType = "image/png";

            var assetResult = await _client.CreateAssetAsync(new FileContentSource(filePath, contentType), descriptions);

            Assert.NotNull(assetResult);
            Assert.Null(assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(new FileInfo(filePath).Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal("kentico_rgb_bigger.png", assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }



        [Fact]
        public async void UpsertAssetByExternalIdAsync_WithFile_FromByteArray_UploadsFile_CreatesAssetCorrectly()
        {
            var content = "Hello world from CM API .NET SDK test UploadFileAsync_UpsertAssetByExternalIdAsync_UploadsFile_CreatesAssetCorrectly!";

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var externalId = "5bec7f21ad2e44bb8a3a1f4a6a5bf8ca";

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new List<AssetDescription>() { assetDescription };

            var assetResult = await _client.UpsertAssetByExternalIdAsync(externalId, new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType), descriptions);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        [Fact]
        public async void UpdateAssetByIdAsync_ReturnsUpdatedAsset()
        {
            var id = AssetIdentifier.ById(Guid.Parse("512047f1-2f7f-45fd-9e90-e71b8feae017"));
            var updatedDescription = new AssetDescription()
            {
                Language = LanguageIdentifier.DEFAULT_LANGUAGE,
                Description = "Dancing Goat Café - Los Angeles - UPDATED",
            };
            var update = new AssetUpdateModel() { Descriptions = new List<AssetDescription>() { updatedDescription } };

            var response = await _client.UpdateAssetAsync(id, update);

            Assert.Equal(response.Id.ToString(), id.Id.ToString());
        }


        [Fact]
        public async void GetAssetAsync_WhenGivenAssetId_ReturnsGivenAsset()
        {
            var id = AssetIdentifier.ById(Guid.Parse("512047f1-2f7f-45fd-9e90-e71b8feae017"));
            
            var response = await _client.GetAssetAsync(id);

            Assert.Equal(response.Id.ToString(), id.Id.ToString());
        }


        #endregion
    }
}