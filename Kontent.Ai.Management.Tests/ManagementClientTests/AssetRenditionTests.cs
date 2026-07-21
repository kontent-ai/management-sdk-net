using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Collections;
using System.Text.Json;

using static Kontent.Ai.Management.Tests.Base.PagedFixtures;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetRenditionTests
{
    private static string AssetRendition => Fixture("AssetRendition.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "AssetRendition", name));

    [Fact]
    public async Task ListAssetRenditionsAsync_PagesThroughAllRenditions()
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

        var listResult = await client.ListAssetRenditionsAsync(identifier);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<AssetRenditionModel> renditions = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        renditions.Should().BeEquivalentTo(ConcatPages<AssetRenditionModel>(page1, page2, page3));
    }

    [Fact]
    public async Task ListAssetRenditionsAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListAssetRenditionsAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
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

    [Fact]
    public async Task GetRenditionAsync_InvalidIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetAssetRenditionAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
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

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets/{identifier.Id}/renditions")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", AssetRendition);

        var result = await client.CreateAssetRenditionAsync(identifier, createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(createModel);
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

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets/codename/{identifier.Codename}/renditions")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", AssetRendition);

        var result = await client.CreateAssetRenditionAsync(identifier, createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(createModel);
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

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets/external-id/{identifier.ExternalId}/renditions")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", AssetRendition);

        var result = await client.CreateAssetRenditionAsync(identifier, createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(createModel);
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

        mock.Expect(HttpMethod.Put, expectedUrl)
            .CaptureBody(out var capturedBody)
            .Respond("application/json", AssetRendition);

        var result = await client.UpdateAssetRenditionAsync(identifier, updateRenditionModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetRenditionModel>(AssetRendition, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(updateRenditionModel);
    }

    [Fact]
    public async Task UpdateAssetRenditionAsync_InvalidIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateRenditionModel = new AssetRenditionUpdateModel
        {
            Transformation = new RectangleResizeTransformation { CustomWidth = 1, CustomHeight = 1, X = 0, Y = 0, Width = 1, Height = 1 },
        };

        await client.Invoking(x => x.UpdateAssetRenditionAsync(null!, updateRenditionModel)).Should().ThrowExactlyAsync<ArgumentNullException>();
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

        public static IEnumerable<(AssetRenditionIdentifier Identifier, string Url)> GetPermutation()
        {
            var assets = new[] { IdentifierPermutations.ById, IdentifierPermutations.ByCodename, IdentifierPermutations.ByExternalId };
            var renditions = new[] { IdentifierPermutations.ById, IdentifierPermutations.ByExternalId };

            foreach (var (asset, assetSegment, rendition, renditionSegment) in IdentifierPermutations.Pairs(assets, renditions))
            {
                var identifier = new AssetRenditionIdentifier(asset, rendition);
                var url = $"{MockClientFactory.BaseUrl}/assets/{assetSegment}/renditions/{renditionSegment}";
                yield return (identifier, url);
            }
        }
    }
}
