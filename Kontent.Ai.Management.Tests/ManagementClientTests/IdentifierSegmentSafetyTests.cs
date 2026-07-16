using AwesomeAssertions;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.AssetRenditions;
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
        badAsset.Invoking(i => i.ToUrlSegment()).Should().Throw<ArgumentException>();
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
