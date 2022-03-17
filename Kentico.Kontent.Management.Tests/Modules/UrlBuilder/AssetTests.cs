using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.Modules.UrlBuilder
{
    public partial class EndpointUrlBuilderTests
    {
        [Fact]
        public void BuildAssetsUrlFromId_ById_ReturnsExpectedUrl()
        {
            var assetId = Guid.NewGuid();
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/{assetId}";
            var actualResult = _builder.BuildAssetsUrl(Reference.ById(assetId));

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildAssetsUrlFromExternalId_ByCodename_ReturnsExpectedUrl()
        {
            _builder.Invoking(c => c.BuildAssetsUrl(Reference.ByCodename("c"))).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void BuildAssetsUrlFromExternalId_ByExternalId_ReturnsExpectedUrl()
        {
            var externalId = "which-brewing-fits-you";
            var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/external-id/{externalId}";
            var actualResult = _builder.BuildAssetsUrl(Reference.ByExternalId(externalId));

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void BuildUploadFileUrl_ReturnsExpectedUrl()
        {
            var fileName = "which-brewing-fits-you-1080px.jpg";
            var expectedResult = $"https://manage.kontent.ai/v2/projects/{PROJECT_ID}/files/{fileName}";
            var actualResult = _builder.BuildUploadFileUrl(fileName);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
