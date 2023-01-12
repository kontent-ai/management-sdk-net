using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentTypeSnippetTests
{
    private readonly Scenario _scenario;

    public ContentTypeSnippetTests()
    {
        _scenario = new Scenario(folder: "ContentTypeSnippet");
    }

    [Fact]
    public async Task ListContentTypeSnippetsAsync_WithContinuation_ListsSnippets()
    {
        var client = _scenario
            .WithResponses("SnippetsPage1.json", "SnippetsPage2.json", "SnippetsPage3.json")
            .CreateManagementClient();

        var response = await client.ListContentTypeSnippetsAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets")
            .Validate();
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_ById_GetsContentTypeSnippetAsync()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.Parse("5482e7b6-9c79-5e81-8c4b-90e172e7ab48"));

        var response = await client.GetContentTypeSnippetAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_ByCodename_GetsContentTypeSnippetAsync()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("metadata");

        var response = await client.GetContentTypeSnippetAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_ByExternalId_GetsContentTypeSnippetAsync()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("metadata");

        var response = await client.GetContentTypeSnippetAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void GetContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetContentTypeSnippetAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CreateContentTypeSnippetAsync_CreatesContentTypeSnippetAsync()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentTypeSnippetCreateModel>();

        var createModel = new ContentTypeSnippetCreateModel
        {
            Codename = expected.Codename,
            Elements = expected.Elements,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        var response = await client.CreateContentTypeSnippetAsync(createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets")
            .Validate();
    }


    [Fact]
    public async void CreateContentTypeSnippetAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateContentTypeSnippetAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }


    [Fact]
    public async void DeleteContentTypeSnippetAsync_ById_DeletesContentTypeSnippetAsync()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        await client.DeleteContentTypeSnippetAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentTypeSnippetAsync_ByCodename_DeletesContentTypeSnippetAsync()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        await client.DeleteContentTypeSnippetAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentTypeSnippetAsync_ByExternalId_DeletesContentTypeSnippetAsync()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        await client.DeleteContentTypeSnippetAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/external-id/{identifier.ExternalId}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteContentTypeSnippetAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_ById_ModifiesContentTypeSnippet()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.ModifyContentTypeSnippetAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_ByCodename_ModifiesContentTypeSnippet()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ByCodename("codename");
        var response = await client.ModifyContentTypeSnippetAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_ByExternalId_ModifiesContentTypeSnippet()
    {
        var client = _scenario
            .WithResponses("Snippet.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ByExternalId("externalId");
        var response = await client.ModifyContentTypeSnippetAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/snippets/external-id/{identifier.ExternalId}")
            .Validate();
    }
    [Fact]
    public async void ModifyContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(null, GetChanges())).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_ChangesAreNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(Reference.ByCodename("metadata"), null)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_NoChanges_Throws()
    {
        var client = _scenario.CreateManagementClient();

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
        }
    };

    public static IEnumerable<object[]> GetIdentifers()
    {
        yield return new object[] { Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")) };
        yield return new object[] { Reference.ByCodename("codename") };
        yield return new object[] { Reference.ByExternalId("external-id") };
    }
}
