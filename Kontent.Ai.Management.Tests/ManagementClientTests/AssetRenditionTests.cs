using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetRenditionTests
{
    private readonly Scenario _scenario;

    public AssetRenditionTests()
    {
        _scenario = new Scenario(folder: "AssetRendition");
    }

    [Fact]
    public async Task ListAssetRenditionsAsync_ById_ReturnsRenditions()
    {
        var client = _scenario
            .WithResponses("AssetRenditionPage1.json", "AssetRenditionPage2.json", "AssetRenditionPage3.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.ListAssetRenditionsAsync(identifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/assets/{identifier.Id}/renditions")
            .Validate();
    }

    [Fact]
    public async Task ListAssetRenditionsAsync_ByCodename_ReturnsRenditions()
    {
        var client = _scenario
            .WithResponses("AssetRenditionPage1.json", "AssetRenditionPage2.json", "AssetRenditionPage3.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.ListAssetRenditionsAsync(identifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/assets/codename/{identifier.Codename}/renditions")
            .Validate();
    }

    [Fact]
    public async Task ListAssetRenditionsAsync_ByExternalId_ReturnsRenditions()
    {
        var client = _scenario
            .WithResponses("AssetRenditionPage1.json", "AssetRenditionPage2.json", "AssetRenditionPage3.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.ListAssetRenditionsAsync(identifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/assets/external-id/{identifier.ExternalId}/renditions")
            .Validate();
    }

    [Fact]
    public async Task ListRenditionsAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ListAssetRenditionsAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfValidIdentifiersAndUrl))]
    public async Task GetRenditionAsync_ReturnsRendition(AssetRenditionIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("AssetRendition.json")
            .CreateManagementClient();

        var response = await client.GetAssetRenditionAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(expectedUrl)
            .Validate();
    }

    [Fact]
    public async Task GetRenditionAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = new AssetRenditionIdentifier(Reference.ByCodename("assetcodename"), Reference.ByCodename("renditioncodename"));
        await client.Invoking(x => x.GetAssetRenditionAsync(identifier)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetRenditionAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetAssetRenditionAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetRenditionAsync_ById_CreatesRenditions()
    {
        var client = _scenario
            .WithResponses("AssetRendition.json")
            .CreateManagementClient();

        var createModel = new AssetRenditionCreateModel
        {
            ExternalId = "rendition-1",
            Transformation = new RectangleResizeTransformation
            {
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            }
        };

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.CreateAssetRenditionAsync(identifier, createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/assets/{identifier.Id}/renditions")
            .Validate();
    }

    [Fact]
    public async Task CreateAssetRenditionAsync_ByCodename_CreatesRenditions()
    {
        var client = _scenario
            .WithResponses("AssetRendition.json")
            .CreateManagementClient();

        var createModel = new AssetRenditionCreateModel
        {
            ExternalId = "rendition-1",
            Transformation = new RectangleResizeTransformation
            {
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            }
        };

        var identifier = Reference.ByCodename("codename");
        var response = await client.CreateAssetRenditionAsync(identifier, createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/assets/codename/{identifier.Codename}/renditions")
            .Validate();
    }

    [Fact]
    public async Task CreateAssetRenditionAsync_ByExternalId_CreatesRenditions()
    {
        var client = _scenario
            .WithResponses("AssetRendition.json")
            .CreateManagementClient();

        var createModel = new AssetRenditionCreateModel
        {
            ExternalId = "rendition-1",
            Transformation = new RectangleResizeTransformation
            {
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            }
        };

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.CreateAssetRenditionAsync(identifier, createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/assets/external-id/{identifier.ExternalId}/renditions")
            .Validate();
    }

    [Fact]
    public async Task CreateRenditionAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario
            .WithResponses("AssetRendition.json")
            .CreateManagementClient();

        var createRenditionModel = new AssetRenditionCreateModel();

        await client.Invoking(x => x.CreateAssetRenditionAsync(null, createRenditionModel)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateRenditionAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateAssetRenditionAsync(Reference.ByExternalId("asset-1"), null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfValidIdentifiersAndUrl))]
    public async Task UpdateAssetRenditionAsync_UpdatesRenditions(AssetRenditionIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("AssetRendition.json")
            .CreateManagementClient();

        var updateRenditionModel = new AssetRenditionUpdateModel()
        {
            Transformation = new RectangleResizeTransformation
            {
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            }
        };

        var response = await client.UpdateAssetRenditionAsync(identifier, updateRenditionModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(updateRenditionModel)
            .Response(response)
            .Url(expectedUrl)
            .Validate();
    }

    [Fact]
    public async Task UpdateAssetRenditionAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = new AssetRenditionIdentifier(Reference.ByCodename("assetcodename"), Reference.ByCodename("renditioncodename"));
        var updateRenditionModel = new AssetRenditionUpdateModel();

        await client.Invoking(x => x.UpdateAssetRenditionAsync(identifier, updateRenditionModel)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task UpdateAssetRenditionAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var updateRenditionModel = new AssetRenditionUpdateModel();

        await client.Invoking(x => x.UpdateAssetRenditionAsync(null, updateRenditionModel)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateRenditionAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = new AssetRenditionIdentifier(Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()));
        await client.Invoking(x => x.UpdateAssetRenditionAsync(null, null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    private class CombinationOfValidIdentifiersAndUrl : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier, Url };
            }
        }

        public IEnumerable<(AssetRenditionIdentifier Identifier, string Url)> GetPermutation()
        {
            var assetIdentifier = new[] { ById, ByCodename, ByExternalId };
            var renditionIdentifiers = new[] { ById, ByExternalId };

            foreach (var item in assetIdentifier)
            {
                foreach (var language in renditionIdentifiers)
                {
                    var identifier = new AssetRenditionIdentifier(item.Identifier, language.Identifier);
                    var url = $"{Endpoint}/projects/{PROJECT_ID}/assets/{item.UrlSegment}/renditions/{language.UrlSegment}";
                    yield return (identifier, url);
                }
            }
        }

        private static (Reference Identifier, string UrlSegment) ById => (Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")), "4b628214-e4fe-4fe0-b1ff-955df33e1515");
        private static (Reference Identifier, string UrlSegment) ByCodename => (Reference.ByCodename("codename"), "codename/codename");
        private static (Reference Identifier, string UrlSegment) ByExternalId => (Reference.ByExternalId("external-id"), "external-id/external-id");
    }
}
