using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentTypeSnippetTests
{
    private static string Snippet => Fixture("Snippet.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ContentTypeSnippet", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonConvert.DeserializeObject<List<T>>(JObject.Parse(p).Properties().First().Value.ToString())!)
            .ToList();

    [Fact]
    public async Task ListContentTypeSnippetsAsync_WithContinuation_ListsSnippets()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("SnippetsPage1.json");
        var page2 = Fixture("SnippetsPage2.json");
        var page3 = Fixture("SnippetsPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/snippets";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListContentTypeSnippetsAsync().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(ConcatPages<ContentTypeSnippetModel>(page1, page2, page3));
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_ById_GetsContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Parse("5482e7b6-9c79-5e81-8c4b-90e172e7ab48"));
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/snippets/{identifier.Id}")
            .Respond("application/json", Snippet);

        var response = await client.GetContentTypeSnippetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_ByCodename_GetsContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("metadata");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/snippets/codename/{identifier.Codename}")
            .Respond("application/json", Snippet);

        var response = await client.GetContentTypeSnippetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_ByExternalId_GetsContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("metadata");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/snippets/external-id/{identifier.ExternalId}")
            .Respond("application/json", Snippet);

        var response = await client.GetContentTypeSnippetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetContentTypeSnippetAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateContentTypeSnippetAsync_CreatesContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<ContentTypeSnippetCreateModel>(Snippet)!;

        var createModel = new ContentTypeSnippetCreateModel
        {
            Codename = expected.Codename,
            Elements = expected.Elements,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/snippets")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Snippet);

        var response = await client.CreateContentTypeSnippetAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ContentTypeSnippetCreateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetCreateModel>(JsonConvert.SerializeObject(createModel)));
    }

    [Fact]
    public async Task CreateContentTypeSnippetAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateContentTypeSnippetAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteContentTypeSnippetAsync_ById_DeletesContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/snippets/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentTypeSnippetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentTypeSnippetAsync_ByCodename_DeletesContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/snippets/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentTypeSnippetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentTypeSnippetAsync_ByExternalId_DeletesContentTypeSnippetAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/snippets/external-id/{identifier.ExternalId}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteContentTypeSnippetAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteContentTypeSnippetAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyContentTypeSnippetAsync_ById_ModifiesContentTypeSnippet()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/snippets/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Snippet);

        var response = await client.ModifyContentTypeSnippetAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
        capturedBody.Should().NotBeNull();
        // Heterogeneous polymorphic operation list: deep per-field equivalence needed the demolished test-only
        // converter. Assert the part that's behaviourally meaningful and converter-free — the ordered sequence of
        // operation kinds (PATCH order matters), via each element's stable "op" discriminator.
        var sentOps = JArray.Parse(capturedBody!).Select(t => (string?)t["op"]);
        var expectedOps = JArray.Parse(JsonConvert.SerializeObject(changes)).Select(t => (string?)t["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeSnippetAsync_ByCodename_ModifiesContentTypeSnippet()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByCodename("codename");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/snippets/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Snippet);

        var response = await client.ModifyContentTypeSnippetAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
        capturedBody.Should().NotBeNull();
        var sentOps = JArray.Parse(capturedBody!).Select(t => (string?)t["op"]);
        var expectedOps = JArray.Parse(JsonConvert.SerializeObject(changes)).Select(t => (string?)t["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeSnippetAsync_ByExternalId_ModifiesContentTypeSnippet()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByExternalId("externalId");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/snippets/external-id/{identifier.ExternalId}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Snippet);

        var response = await client.ModifyContentTypeSnippetAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<ContentTypeSnippetModel>(Snippet));
        capturedBody.Should().NotBeNull();
        var sentOps = JArray.Parse(capturedBody!).Select(t => (string?)t["op"]);
        var expectedOps = JArray.Parse(JsonConvert.SerializeObject(changes)).Select(t => (string?)t["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(null!, GetChanges())).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyContentTypeSnippetAsync_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(Reference.ByCodename("metadata"), null!)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ModifyContentTypeSnippetAsync_NoChanges_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(Reference.ByCodename("tweet"), new List<ContentTypeSnippetOperationBaseModel> { }))
            .Should().ThrowAsync<ArgumentException>();
    }

    private static List<ContentTypeSnippetOperationBaseModel> GetChanges() => new()
    {
        new ContentTypeSnippetPatchRemoveModel
        {
            Path = $"/elements/codename:none"
        },
        new ContentTypeSnippetPatchReplaceModel
        {
            Value = "Provide all personas for which this article is relevant.",
            Path = $"/elements/codename:personas/guidelines"
        },
        new ContentTypeSnippetAddIntoPatchModel
        {
            Value = new TextElementMetadataModel
            {
                Name = "Meta description",
                Guidelines = "Sum up the blog for SEO purposes. Limit for the meta description is 160 characters.",
                IsRequired = false,
                ExternalId = "b9dc537c-2518-e4f5-8325-ce4fce26171e",
                Codename = "meta_description",
                MaximumTextLength = null
            },
            After = Reference.ByCodename("personas"),
            Path = "/elements"
        },
        new ContentTypeSnippetPatchMoveModel {
            Path = "/elements/codename:summary",
            After = Reference.ByCodename("personas")
        }
    };
}
