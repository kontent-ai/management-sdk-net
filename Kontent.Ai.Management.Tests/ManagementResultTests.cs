using System.Net;
using FluentAssertions;
using Xunit;

namespace Kontent.Ai.Management.Tests;

public class ManagementResultTests
{
    [Fact]
    public void Success_ExposesValueAndEmptyErrors()
    {
        var result = ManagementResult<string>.Success("ok");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");
        result.Errors.Should().BeEmpty();
        result.StatusCode.Should().BeNull();
    }

    [Fact]
    public void Success_StatusCodePassedThrough()
    {
        var result = ManagementResult<string>.Success("ok", HttpStatusCode.OK);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public void Failure_ExposesErrorsAndDefaultValue()
    {
        var errors = new[]
        {
            new ManagementError("title is too long", "title"),
            new ManagementError("assets must contain at least 1 item", "assets"),
        };

        var result = ManagementResult<string>.Failure(errors);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().Equal(errors);
        result.StatusCode.Should().BeNull();
    }

    [Fact]
    public void Failure_StatusCodePassedThrough()
    {
        var errors = new[] { new ManagementError("bad request") };

        var result = ManagementResult<string>.Failure(errors, HttpStatusCode.BadRequest);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Failure_NullErrors_Throws()
    {
        Action act = () => _ = ManagementResult<string>.Failure(null!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("errors");
    }

    [Fact]
    public void Failure_EmptyErrors_Throws()
    {
        Action act = () => _ = ManagementResult<string>.Failure(Array.Empty<ManagementError>());
        act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("errors");
    }

    [Fact]
    public void ManagementError_OptionalFields_DefaultToNull()
    {
        var error = new ManagementError("something failed");

        error.Message.Should().Be("something failed");
        error.ElementCodename.Should().BeNull();
        error.Code.Should().BeNull();
    }

    [Fact]
    public void ManagementError_RecordEqualityHoldsByValue()
    {
        var a = new ManagementError("msg", "title", "TOO_LONG");
        var b = new ManagementError("msg", "title", "TOO_LONG");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void IsCovariant_ResultOfDerivedAssignableToResultOfBase()
    {
        // out T on IManagementResult<T> must allow widening — useful when the generated record is returned as IContentItem.
        IManagementResult<string> success = ManagementResult<string>.Success("ok");
        IManagementResult<object> widened = success;

        widened.Value.Should().Be("ok");
    }
}
