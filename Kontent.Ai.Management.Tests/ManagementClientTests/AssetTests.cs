using System.Text;
using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Tests.Base;
using Kontent.Ai.Management.Tests.Data;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetTests
{
    private static string Asset => Fixture("Asset.json");
    private static string File_ => Fixture("File.json");

    private static string Fixture(string name)
        => System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Asset", name));

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

        var expected = new[] {
            "00000000-0000-0000-0000-000000000000",
            "10000000-0000-0000-0000-000000000000",
            "20000000-0000-0000-0000-000000000000"
        }.Select(GetExpectedDynamicAssetModel);

        var response = await client.ListAssetsAsync().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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

        var expected = new[] {
            "00000000-0000-0000-0000-000000000000",
            "10000000-0000-0000-0000-000000000000",
            "20000000-0000-0000-0000-000000000000"
        }.Select(GetExpectedStronglyTypedAssetModel);

        var response = await client.ListAssetsAsync<ComplexTestModel>().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_ById_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedStronglyTypedAssetModel();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ById(expected.Id));

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_ByCodename_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedStronglyTypedAssetModel();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ByCodename(expected.Codename));

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAssetAsync_StronglyTyped_ByExternalId_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedStronglyTypedAssetModel();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ByExternalId(expected.ExternalId));

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedDynamicAssetModel();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync(Reference.ById(expected.Id));

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAssetAsync_DynamicallyTyped_ByCodename_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedDynamicAssetModel();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync(Reference.ByCodename(expected.Codename));

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAssetAsync_DynamicallyTyped_ByExternalId_GetsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedDynamicAssetModel();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.GetAssetAsync(Reference.ByExternalId(expected.ExternalId));

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedStronglyTypedAssetModel();

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
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedDynamicAssetModel();

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
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedStronglyTypedAssetModel();

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
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedDynamicAssetModel();

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
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedStronglyTypedAssetModel();

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_ByCodename_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedStronglyTypedAssetModel();

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByCodename(expected.Codename), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_StronglyTyped_ByExternalId_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedStronglyTypedAssetModel();

        var updateModel = new AssetUpsertModel<ComplexTestModel>
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByExternalId(expected.ExternalId), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedDynamicAssetModel();

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/{expected.Id}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_ByCodename_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedDynamicAssetModel();

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/codename/{expected.Codename}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByCodename(expected.Codename), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpsertAssetAsync_DynamicallyTyped_ByExternalId_UpsertsAsset()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = GetExpectedDynamicAssetModel();

        var updateModel = new AssetUpsertModel
        {
            Title = expected.Title,
            Elements = expected.Elements
        };

        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/assets/external-id/{expected.ExternalId}")
            .Respond("application/json", Asset);

        var response = await client.UpsertAssetAsync(Reference.ByExternalId(expected.ExternalId), updateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedStronglyTypedAssetModel();

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
        response.Should().BeEquivalentTo(expected);
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
        var expected = GetExpectedDynamicAssetModel();

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
        response.Should().BeEquivalentTo(expected);
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

        var expected = JsonConvert.DeserializeObject<FileReference>(File_);

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

    private static AssetModel GetExpectedDynamicAssetModel(string assetId = "01647205-c8c4-4b41-b524-1a98a7b12750")
    {
        var stronglyTyped = GetExpectedStronglyTypedAssetModel(assetId);

        return new AssetModel
        {
            Id = stronglyTyped.Id,
            Codename = stronglyTyped.Codename,
            ExternalId = stronglyTyped.ExternalId,
            FileName = stronglyTyped.FileName,
            Title = stronglyTyped.Title,
            Size = stronglyTyped.Size,
            Type = stronglyTyped.Type,
            Url = stronglyTyped.Url,
            ImageWidth = stronglyTyped.ImageWidth,
            ImageHeight = stronglyTyped.ImageHeight,
            FileReference = stronglyTyped.FileReference,
            LastModified = stronglyTyped.LastModified,
            Descriptions = stronglyTyped.Descriptions,
            Collection = stronglyTyped.Collection,
            Elements = ElementsData.GetExpectedDynamicElements(),
        };
    }

    private static AssetModel<ComplexTestModel> GetExpectedStronglyTypedAssetModel(string assetId = "01647205-c8c4-4b41-b524-1a98a7b12750") => new()
    {
        Id = Guid.Parse(assetId),
        Codename = "my_super_asset",
        ExternalId = "asset-1",
        FileName = "our-story.jpg",
        Title = "My super asset",
        Size = 69518,
        Type = "image/jpeg",
        Url = "https://assets-eu-01.kc-usercontent.com/a9931a80-9af4-010b-0590-ecb1273cf1b8/36f361fa-7f65-446f-b16e-170455766f3e/our-story.jpg",
        ImageWidth = 2160,
        ImageHeight = 1000,
        FileReference = new FileReference
        {
            Id = "36f361fa-7f65-446f-b16e-170455766f3e",
            Type = FileReferenceTypeEnum.Internal,
        },
        LastModified = DateTimeOffset.Parse("2021-11-06T13:57:51.3425375Z").UtcDateTime,
        Descriptions = new[]
        {
            new AssetDescription
            {
                Language = Reference.ById(Guid.Empty),
                Description = "Dancing Goat Café - Los Angeles"
            },
            new AssetDescription
            {
                Language = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024")),
                Description = "Bolso de cafe en grano"
            }
        },
        Collection = new AssetCollectionReference {
            Reference = Reference.ById(Guid.Empty)
        },
        Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
    };
}
