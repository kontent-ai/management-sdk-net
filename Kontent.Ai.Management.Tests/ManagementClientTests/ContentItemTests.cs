using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ContentItemTests
{
    private readonly Scenario _scenario;

    public ContentItemTests()
    {
        _scenario = new Scenario(folder: "ContentItem");
    }

    [Fact]
    public async void ListContentItemsAsync_WithContinuation_ListsContentItems()
    {
        var client = _scenario
            .WithResponses("ContentItemPage1.json", "ContentItemPage2.json", "ContentItemPage3.json")
            .CreateManagementClient();

        var response = await client.ListContentItemsAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items")
            .Validate();
    }

    [Fact]
    public async void GetContentItemAsync_ById_GetsContentItems()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetContentItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetContentItemAsync_ByCodename_GetsContentItems()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.GetContentItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void GetContentItemAsync_ByExternalId_GetsContentItems()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.GetContentItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void GetContentItemAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetContentItemAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CreateContentItemAsync_CreatesContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

        var createModel = new ContentItemCreateModel
        {
            Codename = expected.Codename,
            Collection= expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        };

        var response = await client.CreateContentItemAsync(createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items")
            .Validate();
    }

    [Fact]
    public async void CreateContentItemAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateContentItemAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpsertContentItemAsync_ById_UpsertModel_UpsertsContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

        var upsertModel = new ContentItemUpsertModel
        {
            Codename = expected.Codename,
            Collection= expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name,
            SitemapLocations = expected.SitemapLocations
        };

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.UpsertContentItemAsync(identifier, upsertModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(upsertModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void UpsertContentItemAsync_ByCodename_UpsertModel_UpsertsContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

        var upsertModel = new ContentItemUpsertModel
        {
            Codename = expected.Codename,
            Collection= expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name,
            SitemapLocations = expected.SitemapLocations
        };

        var identifier = Reference.ByCodename("codename");
        var response = await client.UpsertContentItemAsync(identifier, upsertModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(upsertModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void UpsertContentItemAsync_ByExternalId_UpsertModel_UpsertsContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

        var upsertModel = new ContentItemUpsertModel
        {
            Codename = expected.Codename,
            Collection= expected.Collection,
            Type = expected.Type,
            ExternalId = expected.ExternalId,
            Name = expected.Name,
            SitemapLocations = expected.SitemapLocations
        };

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.UpsertContentItemAsync(identifier, upsertModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(upsertModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async Task UpsertContentItemAsync_UpsertModel_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertContentItemAsync(null, new ContentItemUpsertModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertContentItemAsync_UpsertModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ByExternalId("externalId");

        await client.Invoking(x => x.UpsertContentItemAsync(identifier, null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpsertContentItemAsync_ById_ItemModel_UpsertsContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

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
        var response = await client.UpsertContentItemAsync(identifier, model);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(model)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void UpsertContentItemAsync_ByCodename_ItemModel_UpsertsContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

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
        var response = await client.UpsertContentItemAsync(identifier, model);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(model)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void UpsertContentItemAsync_ByExternalId_ItemModel_UpsertsContentItem()
    {
        var client = _scenario
            .WithResponses("ContentItem.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<ContentItemModel>();

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
        var response = await client.UpsertContentItemAsync(identifier, model);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(model)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async Task UpsertContentItemAsync_ItemModel_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertContentItemAsync(null, new ContentItemModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertContentItemAsync_ItemModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ByExternalId("externalId");

        await client.Invoking(x => x.UpsertContentItemAsync(identifier, (ContentItemModel)null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteContentItemAsync_ById_DeletesContentItem()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        await client.DeleteContentItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentItemAsync_ByCodename_DeletesContentItem()
    {
        var client = _scenario.CreateManagementClient();


        var identifier = Reference.ByCodename("codename");
        await client.DeleteContentItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteContentItemAsync_ByExternalId_DeletesContentItem()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByExternalId("external");
        await client.DeleteContentItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/items/external-id/{identifier.ExternalId}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }


    [Fact]
    public async void DeleteContentItemAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }
}
