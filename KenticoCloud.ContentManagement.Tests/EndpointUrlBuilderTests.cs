using System;

using KenticoCloud.ContentManagement.Models.Items;

using Xunit;
using KenticoCloud.ContentManagement.Models.Assets;

namespace KenticoCloud.ContentManagement.Tests
{
    public class EndpointUrlBuilderTests
    {
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
        private const string API_KEY = "SomeFancyApiKey";
        private const string ENDPOINT = "https://manage.kenticocloud.com";

        private static ContentManagementOptions OPTIONS = new ContentManagementOptions() { ProjectId = PROJECT_ID };

        private static Guid ITEM_ID = Guid.Parse("b444004b-a4c4-43e3-94e0-d5bbd49d6cb8");
        private const string ITEM_CODENAME = "{ITEM_CODENAME}";
        private const string ITEM_EXTERNAL_ID = "{ITEM_EXTERNAL_ID}";

        private static Guid VARIANT_ID = Guid.Parse("5a64af00-a98d-4d2e-adb6-45120cbc0242");
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
            var identifier = ContentItemIdentifier.ById(ITEM_ID);
            var actualUrl = _builder.BuildListVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}";
        }

        [Fact]
        public void BuildListVariantsUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = ContentItemIdentifier.ByCodename(ITEM_CODENAME);
            var actualUrl = _builder.BuildListVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}";
        }

        [Fact]
        public void BuildListVariantsUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = ContentItemIdentifier.ByExternalId(ITEM_EXTERNAL_ID);
            var actualUrl = _builder.BuildListVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}";
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemIdVariantId_ReturnsCorrectUrl()
        {
            var itemIdentifier = ContentItemIdentifier.ById(ITEM_ID);
            var variantIdentifier = LanguageIdentifier.ById(VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemIdVariantCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = ContentItemIdentifier.ById(ITEM_ID);
            var variantIdentifier = LanguageIdentifier.ByCodename(VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemCodenameVariantId_ReturnsCorrectUrl()
        {
            var itemIdentifier = ContentItemIdentifier.ByCodename(ITEM_CODENAME);
            var variantIdentifier = LanguageIdentifier.ById(VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemCodenameVariantCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = ContentItemIdentifier.ByCodename(ITEM_CODENAME);
            var variantIdentifier = LanguageIdentifier.ByCodename(VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemExternalIdVariantId_ReturnsCorrectUrl()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(ITEM_EXTERNAL_ID);
            var variantIdentifier = LanguageIdentifier.ById(VARIANT_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildContentItemVariantsUrl_ItemExternalIdVariantCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = ContentItemIdentifier.ByExternalId(ITEM_EXTERNAL_ID);
            var variantIdentifier = LanguageIdentifier.ByCodename(VARIANT_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, variantIdentifier);
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
            var identifier = ContentItemIdentifier.ById(ITEM_ID);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = ContentItemIdentifier.ByCodename(ITEM_CODENAME);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = ContentItemIdentifier.ByExternalId(ITEM_EXTERNAL_ID);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{ITEM_EXTERNAL_ID}";

            Assert.Equal(expectedUrl, actualUrl);
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
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/assets?continuationToken=MjA%3D";
            var actualResult = _builder.BuildAssetListingUrl(continuationToken);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildAssetsUrlFromId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var assetId = Guid.NewGuid();
            var expectedResult = $"https://manage.kenticocloud.com/projects/{PROJECT_ID}/assets/{assetId}";
            var actualResult = _builder.BuildAssetsUrl(AssetIdentifier.ById(assetId));

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
