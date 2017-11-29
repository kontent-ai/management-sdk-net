using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;

using Xunit;
using KenticoCloud.ContentManagement.Models;

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
        public async void UpsertVariantItemIdByLanguageId()
        {

            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var responseVariant = await _client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
        }

        [Fact]
        public async void UpsertVariantByItemCodenameLanguageId()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var variantIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var responseVariant = await _client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantByItemIdLanguageCodename()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var responseVariant = await _client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantByItemCodenameLanguageCodename()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var responseVariant = await _client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantByItemExternalIdLanguageCodename()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var responseVariant = await _client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void ListVariantsByItemId()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID, responseVariants[1].Item.Id);
        }

        [Fact]
        public async void ListVariantsByItemCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID, responseVariants[1].Item.Id);
        }

        [Fact]
        public async void ListVariantsByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var responseVariants = await _client.ListContentItemVariantsAsync(identifier);
            Assert.Equal("cc601fe7-c057-5cb5-98d6-9ca24843b74a", responseVariants[1].Item.Id.ToString());
        }

        [Fact]
        public async void ViewVariantByItemIdLanguageId()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void ViewVariantByLanguageCodename()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void ViewVariantByItemExternalIdLanguageCodename()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void ViewVariantByItemExternalIdLanguageId()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var variantIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var response = await _client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void DeleteVariantByLanguageId()
        {
            // Prepare item
            var itemResponse = await PrepareItemToDelete();

            var variantUpdateModel = await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ById(itemResponse.Id);
            var variantIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteVariantByLanguageCodename()
        {
            var itemResponse = await PrepareItemToDelete();
            var variantUpdateModel = await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ById(itemResponse.Id);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteVariantItemExternalIdLanguageId()
        {
            var externalId = Guid.NewGuid().ToString();
            var itemResponse = await PrepareItemToDelete(externalId);
            var variantResponse = await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var variantIdentifier = LanguageIdentifier.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteVariantItemExternalIdLanguageCodename()
        {
            var externalId = Guid.NewGuid().ToString();
            var itemResponse = await PrepareItemToDelete(externalId);
            await PrepareVariantToDelete(itemResponse);

            var itemIdentifier = ContentItemIdentifier.ByExternalId(externalId);
            var variantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            await _client.DeleteContentItemVariantAsync(identifier);
        }

        #endregion

        #region Item

        [Fact]
        public async void AddContentItem()
        {
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemPostModel() { Name = "Hooray!", Type = type };

            var responseItem = await _client.AddContentItemAsync(item);
            Assert.Equal("Hooray!", responseItem.Name);
        }

        [Fact]
        public async void ListContentItems()
        {
            var response = await _client.ListContentItemsAsync();
            Assert.True(response != null);
        }

        [Fact]
        public async void ListContentItems_WithContinuation()
        {
            Thread.Sleep(1000);

            var client = new ContentManagementClient(OPTIONS);

            var response = await client.ListContentItemsAsync();
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
        public async void UpdateContentItemByCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var sitemapLocation = new List<ManageApiReference>();
            var item = new ContentItemPutModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation };

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemById()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var sitemapLocation = new List<ManageApiReference>();
            var item = new ContentItemPutModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation};

            var contentItemReponse = await _client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var sitemapLocation = new List<ManageApiReference>();
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemUpsertModel() { Name = "Hooray!", SitemapLocations = sitemapLocation, Type = type };

            var contentItemResponse = await _client.UpdateContentItemByExternalIdAsync(identifier, item);
            Assert.Equal("Hooray!", contentItemResponse.Name);
        }

        [Fact]
        public async void ViewContentItemById()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID.ToString(), contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void ViewContentItemByCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID.ToString(), contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void ViewContentItemByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var contentItemReponse = await _client.GetContentItemAsync(identifier);
            Assert.Equal("cc601fe7-c057-5cb5-98d6-9ca24843b74a", contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void DeleteContentItemById()
        {
            var itemToDelete = await PrepareItemToDelete();

            var identifier = ContentItemIdentifier.ById(itemToDelete.Id);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemByCodename()
        {
            var itemToDelete = await PrepareItemToDelete();

            var identifier = ContentItemIdentifier.ByCodename(itemToDelete.CodeName);

            await _client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemByExternalId()
        {
            var itemToDelete = await PrepareItemToDelete();

            var identifier = ContentItemIdentifier.ByExternalId(itemToDelete.ExternalId);

            await _client.DeleteContentItemAsync(identifier);
        }

        #endregion

        #region Assets

        [Fact]
        public async void ListAssets()
        {
            Thread.Sleep(1000);

            var client = new ContentManagementClient(OPTIONS);
            var response = await client.ListAssets();
            Assert.True(response != null);
        }

        [Fact]
        public async void ListAssets_WithContinuation()
        {
            Thread.Sleep(1000);

            var client = new ContentManagementClient(OPTIONS);

            var response = await client.ListAssets();
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
        public async void CreateAsset_WithFile_CreatesAssetCorrectly()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK tests!"));

            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var fileResult = await _client.UploadFile(stream, fileName, contentType);

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
            var externalId = "Hello";

            var assetResult = await _client.UpsertAssetByExternalId(externalId, asset);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(stream.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);
        }

        #endregion

        #region Utils

        private async Task<ContentItemResponseModel> PrepareItemToDelete(string externalId = null)
        {
            var externalIdentifier = string.IsNullOrEmpty(externalId) ? Guid.NewGuid().ToString() : externalId;
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemPostModel() { Name = "Hooray!", Type = type, ExternalId = externalIdentifier };

            return await _client.AddContentItemAsync(item);
        }

        private async Task<ContentItemVariantResponseModel> PrepareVariantToDelete(ContentItemResponseModel item)
        {
            var addedItemIdentifier = ContentItemIdentifier.ByCodename(item.CodeName);
            var addedVariantIdentifier = LanguageIdentifier.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var addedContentItemVariantIdentifier = new ContentItemVariantIdentifier(addedItemIdentifier, addedVariantIdentifier);
            var variantUpdateModel = new ContentItemVariantUpdateModel() { Elements = _elements };

            return await _client.UpsertVariantAsync(addedContentItemVariantIdentifier, variantUpdateModel);
        }

        #endregion
    }
}