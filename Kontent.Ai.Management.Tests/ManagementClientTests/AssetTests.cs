using System.Text;
using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Tests.Base;
using Kontent.Ai.Management.Tests.Data;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetTests
{
    private static string Asset => Fixture("Asset.json");
    private static string File_ => Fixture("File.json");

    private static readonly IElementModelProvider _elementModelProvider = new ElementModelProvider();

    private static string Fixture(string name)
        => System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Asset", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    private static AssetModel<T> DeserializeStronglyTyped<T>(string json) where T : new()
    {
        var dynamicAsset = JsonSerializer.Deserialize<AssetModel>(json, SharedTestJsonOptions.Default)!;
        return new AssetModel<T>
        {
            Id = dynamicAsset.Id,
            FileName = dynamicAsset.FileName,
            Size = dynamicAsset.Size,
            Type = dynamicAsset.Type,
            Url = dynamicAsset.Url,
            FileReference = dynamicAsset.FileReference,
            Descriptions = dynamicAsset.Descriptions,
            Title = dynamicAsset.Title,
            Codename = dynamicAsset.Codename,
            ExternalId = dynamicAsset.ExternalId,
            LastModified = dynamicAsset.LastModified,
            ImageHeight = dynamicAsset.ImageHeight,
            ImageWidth = dynamicAsset.ImageWidth,
            Folder = dynamicAsset.Folder,
            Collection = dynamicAsset.Collection,
            Elements = _elementModelProvider.GetStronglyTypedElements<T>(dynamicAsset.Elements),
        };
    }

    private static List<AssetModel<T>> ConcatStronglyTypedPages<T>(params string[] pages) where T : new()
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<AssetModel>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .Select(a => new AssetModel<T>
            {
                Id = a.Id,
                FileName = a.FileName,
                Size = a.Size,
                Type = a.Type,
                Url = a.Url,
                FileReference = a.FileReference,
                Descriptions = a.Descriptions,
                Title = a.Title,
                Codename = a.Codename,
                ExternalId = a.ExternalId,
                LastModified = a.LastModified,
                ImageHeight = a.ImageHeight,
                ImageWidth = a.ImageWidth,
                Folder = a.Folder,
                Collection = a.Collection,
                Elements = _elementModelProvider.GetStronglyTypedElements<T>(a.Elements),
            })
            .ToList();

    [Fact]
    public async Task ListAssetsAsync_DynamicallyTyped_WithMorePages_ListsAssets()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("AssetsPage1.json");
        var page2 = Fixture("AssetsPage2.json");
        var page3 = Fixture("AssetsPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/assets";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var expected = ConcatPages<AssetModel>(page1, page2, page3);

        var response = await client.ListAssetsAsync().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListAssetsAsync_StronglyTyped_WithMorePages_ListsAssets()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("AssetsPage1.json");
        var page2 = Fixture("AssetsPage2.json");
        var page3 = Fixture("AssetsPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/assets";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var expected = ConcatStronglyTypedPages<ComplexTestModel>(page1, page2, page3);

        var response = await client.ListAssetsAsync<ComplexTestModel>().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_ById_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ById(expected.Id));

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_ByCodename_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ByCodename(expected.Codename));

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_ByExternalId_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ByExternalId(expected.ExternalId));

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.GetAssetAsync<ComplexTestModel>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetAssetAsync_DynamicallyTyped_ById_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync(Reference.ById(expected.Id));

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_DynamicallyTyped_ByCodename_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync(Reference.ByCodename(expected.Codename));

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_DynamicallyTyped_ByExternalId_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync(Reference.ByExternalId(expected.ExternalId));

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetAssetAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.GetAssetAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_StronglyTyped_CreatesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);

        var createModel = new AssetCreateModel<ComplexTestModel>
        {
            Title = expected.Title,
            ExternalId = expected.ExternalId,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets")
            .Respond("application/json", Asset);

        var response = await client.CreateAssetAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task CreateAssetAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.CreateAssetAsync<ComplexTestModel>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_DynamicallyTyped_CreatesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

        var createModel = new AssetCreateModel
        {
            Title = expected.Title,
            ExternalId = expected.ExternalId,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets")
            .Respond("application/json", Asset);

        var response = await client.CreateAssetAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task CreateAssetAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.CreateAssetAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_StronglyTyped_WithFileContent_CreatesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        var updateModel = new AssetCreateModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        var content = new FileContentSource(stream, fileName, contentType);

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/{fileName}")
            .Respond("application/json", File_);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets")
            .Respond("application/json", Asset);

        var response = await client.CreateAssetAsync(content, updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task CreateAssetAsync_StronglyTyped_WithFileContent_FileContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateModel = new AssetCreateModel<ComplexTestModel> { Title = "xxx" };

        await client.Invoking(c => c.CreateAssetAsync(null!, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_StronglyTyped_WithFileContent_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        await client.Invoking(c => c.CreateAssetAsync<ComplexTestModel>(content, null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_DynamicallyTyped_WithFileContent_CreatesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        var updateModel = new AssetCreateModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        var content = new FileContentSource(stream, fileName, contentType);

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/{fileName}")
            .Respond("application/json", File_);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/assets")
            .Respond("application/json", Asset);

        var response = await client.CreateAssetAsync(content, updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task CreateAssetAsync_DynamicallyTyped_WithFileContent_FileContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateModel = new AssetCreateModel { Title = "xxx" };

        await client.Invoking(c => c.CreateAssetAsync(null!, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateAssetAsync_DynamicallyTyped_WithFileContent_UpsertModelIsNull_Throws()
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
    public async Task UpsertAssetAsync_StronglyTyped_ById_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_ByCodename_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByCodename(expected.Codename), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_ByExternalId_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByExternalId(expected.ExternalId), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = "xxx"
        };

        await client.Invoking(c => c.UpsertAssetAsync(null!, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UpsertAssetAsync<ComplexTestModel>(Reference.ByExternalId("ex"), null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_ById_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_ByCodename_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByCodename(expected.Codename), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_ByExternalId_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByExternalId(expected.ExternalId), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateModel = new AssetUpsertModel
        {
            Title = "xxx"
        };

        await client.Invoking(c => c.UpsertAssetAsync(null!, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("ex"), null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_WithFileContent_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = DeserializeStronglyTyped<ComplexTestModel>(Asset);

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        var content = new FileContentSource(stream, fileName, contentType);

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/{fileName}")
            .Respond("application/json", File_);
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), content, updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_WithFileContent_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        var updateModel = new AssetUpsertModel<ComplexTestModel> { Title = "xxx" };

        await client.Invoking(c => c.UpsertAssetAsync(null!, content, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_WithFileContent_FileContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateModel = new AssetUpsertModel<ComplexTestModel> { Title = "xxx" };

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("externalId"), null!, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_WithFileContent_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        await client.Invoking(c => c.UpsertAssetAsync<ComplexTestModel>(Reference.ByExternalId("externalId"), content, null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_WithFileContent_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<AssetModel>(Asset, SharedTestJsonOptions.Default)!;

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        var content = new FileContentSource(stream, fileName, contentType);

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/{fileName}")
            .Respond("application/json", File_);
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), content, updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_WithFileContent_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        var updateModel = new AssetUpsertModel { Title = "xxx" };

        await client.Invoking(c => c.UpsertAssetAsync(null!, content, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_WithFileContent_FileContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var updateModel = new AssetUpsertModel { Title = "xxx" };

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("externalId"), null!, updateModel))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_WithFileContent_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        var content = new FileContentSource(
            new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK")),
            "Hello.txt",
            "text/plain");

        await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("externalId"), content, null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAssetAsync_ById_DeletesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Empty);
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/assets/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.Invoking(c => c.DeleteAssetAsync(identifier))
            .Should().NotThrowAsync();

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteAssetAsync_ByCodename_DeletesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/assets/codename/c")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.Invoking(c => c.DeleteAssetAsync(Reference.ByCodename("c")))
            .Should().NotThrowAsync();

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteAssetAsync_ByExternalId_DeletesAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/assets/external-id/externalId")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.Invoking(c => c.DeleteAssetAsync(Reference.ByExternalId("externalId")))
            .Should().NotThrowAsync();

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteAssetAsync_IdentifierIsNull_DeletesAsset()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.DeleteAssetAsync(null!))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UploadFileAsync_UploadsFile()
    {
        var (client, mock) = MockClientFactory.Create();

        var expected = JsonSerializer.Deserialize<FileReference>(File_, SharedTestJsonOptions.Default);

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK"));
        var fileName = "Hello.txt";
        var contentType = "text/plain";

        var content = new FileContentSource(stream, fileName, contentType);

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/files/{fileName}")
            .Respond("application/json", File_);

        var response = await client.UploadFileAsync(content);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UploadFileAsync_ContentIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.UploadFileAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

}
