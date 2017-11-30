using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Models;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementClientTests
    {
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
        private const string API_KEY = "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAidWlkIjogInVzcl8wdkZrVm9rczdyb2prRmR2Slkzc0ZQIiwNCiAgImp0aSI6ICIwYzUyMTYxZGIwZTU0MDU5YjMwZjUwMGUyMTgxYmU1NiIsDQogICJpYXQiOiAiMTUxMTE3ODYwMCIsDQogICJleHAiOiAiMTUxMzc3MDYwMCIsDQogICJwcm9qZWN0X2lkIjogImJiNjg4MmEwMzA4ODQwNWNhNmFjNGEwZGE0NjgxMGIwIiwNCiAgInZlciI6ICIyLjAuMCIsDQogICJwZXJtaXNzaW9ucyI6IFsNCiAgICAidmlldy1jb250ZW50IiwNCiAgICAiY29tbWVudCIsDQogICAgInVwZGF0ZS13b3JrZmxvdyIsDQogICAgInVwZGF0ZS1jb250ZW50IiwNCiAgICAicHVibGlzaCIsDQogICAgImNvbmZpZ3VyZS1zaXRlbWFwIiwNCiAgICAiY29uZmlndXJlLXRheG9ub215IiwNCiAgICAiY29uZmlndXJlLWNvbnRlbnRfdHlwZXMiLA0KICAgICJjb25maWd1cmUtd2lkZ2V0cyIsDQogICAgImNvbmZpZ3VyZS13b3JrZmxvdyIsDQogICAgIm1hbmFnZS1wcm9qZWN0cyIsDQogICAgIm1hbmFnZS11c2VycyIsDQogICAgImNvbmZpZ3VyZS1wcmV2aWV3LXVybCIsDQogICAgImNvbmZpZ3VyZS1jb2RlbmFtZXMiLA0KICAgICJhY2Nlc3MtYXBpLWtleXMiLA0KICAgICJtYW5hZ2UtYXNzZXRzIiwNCiAgICAibWFuYWdlLWxhbmd1YWdlcyIsDQogICAgIm1hbmFnZS13ZWJob29rcyIsDQogICAgIm1hbmFnZS10cmFja2luZyINCiAgXSwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0.qixIW1NOmyrbfsqoxzHG0NuMMg5yWtG9r0lpb5y31eo";
        private static Guid EXISTING_ITEM_ID = Guid.Parse("ddc8f48a-6df3-43c6-9933-0d4ea0b2c701");
        private const string EXISTING_ITEM_CODENAME = "introduction";
        private static Guid EXISTING_LANGUAGE_ID = Guid.Parse("5f148588-613f-8e32-c023-da82f1308ede");
        private const string EXISTING_LANGUAGE_CODENAME = "Another_language";
        private const string EXISTING_CONTENT_TYPE_CODENAME = "writeapi";
        private const string EXTERNAL_ID = "354136c543gj3154354j1g";

        private static Dictionary<string, object> _elements = new Dictionary<string, object> { { "name", "Martinko Klingacik44" } };
        private static ContentManagementOptions _options = new ContentManagementOptions() { ApiKey = API_KEY, ProjectId = PROJECT_ID };
        private static ContentManagementClient _client = new ContentManagementClient(_options);

        #region Item Variant

        [Fact]
        public async void UpsertVariantAsync_ById_LanguageId_UpdatesVariant()
        {

            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpdateModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
        }

        [Fact]
        public async void UpsertVariantAsync_ByCodename_LanguageId_UpdatesVariant()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpdateModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantAsync_ById_LanguageCodename_UpdatesVariant()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpdateModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }
        
        [Fact]
        public async void UpsertVariantAsync_ByCodename_LanguageCodename_UpdatesVariant()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpdateModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantAsync_ByExternalId_LanguageCodename_UpdatesVariant()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpdateModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantAsync_ByExternalId_LanguageId_UpdatesVariant()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpdateModel);

            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void ListContentItemVariantsAsync_ById_ListsVariants()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants[1].Item.Id);
        }

        [Fact]
        public async void ListContentItemVariantsAsync_ByCodename_ListsVariants()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants[1].Item.Id);
        }

        [Fact]
        public async void ListContentItemVariantsAsync_ByExternalId_ListsVariants()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);

            Assert.Equal("cc601fe7-c057-5cb5-98d6-9ca24843b74a", responseVariants[1].Item.Id.ToString());
        }

        [Fact]
        public async void GetContentItemVariantAsync_ById_LanguageId_GetsVariant()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
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
            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.True(response.Item.Id != Guid.Empty);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async void GetContentItemVariantAsync_ByExternalId_ReturnsVariant()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.True(response.Item.Id != Guid.Empty);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async void DeleteContentItemVariant_ByCodename_LanguageId_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await PrepareItemToDelete();
            await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByCodename(itemResponse.CodeName);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemVariant_ByCodename_LanguageCodename_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await PrepareItemToDelete();
            await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByCodename(itemResponse.CodeName);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemVariant_ById_LanguageCodename_DeletesVariant()
        {
            var itemResponse = await PrepareItemToDelete();
            await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ById(itemResponse.Id);
            var languageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemVariant_ByExternalId_LanguageId_DeletesVariant()
        {
            var externalId = Guid.NewGuid().ToString();
            var itemResponse = await PrepareItemToDelete(externalId);
            await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var languageIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemVariant_ByExternalId_LanguageCodename_DeletesVariant()
        {
            var externalId = Guid.NewGuid().ToString();
            var itemResponse = await PrepareItemToDelete(externalId);
            await PrepareVariantToDelete(itemResponse);

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
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
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
            var sitemapLocation = new List<ManageApiReference>();
            var item = new ContentItemUpdateModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation };

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemAsynx_ById_UpdatesContentItem()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var sitemapLocation = new List<ManageApiReference>();
            var item = new ContentItemUpdateModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation};

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpsertContentItemByExternalIdAsync_UpdatesContentItem()
        {
            var sitemapLocation = new List<ManageApiReference>();
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemUpsertModel() { Name = "Hooray!", SitemapLocations = sitemapLocation, Type = type };

            var contentItemResponse = await _client.UpsertContentItemByExternalIdAsync(EXTERNAL_ID, item);
            Assert.Equal("Hooray!", contentItemResponse.Name);
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
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal("cc601fe7-c057-5cb5-98d6-9ca24843b74a", contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void DeleteContentAsync_ById_DeletesContentItem()
        {
            var itemToDelete = await PrepareItemToDelete();

            var identifier = ContentItemIdentifier.ById(itemToDelete.Id);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        public async void DeleteContentAsync_ByCodename_DeletesContentItem()
        {
            var itemToDelete = await PrepareItemToDelete();

            var identifier = ContentItemIdentifier.ByCodename(itemToDelete.CodeName);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        public async void DeleteContentAsync_ByExternalId_DeletesContentItem()
        {
            var itemToDelete = await PrepareItemToDelete();

            var identifier = ContentItemIdentifier.ByExternalId(itemToDelete.ExternalId);

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
        public async void UploadFileAsync_AddAssetAsync_UploadsFile_CreatesAssetCorrectly()
        {
            var content = "Hello world from CM API .NET SDK test CreateAsset_UploadsFile_CreatesAssetCorrectly!" + "X".PadLeft((int)new Random().NextDouble() * 100, 'X');
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var fileResult = await _client.UploadFileAsync(stream, fileName, contentType);

            Assert.NotNull(fileResult);
            Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

            Guid fileId;
            Assert.True(Guid.TryParse(fileResult.Id, out fileId));

            Assert.NotEqual(Guid.Empty, fileId);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = new List<AssetDescriptionsModel>()
            };

            var assetResult = await _client.AddAssetAsync(asset);

            Assert.NotNull(assetResult);
            Assert.Null(assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(stream.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }

        [Fact]
        public async void UploadFileAsync_UpsertAssetByExternalIdAsync_UploadsFile_CreatesAssetCorrectly()
        {
            var content = "Hello world from CM API .NET SDK test CreateAsset_WithExternalId_UploadsFile_CreatesAssetCorrectly! " + "X".PadLeft((int)new Random().NextDouble() * 100, 'X');
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var fileResult = await _client.UploadFileAsync(stream, fileName, contentType);

            Assert.NotNull(fileResult);
            Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

            Guid fileId;
            Assert.True(Guid.TryParse(fileResult.Id, out fileId));

            Assert.NotEqual(Guid.Empty, fileId);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = new List<AssetDescriptionsModel>()
            };
            var externalId = Guid.NewGuid().ToString();

            var assetResult = await _client.UpsertAssetByExternalIdAsync(externalId, asset);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(stream.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        #endregion

        #region Utils

        private async Task<ContentItemModel> PrepareItemToDelete(string externalId = null)
        {
            var externalIdentifier = string.IsNullOrEmpty(externalId) ? Guid.NewGuid().ToString() : externalId;
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemCreateModel() { Name = "Hooray!", Type = type, ExternalId = externalIdentifier };

            return await _client.CreateContentItemAsync(item);
        }

        private async Task<ContentItemVariantModel> PrepareVariantToDelete(ContentItemModel item)
        {
            var addedItemIdentifier = ContentItemIdentifier.ByCodename(item.CodeName);
            var addedLanguageIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var addedContentItemLanguageIdentifier = new ContentItemVariantIdentifier(addedItemIdentifier, addedLanguageIdentifier);
            var variantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            return await _client.UpsertContentItemVariantAsync(addedContentItemLanguageIdentifier, variantUpdateModel);
        }

        #endregion
    }
}