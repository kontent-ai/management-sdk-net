using FluentAssertions;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildItemsWithVariantFilterUrl_ReturnsCorrectUrl()
    {
        var result = _builder.BuildItemsWithVariantFilterUrl();

        result.Should().Be($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/items-with-variant/filter");
    }

    [Fact]
    public void BuildItemsWithVariantBulkGetUrl_ReturnsCorrectUrl()
    {
        var result = _builder.BuildItemsWithVariantBulkGetUrl();

        result.Should().Be($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/items-with-variant/bulk-get");
    }
}
