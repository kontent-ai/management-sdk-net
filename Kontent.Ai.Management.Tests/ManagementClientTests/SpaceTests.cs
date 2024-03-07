using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class SpaceTests : IClassFixture<FileSystemFixture>
{
    private static readonly string SpacesBaseUrl = $"{Endpoint}/projects/{ENVIRONMENT_ID}/spaces";
    private readonly Scenario _scenario = new("Space");

    [Fact]
    public async void CreateSpace_CreatesSpace()
    {
        var client = _scenario.WithResponses("Space.json").CreateManagementClient();
        var expected = _scenario.GetExpectedResponse<SpaceModel>();
        var createModel = new SpaceCreateModel {
            Codename = expected.Codename,
            Name = expected.Name,
            WebSpotlightRootItem = expected.WebSpotlightRootItem,
            Collections = expected.Collections
        };

        var response = await client.CreateSpaceAsync(createModel);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url(SpacesBaseUrl)
            .Validate();
    }

    [Fact]
    public async void CreateSpace_ModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateSpaceAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ListSpaces_ListsSpaces()
    {
        var client = _scenario.WithResponses("Spaces.json").CreateManagementClient();

        var response = await client.ListSpacesAsync();

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(SpacesBaseUrl)
            .Validate();
    }

    [Fact]
    public async void GetSpace_ById_GetsSpace()
    {
        var client = _scenario.WithResponses("Space.json").CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());

        var response = await client.GetSpaceAsync(identifier);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(SpacesBaseUrl + $"/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetSpace_ByCodename_GetsSpace()
    {
        var client = _scenario.WithResponses("Space.json").CreateManagementClient();
        var identifier = Reference.ByCodename("space_1");

        var response = await client.GetSpaceAsync(identifier);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(SpacesBaseUrl + $"/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void GetSpace_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetSpaceAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifySpace_Replace_ModifiesSpace()
    {
        var client = _scenario.WithResponses("ModifySpace_Replace_ModifiesSpace.json").CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());
        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" },
            new() { PropertyName = PropertyName.WebSpotlightRootItem, Value = identifier },
            new() { PropertyName = PropertyName.Collections, Value = new[] {
                    Reference.ByCodename("collection_codename"),
                    Reference.ById(Guid.NewGuid()) }
            }
        };

        var response =  await client.ModifySpaceAsync(identifier, changes);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Patch)
            .RequestPayload(changes)
            .Response(response)
            .Url(SpacesBaseUrl + $"/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifySpace_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" }
        };

        await client.Invoking(x => x.ModifySpaceAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifySpace_ChangesAreNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.Invoking(x => x.ModifySpaceAsync(identifier, null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteSpace_ById_DeletesSpace()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.DeleteSpaceAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url(SpacesBaseUrl + $"/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteSpace_ByCodename_DeletesSpace()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ByCodename("space_1");

        await client.DeleteSpaceAsync(identifier);

        _scenario.CreateExpectations()
            .Url(SpacesBaseUrl + $"/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteSpace_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteSpaceAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }
}
    