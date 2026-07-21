using AwesomeAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using System.Net;

namespace Kontent.Ai.Management.Tests;

public class ManagementResultExtensionsTests
{
    private static Error SampleError() => new() { Message = "Boom." };

    [Fact]
    public void EnsureSuccess_Generic_ReturnsValue()
    {
        ManagementResult<string>.Success("ok", HttpStatusCode.OK).EnsureSuccess().Should().Be("ok");
    }

    [Fact]
    public void EnsureSuccess_Generic_Throws_WithErrorDetail()
    {
        var error = SampleError();
        var result = ManagementResult<string>.Failure(error, HttpStatusCode.BadRequest, "https://example.org/x");

        var act = () => result.EnsureSuccess();

        act.Should().Throw<ManagementException>()
            .Which.Should().Match<ManagementException>(e =>
                ReferenceEquals(e.Error, error) && e.StatusCode == HttpStatusCode.BadRequest && e.RequestUrl == "https://example.org/x");
    }

    [Fact]
    public void EnsureSuccess_NonGeneric_Throws_OnFailure()
    {
        var act = () => ManagementResult.Failure(SampleError(), HttpStatusCode.BadRequest).EnsureSuccess();

        act.Should().Throw<ManagementException>();
    }

    [Fact]
    public void TryGetValue_ReturnsTrueAndValue_OnSuccess()
    {
        ManagementResult<string>.Success("ok", HttpStatusCode.OK).TryGetValue(out var value).Should().BeTrue();
        value.Should().Be("ok");
    }

    [Fact]
    public void TryGetValue_ReturnsFalse_OnFailure()
    {
        ManagementResult<string>.Failure(SampleError(), HttpStatusCode.BadRequest).TryGetValue(out var value).Should().BeFalse();
        value.Should().BeNull();
    }

    [Fact]
    public void AsFailure_ReprojectsErrorStatusAndUrl()
    {
        var error = SampleError();
        var failure = ManagementResult<string>.Failure(error, HttpStatusCode.Conflict, "https://example.org/x");

        var reprojected = failure.AsFailure<int>();

        reprojected.IsSuccess.Should().BeFalse();
        reprojected.Error.Should().BeSameAs(error);
        reprojected.StatusCode.Should().Be(HttpStatusCode.Conflict);
        reprojected.RequestUrl.Should().Be("https://example.org/x");
    }

    [Fact]
    public void AsFailure_OnSuccess_Throws()
    {
        var act = () => ManagementResult<string>.Success("ok", HttpStatusCode.OK).AsFailure<int>();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void LanguageVariantIdentifier_ByCodenames_BuildsReferences()
    {
        var id = LanguageVariantIdentifier.ByCodenames("on_roasts", "en-US");

        id.ItemIdentifier.Should().Be(Reference.ByCodename("on_roasts"));
        id.LanguageIdentifier.Should().Be(Reference.ByCodename("en-US"));
    }

    [Fact]
    public void LanguageVariantIdentifier_ByIds_BuildsReferences()
    {
        var item = Guid.NewGuid();
        var language = Guid.NewGuid();

        var id = LanguageVariantIdentifier.ByIds(item, language);

        id.ItemIdentifier.Should().Be(Reference.ById(item));
        id.LanguageIdentifier.Should().Be(Reference.ById(language));
    }

    [Fact]
    public void LanguageVariantIdentifier_ByExternalIds_BuildsReferences()
    {
        var id = LanguageVariantIdentifier.ByExternalIds("item-ext", "lang-ext");

        id.ItemIdentifier.Should().Be(Reference.ByExternalId("item-ext"));
        id.LanguageIdentifier.Should().Be(Reference.ByExternalId("lang-ext"));
    }
}
