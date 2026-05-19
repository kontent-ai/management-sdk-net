using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentItemTests
{
    private static string ContentItem => Fixture("ContentItem.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ContentItem", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonConvert.DeserializeObject<List<T>>(JObject.Parse(p).Properties().First().Value.ToString())!)
            .ToList();

    [Fact]
    public async Task ListContentItemsAsync_WithContinuation_ListsContentItems()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("ContentItemPage1.json");
        var page2 = Fixture("ContentItemPage2.json");
        var page3 = Fixture("ContentItemPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/items";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListContentItemsAsync().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(ConcatPages<ContentItemModel>(page1, page2, page3));
    }

    [Fact]
    public async Task GetContentItemAsync_ById_GetsContentItems()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}")
            .Respond("application/json", ContentItem);

        var response = await client.GetContentItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
    }

    [Fact]
    public async Task GetContentItemAsync_ByCodename_GetsContentItems()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/codename/{identifier.Codename}")
            .Respond("application/json", ContentItem);

        var response = await client.GetContentItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
    }

    [Fact]
    public async Task GetContentItemAsync_ByExternalId_GetsContentItems()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/external-id/{identifier.ExternalId}")
            .Respond("application/json", ContentItem);

        var response = await client.GetContentItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
    }

    [Fact]
    public async Task GetContentItemAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetContentItemAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateContentItemAsync_CreatesContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var createModel = new ContentItemCreateModel
        {
            Codename = expected.Codename,
            Collection = expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.CreateContentItemAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemCreateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemCreateModel>(JsonConvert.SerializeObject(createModel)));
    }

    [Fact]
    public async Task CreateContentItemAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateContentItemAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertContentItemAsync_ById_UpsertModel_UpsertsContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var upsertModel = new ContentItemUpsertModel
        {
            Codename = expected.Codename,
            Collection = expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name,
            SitemapLocations = expected.SitemapLocations
        };

        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.UpsertContentItemAsync(identifier, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemUpsertModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemUpsertModel>(JsonConvert.SerializeObject(upsertModel)));
    }

    [Fact]
    public async Task UpsertContentItemAsync_ByCodename_UpsertModel_UpsertsContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var upsertModel = new ContentItemUpsertModel
        {
            Codename = expected.Codename,
            Collection = expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name,
            SitemapLocations = expected.SitemapLocations
        };

        var identifier = Reference.ByCodename("codename");

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/items/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.UpsertContentItemAsync(identifier, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemUpsertModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemUpsertModel>(JsonConvert.SerializeObject(upsertModel)));
    }

    [Fact]
    public async Task UpsertContentItemAsync_ByExternalId_UpsertModel_UpsertsContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var upsertModel = new ContentItemUpsertModel
        {
            Codename = expected.Codename,
            Collection = expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name,
            SitemapLocations = expected.SitemapLocations
        };

        var identifier = Reference.ByExternalId("externalId");

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/items/external-id/{identifier.ExternalId}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.UpsertContentItemAsync(identifier, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemUpsertModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemUpsertModel>(JsonConvert.SerializeObject(upsertModel)));
    }

    [Fact]
    public async Task UpsertContentItemAsync_UpsertModel_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertContentItemAsync(null!, new ContentItemUpsertModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertContentItemAsync_UpsertModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");

        await client.Invoking(x => x.UpsertContentItemAsync(identifier, (ContentItemUpsertModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertContentItemAsync_ById_ItemModel_UpsertsContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var model = new ContentItemModel
        {
            Name = expected.Name,
            Codename = expected.Codename,
            Collection = expected.Collection,
            ExternalId = expected.ExternalId,
            SitemapLocations = expected.SitemapLocations,
            Type = expected.Type
        };

        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.UpsertContentItemAsync(identifier, model);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(JsonConvert.SerializeObject(model)));
    }

    [Fact]
    public async Task UpsertContentItemAsync_ByCodename_ItemModel_UpsertsContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var model = new ContentItemModel
        {
            Name = expected.Name,
            Codename = expected.Codename,
            Collection = expected.Collection,
            ExternalId = expected.ExternalId,
            SitemapLocations = expected.SitemapLocations,
            Type = expected.Type
        };

        var identifier = Reference.ByCodename("codename");

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/items/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.UpsertContentItemAsync(identifier, model);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(JsonConvert.SerializeObject(model)));
    }

    [Fact]
    public async Task UpsertContentItemAsync_ByExternalId_ItemModel_UpsertsContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentItemModel>(ContentItem)!;

        var model = new ContentItemModel
        {
            Name = expected.Name,
            Codename = expected.Codename,
            Collection = expected.Collection,
            ExternalId = expected.ExternalId,
            SitemapLocations = expected.SitemapLocations,
            Type = expected.Type
        };

        var identifier = Reference.ByExternalId("externalId");

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/items/external-id/{identifier.ExternalId}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentItem);

        var response = await client.UpsertContentItemAsync(identifier, model);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(ContentItem));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentItemModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentItemModel>(JsonConvert.SerializeObject(model)));
    }

    [Fact]
    public async Task UpsertContentItemAsync_ItemModel_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertContentItemAsync(null!, new ContentItemModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertContentItemAsync_ItemModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");

        await client.Invoking(x => x.UpsertContentItemAsync(identifier, (ContentItemModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteContentItemAsync_ById_DeletesContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentItemAsync_ByCodename_DeletesContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/items/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentItemAsync_ByExternalId_DeletesContentItem()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("external");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/items/external-id/{identifier.ExternalId}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentItemAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteContentTypeAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }
}
