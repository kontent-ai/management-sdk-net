using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder
{
    public partial class EndpointUrlBuilderTests
    {
        [Fact]
        public void BuildTypeUrl_ReturnsExpectedUrl()
        {
            var actualUrl = _builder.BuildTypeUrl();
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildTypeUrl_ById_ReturnsExpectedUrl()
        {
            var identifier = Reference.ById(Guid.NewGuid());

            var actualUrl = _builder.BuildTypeUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/{identifier.Id}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildTypeUrl_ByCodename_ReturnsExpectedUrl()
        {
            var identifier = Reference.ByCodename("codename");

            var actualUrl = _builder.BuildTypeUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/codename/{identifier.Codename}";

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void BuildTypeUrl_ByExternalId_ReturnsExpectedUrl()
        {
            var identifier = Reference.ByExternalId("external");

            var actualUrl = _builder.BuildTypeUrl(identifier);
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/types/external-id/{identifier.ExternalId}";

            Assert.Equal(expectedUrl, actualUrl);
        }
    }
}
