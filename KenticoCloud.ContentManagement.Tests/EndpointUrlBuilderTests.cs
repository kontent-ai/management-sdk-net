using Xunit;


namespace KenticoCloud.ContentManagement.Tests
{
    public class EndpointUrlBuilderTests
    {
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";

        private static ContentManagementOptions OPTIONS = new ContentManagementOptions() { ProjectId = PROJECT_ID };

        private const string ITEM_ID = "cc601fe7-c057-5cb5-98d6-9ca24843b74a";
        private const string ITEM_CODENAME = "test_item_3182";
        private const string ITEM_EXTERNAL_ID = "external_3128";

        private const string VARIANT_ID = "b2eba115-a0f5-c8f9-974b-a53006c5adde";

        private EndpointUrlBuilder _builder;

        public EndpointUrlBuilderTests()
        {
            _builder = new EndpointUrlBuilder(OPTIONS);
        }

        #region Items Url

        [Fact]
        public void BuildItemsUrl()
        {
            var actualUrl = _builder.BuildItemsUrl();
            var expectedUrl = "https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/items";

            Assert.Equal(expectedUrl, actualUrl);
        }

        #endregion

        #region Item Url

        [Fact]
        public void BuildItemUrl_ItemId()
        {
            var identifier = new ContentItemIdentifier(ITEM_ID, null, null);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = "https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/items/cc601fe7-c057-5cb5-98d6-9ca24843b74a";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemCodename()
        {
            var identifier = new ContentItemIdentifier(null, ITEM_CODENAME, null);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = "https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/items/codename/test_item_3182";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemExternalId()
        {
            var identifier = new ContentItemIdentifier(null, null, ITEM_EXTERNAL_ID);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = "https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/items/external-id/external_3128";

            Assert.Equal(expectedUrl, actualUrl);
        }

        #endregion

        #region Assets

        [Fact]
        public void BuildAssetListingUrl_WithoutContinuationToken_ReturnsExpectedUrl()
        {
            var expectedResult = "https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/assets";
            var actualResult = _builder.BuildAssetListingUrl();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildAssetListingUrl_WithContinuationToken_ReturnsExpectedUrl()
        {
            var continuationToken = "MjA=";
            var expectedResult = "https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/assets?continuationToken=MjA%3d";
            var actualResult = _builder.BuildAssetListingUrl(continuationToken);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildAssetsUrlFromId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var assetId = "fcbb12e6-66a3-4672-85d9-d502d16b8d9c";
            var expectedResult = $"https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/assets/{assetId}";
            var actualResult = _builder.BuildAssetsUrlFromId(assetId);

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void BuildAssetsUrlFromExternalId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var externalId = "which-brewing-fits-you";
            var expectedResult = $"https://manage.kenticocloud.com/projects/bb6882a0-3088-405c-a6ac-4a0da46810b0/assets/external-id/{externalId}";
            var actualResult = _builder.BuildAssetsUrlFromExternalId(externalId);

            Assert.Equal(expectedResult, actualResult);
        }


        #endregion
    }
}
