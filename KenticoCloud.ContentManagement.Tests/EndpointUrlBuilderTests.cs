using System;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class EndpointUrlBuilderTests
    {
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
        private const string API_KEY = "SomeFancyApiKey";
        private const string ENDPOINT = "https://manage.kenticocloud.com";

        private static ContentManagementOptions OPTIONS = new ContentManagementOptions() { ProjectId = PROJECT_ID };

        private const string ITEM_ID = "{ITEM_ID}";
        private const string ITEM_CODENAME = "{ITEM_CODENAME}";
        private const string ITEM_EXTERNAL_ID = "{ITEM_EXTERNAL_ID}";

        private const string VARIANT_ID = "{VARIANT_ID}";
        private const string VARIANT_CODENAME = "{VARIANT_CODENAME}";

        private EndpointUrlBuilder _builder;

        public EndpointUrlBuilderTests()
        {
            _builder = new EndpointUrlBuilder(OPTIONS);
        }

        #region Variants

        [Fact]
        public void BuildListVariantsUrl_ItemId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemIdentifier() { ItemId = ITEM_ID};
            var actualUrl = _builder.BuildListVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}";
        }

        [Fact]
        public void BuildListVariantsUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemIdentifier() { ItemCodename = ITEM_CODENAME };
            var actualUrl = _builder.BuildListVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}";
        }

        [Fact]
        public void BuildListVariantsUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemIdentifier() { ItemExternalId = ITEM_EXTERNAL_ID };
            var actualUrl = _builder.BuildListVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}";
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemIdVariantId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemVariantIdentifier() { ItemId = ITEM_ID, LanguageId = VARIANT_ID };
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemIdVariantCodename_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemVariantIdentifier() { ItemId = ITEM_ID, LanguageCodename = VARIANT_CODENAME };
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemCodenameVariantId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemVariantIdentifier() { ItemCodename = ITEM_CODENAME, LanguageId = VARIANT_ID };
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemCodenameVariantCodename_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemVariantIdentifier() { ItemCodename = ITEM_CODENAME, LanguageCodename = VARIANT_CODENAME };
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemExternalIdVariantId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemVariantIdentifier() { ItemExternalId = ITEM_EXTERNAL_ID, LanguageId = VARIANT_ID };
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemExternalIdVariantCodename_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemVariantIdentifier() { ItemExternalId = ITEM_EXTERNAL_ID, LanguageCodename = VARIANT_CODENAME };
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        #endregion

        #region Items Url

        [Fact]
        public void BuildItemsUrl_ReturnsCorrectUrl()
        {
            var actualUrl = _builder.BuildItemsUrl();
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items";

            Assert.Equal(expectedUrl, actualUrl);
        }

        #endregion

        #region Item Url

        [Fact]
        public void BuildItemUrl_ItemId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemIdentifier(ITEM_ID, null, null);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemIdentifier(null, ITEM_CODENAME, null);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = new ContentItemIdentifier(null, null, ITEM_EXTERNAL_ID);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_NoIdentifier_ThrowsEx()
        {
            var identifier = new ContentItemIdentifier(null, null, null);

            Assert.ThrowsAny<ArgumentException>(() => _builder.BuildItemUrl(identifier));
        }

        #endregion

        #region Assets

        [Fact]
        public void BuildAssetListingUrl_WithoutContinuationToken_ReturnsExpectedUrl()
        {
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/assets";
            var actualResult = _builder.BuildAssetListingUrl();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildAssetListingUrl_WithContinuationToken_ReturnsExpectedUrl()
        {
            var continuationToken = "MjA=";
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/assets?continuationToken=MjA%3d";
            var actualResult = _builder.BuildAssetListingUrl(continuationToken);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildAssetsUrlFromId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var assetId = "fcbb12e6-66a3-4672-85d9-d502d16b8d9c";
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/assets/{assetId}";
            var actualResult = _builder.BuildAssetsUrlFromId(assetId);

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void BuildAssetsUrlFromExternalId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var externalId = "which-brewing-fits-you";
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/assets/external-id/{externalId}";
            var actualResult = _builder.BuildAssetsUrlFromExternalId(externalId);

            Assert.Equal(expectedResult, actualResult);
        }

        #endregion

        #region File

        [Fact]
        public void BuildUploadFileUrl_ReturnsExpectedUrl()
        {
            var fileName = "which-brewing-fits-you-1080px.jpg";
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/files/{fileName}";
            var actualResult = _builder.BuildUploadFileUrl(fileName);

            Assert.Equal(expectedResult, actualResult);
        }

        #endregion
    }
}
