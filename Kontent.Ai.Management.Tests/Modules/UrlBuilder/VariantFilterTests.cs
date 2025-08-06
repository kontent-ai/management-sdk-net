using FluentAssertions;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildVariantFilterUrl_ReturnsCorrectUrl()
    {
        var result = _builder.BuildVariantFilterUrl();

        result.Should().Be($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/early-access/variants/filter");
    }
}