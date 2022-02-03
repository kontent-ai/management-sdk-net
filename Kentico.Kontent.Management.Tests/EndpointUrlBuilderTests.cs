﻿using System;
using FluentAssertions;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.UrlBuilder;
using Xunit;

namespace Kentico.Kontent.Management.Tests
{
    public class EndpointUrlBuilderTests
    {
        private const string PROJECT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
        private const string ENDPOINT = "https://manage.kontent.ai/v2";

        private static readonly ManagementOptions OPTIONS = new ManagementOptions() { ProjectId = PROJECT_ID };

        private static Guid ITEM_ID = Guid.Parse("b444004b-a4c4-43e3-94e0-d5bbd49d6cb8");
        private const string ITEM_CODENAME = "{ITEM_CODENAME}";
        private const string ITEM_EXTERNAL_ID = "{ITEM_EXT/ERNAL_ID}";
        private const string EXPECTED_ITEM_EXTERNAL_ID = "%7BITEM_EXT%2FERNAL_ID%7D";

        private static Guid VARIANT_ID = Guid.Parse("5a64af00-a98d-4d2e-adb6-45120cbc0242");
        private const string VARIANT_CODENAME = "{VARIANT_CODENAME}";

        private readonly EndpointUrlBuilder _builder;

        public EndpointUrlBuilderTests()
        {
            _builder = new EndpointUrlBuilder(OPTIONS);
        }

        #region Variants

        [Fact]
        public void BuildListVariantsUrl_ItemId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ById(ITEM_ID);
            var actualUrl = _builder.BuildListVariantsByItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByCodename(ITEM_CODENAME);
            var actualUrl = _builder.BuildListVariantsByItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByExternalId(ITEM_EXTERNAL_ID);
            var actualUrl = _builder.BuildListVariantsByItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{EXPECTED_ITEM_EXTERNAL_ID}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildLanguageVariantsUrl_ItemIdVariantId_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ById(ITEM_ID);
            var variantIdentifier = Reference.ById(VARIANT_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildLanguageVariantsUrl_ItemIdVariantCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ById(ITEM_ID);
            var variantIdentifier = Reference.ByCodename(VARIANT_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildLanguageVariantsUrl_ItemCodenameVariantId_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByCodename(ITEM_CODENAME);
            var variantIdentifier = Reference.ById(VARIANT_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildLanguageVariantsUrl_ItemCodenameVariantCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByCodename(ITEM_CODENAME);
            var variantIdentifier = Reference.ByCodename(VARIANT_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}/variants/codename/{VARIANT_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildLanguageVariantsUrl_ItemExternalIdVariantId_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByExternalId(ITEM_EXTERNAL_ID);
            var variantIdentifier = Reference.ById(VARIANT_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{EXPECTED_ITEM_EXTERNAL_ID}/variants/{VARIANT_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildLanguageVariantsUrl_ItemExternalIdVariantCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByExternalId(ITEM_EXTERNAL_ID);
            var variantIdentifier = Reference.ByCodename(VARIANT_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, variantIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{EXPECTED_ITEM_EXTERNAL_ID}/variants/codename/{VARIANT_CODENAME}";

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
            var identifier = Reference.ById(ITEM_ID);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{ITEM_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByCodename(ITEM_CODENAME);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{ITEM_CODENAME}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildItemUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByExternalId(ITEM_EXTERNAL_ID);
            var actualUrl = _builder.BuildItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{EXPECTED_ITEM_EXTERNAL_ID}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        #endregion

        #region Assets

        [Fact]
        public void BuildAssetsUrlFromId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var assetId = Guid.NewGuid();
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/{assetId}";
            var actualResult = _builder.BuildAssetsUrl(Reference.ById(assetId));

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void BuildAssetsUrlFromExternalId_WithGivenAssetId_ReturnsExpectedUrl()
        {
            var externalId = "which-brewing-fits-you";
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/external-id/{externalId}";
            var actualResult = _builder.BuildAssetsUrl(Reference.ByExternalId(externalId));

            Assert.Equal(expectedResult, actualResult);
        }

        #endregion

        #region File

        [Fact]
        public void BuildUploadFileUrl_ReturnsExpectedUrl()
        {
            var fileName = "which-brewing-fits-you-1080px.jpg";
            var expectedResult = $"https://manage.kontent.ai/v2/projects/{PROJECT_ID}/files/{fileName}";
            var actualResult = _builder.BuildUploadFileUrl(fileName);

            Assert.Equal(expectedResult, actualResult);
        }

        #endregion

        #region Validation

        [Fact]
        public void BuildItemUrl_ReturnsValidationUrl()
        {
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/validate";
            var actualUrl = _builder.BuildValidationUrl();

            Assert.Equal(expectedUrl, actualUrl);
        }

        #endregion

        [Fact]
        public void BuildUsersUrl_ReturnsExpectedUrl()
        {
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/users";
            var actualResult = _builder.BuildUsersUrl();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildModifyUsersRoleUrl_WithEmail_ReturnsExpectedUrl()
        {
            var email = "test@test.test";
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/users/email/{email}/roles";
            var actualResult = _builder.BuildModifyUsersRoleUrl(UserIdentifier.ByEmail(email));

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildModifyUsersRoleUrl_WithId_ReturnsExpectedUrl()
        {
            var id = "id";
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/users/{id}/roles";
            var actualResult = _builder.BuildModifyUsersRoleUrl(UserIdentifier.ById(id));

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildModifyUsersRoleUrl_MissingId_MissingEmail_ThrowsException()
        {
            _builder.Invoking(x => x.BuildModifyUsersRoleUrl(UserIdentifier.ByEmail(null))).Should().ThrowExactly<ArgumentException>();
        }
        
        [Fact]
        public void BuildProjectRolesUrl_ReturnsCorrectUrl()
        {
            var actualUrl = _builder.BuildProjectRolesUrl();
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/roles";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildProjectRoleUrl_WithId_ReturnsCorrectUrl()
        {
            var roleIdentifier = Reference.ById(Guid.NewGuid());
            var actualUrl = _builder.BuildProjectRoleUrl(roleIdentifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/roles/{roleIdentifier.Id}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildProjectRoleUrl_WithCodename_ReturnsCorrectUrl()
        {
            var roleIdentifier = Reference.ByCodename("codename");
            var actualUrl = _builder.BuildProjectRoleUrl(roleIdentifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/roles/codename/{roleIdentifier.Codename}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildProjectRoleUrl_WithExternalId_Throws()
        {
            var roleIdentifier = Reference.ByExternalId("external");
            Assert.Throws<InvalidOperationException>(() => _builder.BuildProjectRoleUrl(roleIdentifier));
        }
    }
}
