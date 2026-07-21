using AwesomeAssertions;
using Kontent.Ai.Management.Models.ContentModel.Patch;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;
using System.Text.Json.Nodes;

using static Kontent.Ai.Management.Tests.Base.PagedFixtures;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentTypeTests
{
    private static string ContentType => Fixture("ContentType.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ContentType", name));

    [Fact]
    public async Task ListContentTypesAsync_PagesThroughAllContentTypes()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("ContentTypesPage1.json");
        var page2 = Fixture("ContentTypesPage2.json");
        var page3 = Fixture("ContentTypesPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/types";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var listResult = await client.ListContentTypesAsync();
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ContentTypeModel> contentTypes = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        contentTypes.Should().BeEquivalentTo(ConcatPages<ContentTypeModel>(page1, page2, page3));
    }

    [Fact]
    public async Task GetContentTypeAsync_ById_GetsContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types/{identifier.Id}")
            .Respond("application/json", ContentType);

        var result = await client.GetContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetContentTypeAsync_ByCodename_GetsContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types/codename/{identifier.Codename}")
            .Respond("application/json", ContentType);

        var result = await client.GetContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetContentTypeAsync_ByExternalId_GetsContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types/external-id/{identifier.ExternalId}")
            .Respond("application/json", ContentType);

        var result = await client.GetContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
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
        var expected = JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default)!;

        var createModel = new ContentTypeCreateModel
        {
            Codename = expected.Codename,
            ContentGroups = expected.ContentGroups,
            Elements = expected.Elements,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/types")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ContentType);

        var result = await client.CreateContentTypeAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(createModel);
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

        var result = await client.DeleteContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteContentTypeAsync_ByCodename_DeletesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/types/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteContentTypeAsync_ByExternalId_DeletesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("external");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/types/external-id/{identifier.ExternalId}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteContentTypeAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
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

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/types/{identifier.Id}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ContentType);

        var result = await client.ModifyContentTypeAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        // Heterogeneous polymorphic operation list: assert the converter-free, behaviourally meaningful part — the
        // ordered sequence of operation kinds (PATCH order matters), via each element's stable "op" discriminator.
        var sentOps = JsonNode.Parse(capturedBody.Value!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ByCodename_ModifiesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByCodename("tweet");

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/types/codename/{identifier.Codename}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ContentType);

        var result = await client.ModifyContentTypeAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        var sentOps = JsonNode.Parse(capturedBody.Value!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ByExternId_ModifiesContentType()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByExternalId("tweet");

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/types/external-id/{identifier.ExternalId}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ContentType);

        var result = await client.ModifyContentTypeAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<ContentTypeModel>(ContentType, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        var sentOps = JsonNode.Parse(capturedBody.Value!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyContentTypeAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeAsync(null!, GetChanges())).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyContentTypeAsync_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyContentTypeAsync(Reference.ByCodename("tweet"), null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    private static List<ContentModelOperationBaseModel> GetChanges() => new()
        {
            new ContentModelRemovePatchModel
            {
                Path = $"/elements/codename:none"
            },
            new ContentModelReplacePatchModel
            {
                Value = "bla bla bla",
                Path = $"/elements/codename:display_options/guidelines"
            },
            new ContentModelAddIntoPatchModel
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
            new ContentModelMovePatchModel {
                Path = "/elements/codename:display_options",
                After = Reference.ByCodename("theme")
            }
        };
}
