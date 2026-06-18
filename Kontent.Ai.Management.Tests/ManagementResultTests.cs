using AwesomeAssertions;
using System.Net;

namespace Kontent.Ai.Management.Tests;

public class ManagementResultTests
{
    private static Error SampleError() => new()
    {
        Message = "Content item validation failed.",
        ValidationErrors = [new ValidationError { Message = "title is too long", Path = "title" }],
    };

    [Fact]
    public void Success_ExposesValueAndNoError()
    {
        var result = ManagementResult<string>.Success("ok");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");
        result.Error.Should().BeNull();
        result.StatusCode.Should().BeNull();
    }

    [Fact]
    public void Success_PassesThroughStatusAndRequestUrl()
    {
        var result = ManagementResult<string>.Success("ok", HttpStatusCode.OK, "https://example.org/x");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.RequestUrl.Should().Be("https://example.org/x");
    }

    [Fact]
    public void Failure_ExposesErrorAndDefaultValue()
    {
        var error = SampleError();

        var result = ManagementResult<string>.Failure(error, HttpStatusCode.BadRequest);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().BeSameAs(error);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void NonGeneric_Success_ExposesNoError()
    {
        var result = ManagementResult.Success(HttpStatusCode.NoContent);

        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public void NonGeneric_Failure_ExposesError()
    {
        var error = SampleError();

        var result = ManagementResult.Failure(error, HttpStatusCode.BadRequest);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeSameAs(error);
    }

    [Fact]
    public void IsCovariant_ResultOfDerivedAssignableToResultOfBase()
    {
        // out T on IManagementResult<T> must allow widening — useful when the generated record is returned as IElementsModel.
        IManagementResult<string> success = ManagementResult<string>.Success("ok");
        IManagementResult<object> widened = success;

        widened.Value.Should().Be("ok");
    }

    [Fact]
    public void GenericResult_IsAlsoNonGenericResult()
    {
        IManagementResult result = ManagementResult<string>.Success("ok");

        result.IsSuccess.Should().BeTrue();
    }
}
