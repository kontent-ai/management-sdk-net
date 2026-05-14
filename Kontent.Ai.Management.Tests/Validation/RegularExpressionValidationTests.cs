using FluentAssertions;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
using Kontent.Ai.Management.Validation;
using Xunit;

namespace Kontent.Ai.Management.Tests.Validation;

public class RegularExpressionValidationTests
{
    [Fact]
    public void NullValue_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Slug = null });

        result.Errors.Should().NotContain(e => e.ElementCodename == "slug");
    }

    [Theory]
    [InlineData("hello")]
    [InlineData("hello-world")]
    [InlineData("article-1")]
    public void MatchingPattern_IsAccepted(string slug)
    {
        var result = ContentItemValidator.Validate(new Article { Slug = slug });

        result.Errors.Should().NotContain(e => e.ElementCodename == "slug");
    }

    [Theory]
    [InlineData("Hello")]            // uppercase
    [InlineData("hello world")]      // space
    [InlineData("hello_world")]      // underscore not in [a-z0-9-]
    public void NonMatchingPattern_ProducesError(string slug)
    {
        var result = ContentItemValidator.Validate(new Article { Slug = slug });

        result.IsSuccess.Should().BeFalse();
        result.Errors.Single(e => e.ElementCodename == "slug")
            .Message.Should().Contain("^[a-z0-9-]+$");
    }
}
