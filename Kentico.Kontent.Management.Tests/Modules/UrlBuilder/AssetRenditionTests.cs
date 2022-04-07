using FluentAssertions;
using Kentico.Kontent.Management.Models.AssetRenditions;
using Kentico.Kontent.Management.Models.Shared;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildAssetRenditionsUrlFromAssetId_WithGivenAssetId_ReturnsExpectedUrl()
    {
        var assetId = Guid.NewGuid();
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/{assetId}/renditions";
        var actualResult = _builder.BuildAssetRenditionsUrl(Reference.ById(assetId));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetExternalId_WithGivenAssetId_ReturnsExpectedUrl()
    {
        var assetExternalId = "which-brewing-fits-you";
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/external-id/{assetExternalId}/renditions";
        var actualResult = _builder.BuildAssetRenditionsUrl(Reference.ByExternalId(assetExternalId));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetIdAndRenditionId_WithGivenAssetAndRenditionIds_ReturnsExpectedUrl()
    {
        var assetId = Guid.NewGuid();
        var renditionId = Guid.NewGuid();
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/{assetId}/renditions/{renditionId}";
        var actualResult = _builder.BuildAssetRenditionsUrl(new AssetRenditionIdentifier(Reference.ById(assetId), Reference.ById(renditionId)));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetIdAndRenditionExternalId_WithGivenAssetAndRenditionIds_ReturnsExpectedUrl()
    {
        var assetId = Guid.NewGuid();
        var renditionExternalId = "customized-brewer";
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/{assetId}/renditions/external-id/{renditionExternalId}";
        var actualResult = _builder.BuildAssetRenditionsUrl(new AssetRenditionIdentifier(Reference.ById(assetId), Reference.ByExternalId(renditionExternalId)));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetExternalIdAndRenditionId_WithGivenAssetAndRenditionIds_ReturnsExpectedUrl()
    {
        var assetExternalId = "which-brewing-fits-you";
        var renditionId = Guid.NewGuid();
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/external-id/{assetExternalId}/renditions/{renditionId}";
        var actualResult = _builder.BuildAssetRenditionsUrl(new AssetRenditionIdentifier(Reference.ByExternalId(assetExternalId), Reference.ById(renditionId)));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetExternalIdAndRenditionExternalId_WithGivenAssetAndRenditionIds_ReturnsExpectedUrl()
    {
        var assetExternalId = "which-brewing-fits-you";
        var renditionExternalId = "customized-brewer";
        var expectedResult = $"{ENDPOINT}/projects/{PROJECT_ID}/assets/external-id/{assetExternalId}/renditions/external-id/{renditionExternalId}";
        var actualResult = _builder.BuildAssetRenditionsUrl(new AssetRenditionIdentifier(Reference.ByExternalId(assetExternalId), Reference.ByExternalId(renditionExternalId)));

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetCodenameAndRenditionId_AssetDoesNotSupportCodename_Throws()
    {
        _builder.Invoking(x => x.BuildAssetRenditionsUrl(new AssetRenditionIdentifier(Reference.ByCodename("not-supported"), Reference.ById(Guid.NewGuid()))))
            .Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void BuildAssetRenditionsUrlFromAssetIdAndRenditionCodename_RenditionDoesNotSupportCodename_Throws()
    {
        _builder.Invoking(x => x.BuildAssetRenditionsUrl(new AssetRenditionIdentifier(Reference.ById(Guid.NewGuid()), Reference.ByCodename("not-supported"))))
            .Should().ThrowExactly<InvalidOperationException>();
    }
}
