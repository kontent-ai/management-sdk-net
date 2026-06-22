using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetTests
{
    private static string Asset => Fixture("Asset.json");
    private static string File_ => Fixture("File.json");

    private static string Fixture(string name)
        => System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Asset", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    private static AssetModel ExpectedAsset()
        => JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

    [Fact]
    public async Task ListAssetsAsync_PagesThroughAllAssets()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("AssetsPage1.json");
        var page2 = Fixture("AssetsPage2.json");
        var page3 = Fixture("AssetsPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/assets";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var listResult = await client.ListAssetsAsync();
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<AssetModel> assets = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        assets.ShouldEqualAsJson(ConcatPages<AssetModel>(page1, page2, page3));
    }

    [Fact]
    public async Task EnumerateAssetPagesAsync_StreamsAllPages()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("AssetsPage1.json");
        var page2 = Fixture("AssetsPage2.json");
        var page3 = Fixture("AssetsPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/assets";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var assets = new List<AssetModel>();
        await foreach (var page in client.EnumerateAssetPagesAsync())
        {
            page.IsSuccess.Should().BeTrue();
            assets.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        assets.ShouldEqualAsJson(ConcatPages<AssetModel>(page1, page2, page3));
    }

    [Fact]
    public async Task GetAssetAsync_ById_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var result = await client.GetAssetAsync(Reference.ById(expected.Id));

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_ByCodename_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var result = await client.GetAssetAsync(Reference.ByCodename(expected.Codename));

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_ByExternalId_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/external-id/my-external-id")
            .Respond("application/json", Asset);

        var result = await client.GetAssetAsync(Reference.ByExternalId("my-external-id"));

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(ExpectedAsset());
    }

    [Fact]
    public async Task GetAssetAsync_TaxonomyElements_BindAsAssetElements()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var result = await client.GetAssetAsync(Reference.ById(expected.Id));

        mock.VerifyNoOutstandingExpectation();
        var elements = result.Value.Elements.ToList();
        elements.Should().HaveCount(2);
        elements[0].Element.Id.Should().Be("7ef2ebb4-c480-42b7-ba35-a3078d6cce3f");
        elements[0].Value.Select(t => t.Id).Should().Equal(
            Guid.Parse("4a4f8cb0-e7fe-40ad-9943-66f395e58571"),
            Guid.Parse("96e493ab-45c4-4505-a3d0-b46192dd179e"));
        elements[1].Element.Id.Should().Be("70dfa72d-4599-40cb-aa27-7597470d5e2e");
        elements[1].Value.Select(t => t.Id).Should().Equal(Guid.Parse("16d27bf1-e0f4-8646-0e54-1b71efc6947f"));
    }

    [Fact]
    public async Task GetAssetAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.GetAssetAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_CreatesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();

        var createModel = new AssetCreateModel
        {
            FileReference = new FileReference { Id = expected.FileReference.Id, Type = FileReferenceTypeEnum.Internal },
            Title = expected.Title,
            Elements = expected.Elements,
        };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets")
            .Respond("application/json", Asset);

        var result = await client.CreateAssetAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task CreateAssetAsync_ModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.CreateAssetAsync((AssetCreateModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_WithFileContent_CreatesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/Hello.txt")
            .Respond("application/json", File_);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets")
            .Respond("application/json", Asset);

        var result = await client.CreateAssetAsync(content, fileReference => new AssetCreateModel
        {
            FileReference = fileReference,
            Title = expected.Title,
            Elements = expected.Elements,
        });

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task CreateAssetAsync_WithFileContent_FileContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.CreateAssetAsync(null!, fileReference => new AssetCreateModel
        {
            FileReference = fileReference,
            Title = "x",
        }))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_WithFileContent_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        await client.Invoking(c => c.CreateAssetAsync(content, null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_ById_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();

        var upsertModel = new AssetUpsertModel { Title = expected.Title, Elements = expected.Elements };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var result = await client.UpsertAssetAsync(Reference.ById(expected.Id), upsertModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_ByCodename_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();

        var upsertModel = new AssetUpsertModel { Title = expected.Title, Elements = expected.Elements };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var result = await client.UpsertAssetAsync(Reference.ByCodename(expected.Codename), upsertModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_ByExternalId_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();

        var upsertModel = new AssetUpsertModel { Title = "Chemex Paper Filters" };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/external-id/my-external-id")
            .Respond("application/json", Asset);

        var result = await client.UpsertAssetAsync(Reference.ByExternalId("my-external-id"), upsertModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(ExpectedAsset());
    }

    [Fact]
    public async Task UpsertAssetAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UpsertAssetAsync(null!, new AssetUpsertModel { Title = "x" }))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("ex"), (AssetUpsertModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_WithFileContent_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = ExpectedAsset();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");
        var upsertModel = new AssetUpsertModel { Title = expected.Title, Elements = expected.Elements };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/Hello.txt")
            .Respond("application/json", File_);
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var result = await client.UpsertAssetAsync(Reference.ById(expected.Id), content, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_WithFileContent_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        await client.Invoking(c => c.UpsertAssetAsync(null!, content, new AssetUpsertModel { Title = "x" }))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_WithFileContent_FileContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("ex"), null!, new AssetUpsertModel { Title = "x" }))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_WithFileContent_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("ex"), content, null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAssetAsync_ById_DeletesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Empty);
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/assets/{identifier.Id}")
            .Respond(HttpStatusCode.OK);

        var result = await client.DeleteAssetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAssetAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.DeleteAssetAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UploadFileAsync_UploadsFile()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<FileReference>(File_, SharedTestJsonOptions.Default);

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/Hello.txt")
            .Respond("application/json", File_);

        var result = await client.UploadFileAsync(content);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UploadFileAsync_ContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UploadFileAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
