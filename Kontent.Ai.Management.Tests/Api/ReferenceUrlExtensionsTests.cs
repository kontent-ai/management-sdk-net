using AwesomeAssertions;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Tests.Api;

public class ReferenceUrlExtensionsTests
{
    // ----- Reference.ToUrlSegment: per-kind rendering ---------------------------------------------------------------

    [Fact]
    public void ToUrlSegment_ById_RendersBareGuid()
    {
        var id = Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000001");

        Reference.ById(id).ToUrlSegment().Should().Be(id.ToString());
    }

    [Fact]
    public void ToUrlSegment_ByCodename_PrefixesWithCodenameSegment()
    {
        Reference.ByCodename("my_space").ToUrlSegment().Should().Be("codename/my_space");
    }

    [Fact]
    public void ToUrlSegment_ByExternalId_PrefixesWithExternalIdSegment()
    {
        Reference.ByExternalId("ext-123").ToUrlSegment().Should().Be("external-id/ext-123");
    }

    // ----- Reference.ToUrlSegment: codename / external-id are left RAW ----------------------------------------------
    // ToUrlSegment does not percent-encode: the value routes through a Refit `{**}` catch-all that encodes it exactly
    // once. Pre-escaping here would double-encode (a space would reach the wire as %2520). The single, wire-level
    // encoding is asserted by ReferenceWireEncodingTests.

    [Fact]
    public void ToUrlSegment_ByExternalId_LeavesSpaceRaw()
    {
        Reference.ByExternalId("with space").ToUrlSegment().Should().Be("external-id/with space");
    }

    [Fact]
    public void ToUrlSegment_ByExternalId_LeavesAtSignRaw()
    {
        Reference.ByExternalId("user@kontent.ai").ToUrlSegment().Should().Be("external-id/user@kontent.ai");
    }

    [Fact]
    public void ToUrlSegment_ByExternalId_LeavesReservedCharsRaw()
    {
        Reference.ByExternalId("a&b=c+d").ToUrlSegment().Should().Be("external-id/a&b=c+d");
    }

    [Fact]
    public void ToUrlSegment_ByCodename_LeavesValueRaw()
    {
        Reference.ByCodename("not a codename").ToUrlSegment().Should().Be("codename/not a codename");
    }

    [Fact]
    public void ToUrlSegment_RendersEveryKind()
    {
        Reference.ById(Guid.NewGuid()).Invoking(r => r.ToUrlSegment()).Should().NotThrow();
        Reference.ByCodename("x").Invoking(r => r.ToUrlSegment()).Should().NotThrow();
        Reference.ByExternalId("x").Invoking(r => r.ToUrlSegment()).Should().NotThrow();
    }

    [Fact]
    public void ToUrlSegment_NullReference_Throws()
    {
        var act = () => ((Reference)null!).ToUrlSegment();

        act.Should().Throw<ArgumentNullException>();
    }

    // ----- LanguageVariantIdentifier.ToUrlSegment -------------------------------------------------------------------
    // Shape: `{item}/variants/{language}`. Item allows all three kinds; language allows id or codename only.

    [Fact]
    public void LanguageVariantIdentifier_ToUrlSegment_BothById_ComposesItemSlashVariantsSlashLanguage()
    {
        var itemId = Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000010");
        var languageId = Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000011");

        new LanguageVariantIdentifier(Reference.ById(itemId), Reference.ById(languageId))
            .ToUrlSegment()
            .Should().Be($"{itemId}/variants/{languageId}");
    }

    [Fact]
    public void LanguageVariantIdentifier_ToUrlSegment_ItemByCodename_LanguageById()
    {
        var languageId = Guid.NewGuid();

        new LanguageVariantIdentifier(Reference.ByCodename("article"), Reference.ById(languageId))
            .ToUrlSegment()
            .Should().Be($"codename/article/variants/{languageId}");
    }

    [Fact]
    public void LanguageVariantIdentifier_ToUrlSegment_ItemByExternalId_LanguageByCodename()
    {
        new LanguageVariantIdentifier(Reference.ByExternalId("ext-article"), Reference.ByCodename("en_us"))
            .ToUrlSegment()
            .Should().Be("external-id/ext-article/variants/codename/en_us");
    }

    [Fact]
    public void LanguageVariantIdentifier_ToUrlSegment_NullIdentifier_Throws()
    {
        var act = () => ((LanguageVariantIdentifier)null!).ToUrlSegment();

        act.Should().Throw<ArgumentNullException>();
    }

    // ----- AssetRenditionIdentifier.ToUrlSegment --------------------------------------------------------------------
    // Shape: `{asset}/renditions/{rendition}`.

    [Fact]
    public void AssetRenditionIdentifier_ToUrlSegment_BothById_ComposesAssetSlashRenditionsSlashRendition()
    {
        var assetId = Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000020");
        var renditionId = Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000021");

        new AssetRenditionIdentifier(Reference.ById(assetId), Reference.ById(renditionId))
            .ToUrlSegment()
            .Should().Be($"{assetId}/renditions/{renditionId}");
    }

    [Fact]
    public void AssetRenditionIdentifier_ToUrlSegment_AssetByCodename_RenditionById()
    {
        var renditionId = Guid.NewGuid();

        new AssetRenditionIdentifier(Reference.ByCodename("hero_image"), Reference.ById(renditionId))
            .ToUrlSegment()
            .Should().Be($"codename/hero_image/renditions/{renditionId}");
    }

    [Fact]
    public void AssetRenditionIdentifier_ToUrlSegment_AssetByExternalId_RenditionByExternalId()
    {
        new AssetRenditionIdentifier(Reference.ByExternalId("ext-asset"), Reference.ByExternalId("ext-rendition"))
            .ToUrlSegment()
            .Should().Be("external-id/ext-asset/renditions/external-id/ext-rendition");
    }

    [Fact]
    public void AssetRenditionIdentifier_ToUrlSegment_NullIdentifier_Throws()
    {
        var act = () => ((AssetRenditionIdentifier)null!).ToUrlSegment();

        act.Should().Throw<ArgumentNullException>();
    }
}
