using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentTypeTests
{
    private static string ContentType => Fixture("ContentType.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ContentType", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonConvert.DeserializeObject<List<T>>(JObject.Parse(p).Properties().First().Value.ToString())!)
            .ToList();

    [Fact]
    public async Task ListContentTypesAsync_WithContinuation_ListsContentTypes()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("ContentTypesPage1.json");
        var page2 = Fixture("ContentTypesPage2.json");
        var page3 = Fixture("ContentTypesPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/types";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListContentTypesAsync().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(ConcatPages<ContentTypeModel>(page1, page2, page3));
    }

    [Fact]
    public async Task GetContentTypeAsync_ById_GetsContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types/{identifier.Id}")
            .Respond("application/json", ContentType);

        var response = await client.GetContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
    }

    [Fact]
    public async Task GetContentTypeAsync_ByCodename_GetsContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types/codename/{identifier.Codename}")
            .Respond("application/json", ContentType);

        var response = await client.GetContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
    }

    [Fact]
    public async Task GetContentTypeAsync_ByExternalId_GetsContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types/external-id/{identifier.ExternalId}")
            .Respond("application/json", ContentType);

        var response = await client.GetContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
    }

    [Fact]
    public async Task GetContentTypeAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetContentTypeAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateContentTypeAsync_CreatesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentTypeModel>(ContentType)!;

        var createModel = new ContentTypeCreateModel
        {
            Codename = expected.Codename,
            ContentGroups = expected.ContentGroups,
            Elements = expected.Elements,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/types")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentType);

        var response = await client.CreateContentTypeAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentTypeCreateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeCreateModel>(JsonConvert.SerializeObject(createModel)));
    }

    [Fact]
    public async Task CreateContentTypeAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateContentTypeAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteContentTypeAsync_ById_DeletesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/types/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentTypeAsync_ByCodename_DeletesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/types/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentTypeAsync_ByExternalId_DeletesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("external");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/types/external-id/{identifier.ExternalId}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentTypeAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteContentTypeAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ById_ModifiesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/types/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentType);

        var response = await client.ModifyContentTypeAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
        capturedBody.Should().NotBeNull();
        // Heterogeneous polymorphic operation list: deep per-field equivalence needed the demolished test-only
        // converter. Assert the part that's behaviourally meaningful and converter-free — the ordered sequence of
        // operation kinds (PATCH order matters), via each element's stable "op" discriminator.
        var sentOps = JArray.Parse(capturedBody!).Select(t => (string?)t["op"]);
        var expectedOps = JArray.Parse(JsonConvert.SerializeObject(changes)).Select(t => (string?)t["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ByCodename_ModifiesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByCodename("tweet");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/types/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentType);

        var response = await client.ModifyContentTypeAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
        capturedBody.Should().NotBeNull();
        var sentOps = JArray.Parse(capturedBody!).Select(t => (string?)t["op"]);
        var expectedOps = JArray.Parse(JsonConvert.SerializeObject(changes)).Select(t => (string?)t["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ByExternId_ModifiesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByExternalId("tweet");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/types/external-id/{identifier.ExternalId}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ContentType);

        var response = await client.ModifyContentTypeAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeModel>(ContentType));
        capturedBody.Should().NotBeNull();
        var sentOps = JArray.Parse(capturedBody!).Select(t => (string?)t["op"]);
        var expectedOps = JArray.Parse(JsonConvert.SerializeObject(changes)).Select(t => (string?)t["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        List<ContentTypeOperationBaseModel> changes = new();

        await client.Invoking(x => x.ModifyContentTypeAsync(null!, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeAsync(Reference.ByCodename("tweet"), null!)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ModifyContentTypeAsync_NoChanges_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeAsync(Reference.ByCodename("tweet"), new List<ContentTypeOperationBaseModel> { }))
            .Should().ThrowAsync<ArgumentException>();
    }

    private static List<ContentTypeOperationBaseModel> GetChanges() => new()
        {
            new ContentTypeRemovePatchModel
            {
                Path = $"/elements/codename:none"
            },
            new ContentTypeReplacePatchModel
            {
                Value = "bla bla bla",
                Path = $"/elements/codename:display_options/guidelines"
            },
            new ContentTypeAddIntoPatchModel
            {
                Value = new TextElementMetadataModel
                {
                    Name = "Tweet link",
                    Guidelines = "new guidelines",
                    IsRequired = true,
                    ExternalId = "20bf9ba1-28fe-203c-5920-6f9610498fb9",
                    Codename = "tweet_link",
                    MaximumTextLength = null
                },
                Before = Reference.ByCodename("theme"),
                Path = "/elements"
            },
            new ContentTypeMovePatchModel {
                Path = "/elements/codename:display_options",
                After = Reference.ByCodename("theme")
            }
        };
}
