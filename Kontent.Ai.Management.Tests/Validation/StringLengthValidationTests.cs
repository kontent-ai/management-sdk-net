using AwesomeAssertions;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
using Kontent.Ai.Management.Validation;
using Xunit;

namespace Kontent.Ai.Management.Tests.Validation;

public class StringLengthValidationTests
{
    [Fact]
    public void NullValue_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Title = null });

        result.ValidationErrors().Should().NotContain(e => e.Path == "title");
    }

    [Fact]
    public void EmptyString_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Title = "" });

        result.ValidationErrors().Should().NotContain(e => e.Path == "title");
    }

    [Fact]
    public void ValueAtLimit_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Title = new string('x', 50) });

        result.ValidationErrors().Should().NotContain(e => e.Path == "title");
    }

    [Fact]
    public void ValueOverLimit_ProducesError()
    {
        var result = ContentItemValidator.Validate(new Article { Title = new string('x', 51) });

        result.IsSuccess.Should().BeFalse();
        var error = result.ValidationErrors().Single(e => e.Path == "title");
        error.Message.Should().Contain("51").And.Contain("50");
    }
}
