using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentTypeTests
{
    private readonly Scenario _scenario;

    public ContentTypeTests()
    {
        _scenario = new Scenario(folder: "ContentType");
    }

    [Fact]
    public async void ListContentTypesAsync_WithContinuation_ListsContentTypes()
    {
        var client = _scenario
            .WithResponses("ContentTypesPage1.json", "ContentTypesPage2.json", "ContentTypesPage3.json")
            .CreateManagementClient();

        var response = await client.ListContentTypesAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types")
            .Validate();
    }

    [Fact]
    public async void GetContentTypeAsync_ById_GetsContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetContentTypeAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetContentTypeAsync_ByCodename_GetsContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.GetContentTypeAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void GetContentTypeAsync_ByExternalId_GetsContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.GetContentTypeAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void GetContentTypeAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CreateContentTypeAsync_CreatesContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentTypeModel>();

        var createModel = new ContentTypeCreateModel
        {
            Codename = expected.Codename,
            ContentGroups = expected.ContentGroups,
            Elements = expected.Elements,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        var response = await client.CreateContentTypeAsync(createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types")
            .Validate();
    }

    [Fact]
    public async void CreateContentTypeAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteContentTypeAsync_ById_DeletesContentType()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        await client.DeleteContentTypeAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentTypeAsync_ByCodename_DeletesContentType()
    {
        var client = _scenario.CreateManagementClient();


        var identifier = Reference.ByCodename("codename");
        await client.DeleteContentTypeAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentTypeAsync_ByExternalId_DeletesContentType()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByExternalId("external");
        await client.DeleteContentTypeAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/external-id/{identifier.ExternalId}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentTypeAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyContentTypeAsync_ById_ModifiesContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.ModifyContentTypeAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifyContentTypeAsync_ByCodename_ModifiesContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ByCodename("tweet");
        var response = await client.ModifyContentTypeAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void ModifyContentTypeAsync_ByExternId_ModifiesContentType()
    {
        var client = _scenario
            .WithResponses("ContentType.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ByExternalId("tweet");
        var response = await client.ModifyContentTypeAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void ModifyContentTypeAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        List<ContentTypeOperationBaseModel> changes = new();

        await client.Invoking(x => x.ModifyContentTypeAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyContentTypeAsync_ChangesAreNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyContentTypeAsync(Reference.ByCodename("tweet"), null)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async void ModifyContentTypeAsync_NoChanges_Throws()
    {
        var client = _scenario.CreateManagementClient();

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
