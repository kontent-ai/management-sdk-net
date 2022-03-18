using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder
{
    public partial class EndpointUrlBuilderTests
    {
        [Fact]
        public void BuildListVariantsByItemUrl_ItemId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ById(Guid.NewGuid());
            var actualUrl = _builder.BuildListVariantsByItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{identifier.Id}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByItemUrl_ItemCodename_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByCodename("codename");
            var actualUrl = _builder.BuildListVariantsByItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{identifier.Codename}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByItemUrl_ItemExternalId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByExternalId("externalId");
            var actualUrl = _builder.BuildListVariantsByItemUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{identifier.ExternalId}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByTypeUrl_ContentTypeId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ById(Guid.NewGuid());
            var actualUrl = _builder.BuildListVariantsByTypeUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/{identifier.Id}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByTypeUrl_ContentTypeCodename_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByCodename("codename");
            var actualUrl = _builder.BuildListVariantsByTypeUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/codename/{identifier.Codename}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByTypeUrl_ContentTypeExternalId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByExternalId("externalId");
            var actualUrl = _builder.BuildListVariantsByTypeUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/external-id/{identifier.ExternalId}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByComponentUrl_ComponentId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ById(Guid.NewGuid());
            var actualUrl = _builder.BuildListVariantsByComponentUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/{identifier.Id}/components";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByComponentUrl_ComponentCodename_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByCodename("codename");
            var actualUrl = _builder.BuildListVariantsByComponentUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/codename/{identifier.Codename}/components";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByComponentUrl_ComponentExternalId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByExternalId("externalId");
            var actualUrl = _builder.BuildListVariantsByComponentUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/external-id/{identifier.ExternalId}/components";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByCollectionUrl_CollectionId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ById(Guid.NewGuid());
            var actualUrl = _builder.BuildListVariantsByCollectionUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/collections/{identifier.Id}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByCollectionUrl_CollectionCodename_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByCodename("codename");
            var actualUrl = _builder.BuildListVariantsByCollectionUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/collections/codename/{identifier.Codename}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildListVariantsByCollectionUrl_CollectionExternalId_ReturnsCorrectUrl()
        {
            var identifier = Reference.ByExternalId("externalId");
            var actualUrl = _builder.BuildListVariantsByCollectionUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/collections/external-id/{identifier.ExternalId}/variants";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildVariantsUrl_ItemIdLanguageId_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ById(Guid.NewGuid());
            var languageIdentifier = Reference.ById(Guid.NewGuid());
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{itemIdentifier.Id}/variants/{languageIdentifier.Id}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildVariantsUrl_ItemIdLanguageCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ById(Guid.NewGuid());
            var languageIdentifier = Reference.ByCodename("codename");
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/{itemIdentifier.Id}/variants/codename/{languageIdentifier.Codename}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildVariantsUrl_ItemCodenameLanguageId_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByCodename("codename");
            var languageIdentifier = Reference.ById(Guid.NewGuid());
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{itemIdentifier.Codename}/variants/{languageIdentifier.Id}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildVariantsUrl_ItemCodenameLanguageCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByCodename("codename");
            var languageIdentifier = Reference.ByCodename("codename");
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/codename/{itemIdentifier.Codename}/variants/codename/{languageIdentifier.Codename}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildVariantsUrl_ItemExternalIdLanguageId_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByExternalId("externalId");
            var languageIdentifier = Reference.ById(Guid.NewGuid());
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{itemIdentifier.ExternalId}/variants/{languageIdentifier.Id}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildVariantsUrl_ItemExternalIdLanguageCodename_ReturnsCorrectUrl()
        {
            var itemIdentifier = Reference.ByExternalId("externalId");
            var languageIdentifier = Reference.ByCodename("codename");
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var actualUrl = _builder.BuildVariantsUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/items/external-id/{itemIdentifier.ExternalId}/variants/codename/{languageIdentifier.Codename}";

            Assert.Equal(expectedUrl, actualUrl);
        }
    }
}
