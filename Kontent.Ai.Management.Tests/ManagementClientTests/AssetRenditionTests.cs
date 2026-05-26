using System.Collections;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetRenditionTests
{
    private static string AssetRendition => Fixture("AssetRendition.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "AssetRendition", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    public static TheoryData<AssetRenditionIdentifier?> InvalidGetIdentifiers =>
    [
        new AssetRenditionIdentifier(Reference.ByCodename("assetcodename"), Reference.ByCodename("renditioncodename")),
        null,
    ];

    public static TheoryData<AssetRenditionIdentifier?> InvalidUpdateIdentifiers =>
    [
        new AssetRenditionIdentifier(Reference.ByCodename("assetcodename"), Reference.ByCodename("renditioncodename")),
        null,
    ];

    [Fact]
    public async Task EnumerateAssetRenditionPagesAsync_PagesThroughAllRenditions()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("AssetRenditionPage1.json");
        var page2 = Fixture("AssetRenditionPage2.json");
        var page3 = Fixture("AssetRenditionPage3.json");
        var identifier = Reference.ById(Guid.NewGuid());
        var url = $"{MockClientFactory.BaseUrl}/assets/{identifier.Id}/renditions";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var renditions = new List<AssetRenditionModel>();
        await foreach (var page in client.EnumerateAssetRenditionPagesAsync(identifier))
        {
            page.IsSuccess.Should().BeTrue();
            renditions.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        renditions.Should().BeEquivalentTo(ConcatPages<AssetRenditionModel>(page1, page2, page3));
    }

    [Fact]
    public void EnumerateAssetRenditionPagesAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        client.Invoking(x => x.EnumerateAssetRenditionPagesAsync(null!)).Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfValidIdentifiersAndUrl))]
    public async Task GetRenditionAsync_ReturnsRendition(AssetRenditionIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, expectedUrl)
            .Respond("application/json", AssetRendition);

        var result = await client.GetAssetRenditionAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
    }

    [Theory]
    [MemberData(nameof(InvalidGetIdentifiers))]
    public async Task GetRenditionAsync_InvalidIdentifier_Throws(AssetRenditionIdentifier? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetAssetRenditionAsync(identifier!)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task CreateAssetRenditionAsync_ById_CreatesRenditions()
    {
        var (client, mock) = MockClientFactory.Create();
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets/{identifier.Id}/renditions")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", AssetRendition);

        var result = await client.CreateAssetRenditionAsync(identifier, createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<AssetRenditionCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionCreateModel>(JsonSerializer.Serialize(createModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateAssetRenditionAsync_ByCodename_CreatesRenditions()
    {
        var (client, mock) = MockClientFactory.Create();
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets/codename/{identifier.Codename}/renditions")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", AssetRendition);

        var result = await client.CreateAssetRenditionAsync(identifier, createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<AssetRenditionCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionCreateModel>(JsonSerializer.Serialize(createModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateAssetRenditionAsync_ByExternalId_CreatesRenditions()
    {
        var (client, mock) = MockClientFactory.Create();
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets/external-id/{identifier.ExternalId}/renditions")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", AssetRendition);

        var result = await client.CreateAssetRenditionAsync(identifier, createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<AssetRenditionCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionCreateModel>(JsonSerializer.Serialize(createModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateRenditionAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var createRenditionModel = new AssetRenditionCreateModel
        {
            Transformation = new RectangleResizeTransformation { CustomWidth = 1, CustomHeight = 1, X = 0, Y = 0, Width = 1, Height = 1 },
        };

        await client.Invoking(x => x.CreateAssetRenditionAsync(null!, createRenditionModel)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateRenditionAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateAssetRenditionAsync(Reference.ByExternalId("asset-1"), null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfValidIdentifiersAndUrl))]
    public async Task UpdateAssetRenditionAsync_UpdatesRenditions(AssetRenditionIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, expectedUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", AssetRendition);

        var result = await client.UpdateAssetRenditionAsync(identifier, updateRenditionModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<AssetRenditionUpdateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionUpdateModel>(JsonSerializer.Serialize(updateRenditionModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Theory]
    [MemberData(nameof(InvalidUpdateIdentifiers))]
    public async Task UpdateAssetRenditionAsync_InvalidIdentifier_Throws(AssetRenditionIdentifier? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        var updateRenditionModel = new AssetRenditionUpdateModel
        {
            Transformation = new RectangleResizeTransformation { CustomWidth = 1, CustomHeight = 1, X = 0, Y = 0, Width = 1, Height = 1 },
        };

        await client.Invoking(x => x.UpdateAssetRenditionAsync(identifier!, updateRenditionModel)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task UpdateRenditionAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpdateAssetRenditionAsync(null!, null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
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
                    var url = $"{MockClientFactory.BaseUrl}/assets/{item.UrlSegment}/renditions/{language.UrlSegment}";
                    yield return (identifier, url);
                }
            }
        }

        private static (Reference Identifier, string UrlSegment) ById => (Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")), "4b628214-e4fe-4fe0-b1ff-955df33e1515");
        private static (Reference Identifier, string UrlSegment) ByCodename => (Reference.ByCodename("codename"), "codename/codename");
        private static (Reference Identifier, string UrlSegment) ByExternalId => (Reference.ByExternalId("external-id"), "external-id/external-id");
    }
}
