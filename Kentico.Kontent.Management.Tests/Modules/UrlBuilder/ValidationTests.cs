using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.Modules.UrlBuilder
{
    public partial class EndpointUrlBuilderTests
    {
        [Fact]
        public void BuildValidationUrl_ReturnsValidationUrl()
        {
            var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/validate";
            var actualUrl = _builder.BuildValidationUrl();

            Assert.Equal(expectedUrl, actualUrl);
        }
    }
}
