using AwesomeAssertions;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

/// <summary>
/// A caller-supplied identifier is interpolated into a Refit <c>{**}</c> catch-all route, which passes <c>/</c> and the
/// dot-segments <c>.</c> / <c>..</c> through as path structure. Left unchecked, a value such as <c>../../webhooks-vnext</c>
/// retargets the request to a different Management API endpoint under the same token. These tests pin that the offending
/// characters are rejected at the URL boundary (<see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/> and the user
/// / composite overloads) and that harmless-but-encodable characters still round-trip.
/// </summary>
public class IdentifierSegmentSafetyTests
{
    public static IEnumerable<object[]> TraversalPayloads =>
    [
        ["../../webhooks-vnext"],
        [".."],
        ["."],
        ["a/b"],
        ["a\\b"],
        ["..\\..\\x"],
    ];

    [Theory]
    [MemberData(nameof(TraversalPayloads))]
    public void ToUrlSegment_Reference_Codename_RejectsTraversal(string payload)
    {
        var act = () => Reference.ByCodename(payload).ToUrlSegment();
        act.Should().Throw<ArgumentException>().WithMessage($"*{payload}*");
    }

    [Theory]
    [MemberData(nameof(TraversalPayloads))]
    public void ToUrlSegment_Reference_ExternalId_RejectsTraversal(string payload)
    {
        var act = () => Reference.ByExternalId(payload).ToUrlSegment();
        act.Should().Throw<ArgumentException>().WithMessage($"*{payload}*");
    }

    [Theory]
    [MemberData(nameof(TraversalPayloads))]
    public void ToUrlSegment_UserIdentifier_Email_RejectsTraversal(string payload)
    {
        var act = () => UserIdentifier.ByEmail(payload).ToUrlSegment();
        act.Should().Throw<ArgumentException>().WithMessage($"*{payload}*");
    }

    [Theory]
    [MemberData(nameof(TraversalPayloads))]
    public void ToUrlSegment_UserIdentifier_Id_RejectsTraversal(string payload)
    {
        var act = () => UserIdentifier.ById(payload).ToUrlSegment();
        act.Should().Throw<ArgumentException>().WithMessage($"*{payload}*");
    }

    [Fact]
    public void ToUrlSegment_Composite_Variant_RejectsTraversalInEitherPart()
    {
        var badItem = new LanguageVariantIdentifier(Reference.ByCodename("../../x"), Reference.ByCodename("en"));
        var badLanguage = new LanguageVariantIdentifier(Reference.ByCodename("item"), Reference.ByCodename(".."));

        badItem.Invoking(i => i.ToUrlSegment()).Should().Throw<ArgumentException>();
        badLanguage.Invoking(i => i.ToUrlSegment()).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToUrlSegment_Composite_Rendition_RejectsTraversalInEitherPart()
    {
        var badAsset = new AssetRenditionIdentifier(Reference.ByCodename("a/b"), Reference.ByCodename("r"));
        var badRendition = new AssetRenditionIdentifier(Reference.ByCodename("a"), Reference.ByCodename(".."));

        badAsset.Invoking(i => i.ToUrlSegment()).Should().Throw<ArgumentException>();
        badRendition.Invoking(i => i.ToUrlSegment()).Should().Throw<ArgumentException>();
    }

    // An empty segment retargets a single-resource route onto its parent collection route (…/users/{id} with an
    // empty id becomes the list-users endpoint), so empty identifiers are rejected eagerly at the factories.
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void IdentifierFactories_RejectEmptyAndWhitespace(string payload)
    {
        FluentActions.Invoking(() => Reference.ByCodename(payload)).Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => Reference.ByExternalId(payload)).Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => UserIdentifier.ById(payload)).Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => UserIdentifier.ByEmail(payload)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IdentifierFactories_RejectNull()
    {
        FluentActions.Invoking(() => Reference.ByCodename(null!)).Should().Throw<ArgumentNullException>();
        FluentActions.Invoking(() => Reference.ByExternalId(null!)).Should().Throw<ArgumentNullException>();
        FluentActions.Invoking(() => UserIdentifier.ById(null!)).Should().Throw<ArgumentNullException>();
        FluentActions.Invoking(() => UserIdentifier.ByEmail(null!)).Should().Throw<ArgumentNullException>();
    }

    // Defense in depth: even a value that bypassed the factories (e.g. via deserialization) is rejected at the URL boundary.
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("..")]
    [InlineData("a/b")]
    public void EnsureSingleSegment_RejectsStructuralValues(string payload)
        => FluentActions.Invoking(() => ReferenceUrlExtensions.EnsureSingleSegment(payload)).Should().Throw<ArgumentException>();

    // The upload file name travels as a plain Refit route parameter, where dot-segments survive escaping and URI
    // normalization retargets the request (".." reaches the environment root). The guard sits on FileContentSource.
    public static IEnumerable<object[]> TraversalFileNames =>
    [
        [".."],
        ["."],
        ["a/b.png"],
        ["a\\b.png"],
        ["../asset.png"],
        [""],
        ["   "],
    ];

    [Theory]
    [MemberData(nameof(TraversalFileNames))]
    public void FileContentSource_ByteArray_RejectsTraversalFileName(string fileName)
        => FluentActions.Invoking(() => new FileContentSource([1], fileName, "image/png"))
            .Should().Throw<ArgumentException>();

    [Theory]
    [MemberData(nameof(TraversalFileNames))]
    public void FileContentSource_Stream_RejectsTraversalFileName(string fileName)
        => FluentActions.Invoking(() => new FileContentSource(new MemoryStream([1]), fileName, "image/png"))
            .Should().Throw<ArgumentException>();

    [Theory]
    [InlineData("dir/..")]
    [InlineData("dir/")]
    public void FileContentSource_FilePath_RejectsPathsWithNoSafeFileName(string filePath)
        => FluentActions.Invoking(() => new FileContentSource(filePath, "image/png"))
            .Should().Throw<ArgumentException>();

    [Fact]
    public void FileContentSource_FilePath_DerivesBareFileName()
    {
        var source = new FileContentSource(Path.Combine("some", "dir", "photo.png"), "image/png");

        source.FileName.Should().Be("photo.png");
    }

    [Theory]
    [InlineData("normal_codename")]
    [InlineData("50% off")]
    [InlineData("has space")]
    [InlineData("q?x#y")]
    public void ToUrlSegment_Codename_AllowsEncodableCharacters(string codename)
    {
        var act = () => Reference.ByCodename(codename).ToUrlSegment();
        act.Should().NotThrow();
    }

    [Fact]
    public async Task ClientCall_WithTraversalCodename_ThrowsAndSendsNoRequest()
    {
        var (client, mock) = MockClientFactory.Create();
        var probe = mock.When("*").Respond(System.Net.HttpStatusCode.OK, "application/json", "{}");

        var act = () => client.GetContentItemAsync(Reference.ByCodename("../../preview-configuration"));

        await act.Should().ThrowAsync<ArgumentException>();
        mock.GetMatchCount(probe).Should().Be(0);
    }
}
