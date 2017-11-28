using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Models.Shared;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementClientTests
    {
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
        private const string API_KEY = "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAidWlkIjogInVzcl8wdkZrVm9rczdyb2prRmR2Slkzc0ZQIiwNCiAgImp0aSI6ICIwYzUyMTYxZGIwZTU0MDU5YjMwZjUwMGUyMTgxYmU1NiIsDQogICJpYXQiOiAiMTUxMTE3ODYwMCIsDQogICJleHAiOiAiMTUxMzc3MDYwMCIsDQogICJwcm9qZWN0X2lkIjogImJiNjg4MmEwMzA4ODQwNWNhNmFjNGEwZGE0NjgxMGIwIiwNCiAgInZlciI6ICIyLjAuMCIsDQogICJwZXJtaXNzaW9ucyI6IFsNCiAgICAidmlldy1jb250ZW50IiwNCiAgICAiY29tbWVudCIsDQogICAgInVwZGF0ZS13b3JrZmxvdyIsDQogICAgInVwZGF0ZS1jb250ZW50IiwNCiAgICAicHVibGlzaCIsDQogICAgImNvbmZpZ3VyZS1zaXRlbWFwIiwNCiAgICAiY29uZmlndXJlLXRheG9ub215IiwNCiAgICAiY29uZmlndXJlLWNvbnRlbnRfdHlwZXMiLA0KICAgICJjb25maWd1cmUtd2lkZ2V0cyIsDQogICAgImNvbmZpZ3VyZS13b3JrZmxvdyIsDQogICAgIm1hbmFnZS1wcm9qZWN0cyIsDQogICAgIm1hbmFnZS11c2VycyIsDQogICAgImNvbmZpZ3VyZS1wcmV2aWV3LXVybCIsDQogICAgImNvbmZpZ3VyZS1jb2RlbmFtZXMiLA0KICAgICJhY2Nlc3MtYXBpLWtleXMiLA0KICAgICJtYW5hZ2UtYXNzZXRzIiwNCiAgICAibWFuYWdlLWxhbmd1YWdlcyIsDQogICAgIm1hbmFnZS13ZWJob29rcyIsDQogICAgIm1hbmFnZS10cmFja2luZyINCiAgXSwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0.qixIW1NOmyrbfsqoxzHG0NuMMg5yWtG9r0lpb5y31eo";
        private static Guid EXISTING_ITEM_ID = Guid.Parse("ddc8f48a-6df3-43c6-9933-0d4ea0b2c701");
        private const string EXISTING_ITEM_CODENAME = "introduction";
        private static Guid EXISTING_VARIANT_ID = Guid.Parse("5f148588-613f-8e32-c023-da82f1308ede");
        private const string EXISTING_VARIANT_CODENAME = "Another_language";
        private const string EXISTING_CONTENT_TYPE_CODENAME = "writeapi";
        private const string EXTERNAL_ID = "354136c543gj3154354j1g";

        private static Dictionary<string, object> ELEMENTS = new Dictionary<string, object> { { "name", "Martinko Klingacik44" } };
        private static ContentManagementOptions OPTIONS = new ContentManagementOptions() { ApiKey = API_KEY, ProjectId = PROJECT_ID };


        #region Item Variant

        [Fact]
        public async void UpsertVariantItemIdByLanguageId()
        {

            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = ELEMENTS };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageId(EXISTING_VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
        }

        [Fact]
        public async void UpsertVariantByItemCodenameLanguageId()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = ELEMENTS };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageId(EXISTING_VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantByItemIdLanguageCodename()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = ELEMENTS };

            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantByItemCodenameLanguageCodename()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = ELEMENTS };

            var itemIdentifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void UpsertVariantByItemExternalIdLanguageCodename()
        {
            var contentItemVariantUpdateModel = new ContentItemVariantUpdateModel() { Elements = ELEMENTS };

            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariant = await client.UpsertVariantAsync(identifier, contentItemVariantUpdateModel);
            Assert.False(responseVariant.Item.Id == Guid.Empty);
        }

        [Fact]
        public async void ListVariantsByItemId()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariants = await client.ListContentItemVariantsAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID, responseVariants[1].Item.Id);
        }

        [Fact]
        public async void ListVariantsByItemCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariants = await client.ListContentItemVariantsAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID, responseVariants[1].Item.Id);
        }

        [Fact]
        public async void ListVariantsByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var client = new ContentManagementClient(OPTIONS);
            var responseVariants = await client.ListContentItemVariantsAsync(identifier);
            Assert.Equal("cc601fe7-c057-5cb5-98d6-9ca24843b74a", responseVariants[1].Item.Id.ToString());
        }

        [Fact]
        public async void ViewVariantByItemIdLanguageId()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageId(EXISTING_VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var response = await client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void ViewVariantByLanguageCodename()
        {
            var itemIdentifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var response = await client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void ViewVariantByItemExternalIdLanguageCodename()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var response = await client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void ViewVariantByItemExternalIdLanguageId()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);
            var variantIdentifier = ContentVariantIdentifier.ByLanguageId(EXISTING_VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            var response = await client.GetContentItemVariantAsync(identifier);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void DeleteVariantByLanguageId()
        {
            var itemIdentifier = ContentItemIdentifier.ById(Guid.NewGuid());
            var variantIdentifier = ContentVariantIdentifier.ByLanguageId(EXISTING_VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteVariantByLanguageCodename()
        {
            var itemIdentifier = ContentItemIdentifier.ById(Guid.NewGuid());
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteVariantItemExternalIdLanguageId()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId("123456555");
            var variantIdentifier = ContentVariantIdentifier.ByLanguageId(EXISTING_VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        public async void DeleteVariantItemExternalIdLanguageCodename()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId("123456555");
            var variantIdentifier = ContentVariantIdentifier.ByLanguageCodename(EXISTING_VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemVariantAsync(identifier);
        }

        #endregion

        #region Item

        [Fact]
        public async void AddContentItem()
        {
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemPostModel() { Name = "Hooray!", Type = type };

            var client = new ContentManagementClient(OPTIONS);
            var responseItem = await client.AddContentItemAsync(item);
            Assert.Equal("Hooray!", responseItem.Name);
        }

        [Fact]
        public async void ListContentItems()
        {
            var client = new ContentManagementClient(OPTIONS);
            var response = await client.ListContentItemsAsync();
            Assert.True(response != null);
        }

        [Fact]
        public async void UpdateContentItemByCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);
            var sitemapLocation = new List<ManageApiReference>();
            var item = new ContentItemPutModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation };

            var client = new ContentManagementClient(OPTIONS);
            var contentItemReponse = await client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemById()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);
            var sitemapLocation = new List<ManageApiReference>();
            var item = new ContentItemPutModel() { Name = EXISTING_ITEM_CODENAME, SitemapLocations = sitemapLocation};

            var client = new ContentManagementClient(OPTIONS);
            var contentItemReponse = await client.UpdateContentItemAsync(identifier, item);
            Assert.Equal(EXISTING_ITEM_CODENAME, contentItemReponse.Name);
        }

        [Fact]
        public async void UpdateContentItemByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var sitemapLocation = new List<ManageApiReference>();
            var type = new ManageApiReference() { CodeName = EXISTING_CONTENT_TYPE_CODENAME };
            var item = new ContentItemUpsertModel() { Name = "Hooray!", SitemapLocations = sitemapLocation, Type = type };

            var client = new ContentManagementClient(OPTIONS);
            var contentItemResponse = await client.UpdateContentItemByExternalIdAsync(identifier, item);
            Assert.Equal("Hooray!", contentItemResponse.Name);
        }

        [Fact]
        public async void ViewContentItemById()
        {
            var identifier = ContentItemIdentifier.ById(EXISTING_ITEM_ID);

            var client = new ContentManagementClient(OPTIONS);
            var contentItemReponse = await client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID.ToString(), contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void ViewContentItemByCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename(EXISTING_ITEM_CODENAME);

            var client = new ContentManagementClient(OPTIONS);
            var contentItemReponse = await client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID.ToString(), contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void ViewContentItemByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId(EXTERNAL_ID);

            var client = new ContentManagementClient(OPTIONS);
            var contentItemReponse = await client.GetContentItemAsync(identifier);
            Assert.Equal("cc601fe7-c057-5cb5-98d6-9ca24843b74a", contentItemReponse.Id.ToString());
        }

        [Fact]
        public async void DeleteContentItemById()
        {
            var identifier = ContentItemIdentifier.ById(Guid.NewGuid());

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemByCodename()
        {
            var identifier = ContentItemIdentifier.ByCodename("some id");

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemAsync(identifier);
        }

        [Fact]
        public async void DeleteContentItemByExternalId()
        {
            var identifier = ContentItemIdentifier.ByExternalId("externalId");

            var client = new ContentManagementClient(OPTIONS);
            await client.DeleteContentItemAsync(identifier);
        }

        #endregion

        #region Binary files

        [Fact]
        public async void UploadFile_UploadsFile()
        {
            var client = new ContentManagementClient(OPTIONS);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK tests!"));

            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var fileResult = await client.UploadFile(stream, fileName, contentType);

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

            var assetResult = await client.UpsertAssetByExternalId(externalId, asset);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(stream.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);
        }

        #endregion
    }
}