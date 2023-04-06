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
    private readonly Scenario _scenario;

    public SpaceTests()
    {
        _scenario = new Scenario(folder: "Space");
    }

    [Fact]
    public async void ListSpacesAsync_ListsSpaces()
    {
        var client = _scenario
            .WithResponses("Spaces.json")
            .CreateManagementClient();

        var response = await client.ListSpacesAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces")
            .Validate();
    }

    [Fact]
    public async void GetSpaceAsync_ById_GetsSpace()
    {
        var client = _scenario
            .WithResponses("Space.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetSpaceAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetSpaceAsync_ByCodename_GetsSpace()
    {
        var client = _scenario
            .WithResponses("Space.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.GetSpaceAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void GetSpaceAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetSpaceAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void GetSpaceAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetSpaceAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CreateSpaceAsync_CreatesSpace()
    {
        var client = _scenario
            .WithResponses("Space.json")
            .CreateManagementClient();

        var createModel = new SpaceCreateModel { Codename = "codename", Name = "name"};

        var response = await client.CreateSpaceAsync(createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces")
            .Validate();
    }

    [Fact]
    public async void CreateSpaceAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateSpaceAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteSpaceAsync_ById_DeletesSpace()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        await client.DeleteSpaceAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteSpaceAsync_ByCodename_DeletesSpace()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        await client.DeleteSpaceAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteSpaceAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteSpaceAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DeleteSpaceAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteSpaceAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifySpaceAsync_ById_ModifiesSpace()
    {
        var client = _scenario
            .WithResponses("ModifySpace_Replace_ModifiesSpace.json")
            .CreateManagementClient();

        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" }
        };

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.ModifySpaceAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifySpaceAsync_ByCodename_ModifiesSpace()
    {
        var client = _scenario
            .WithResponses("ModifySpace_Replace_ModifiesSpace.json")
            .CreateManagementClient();

        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" }
        };

        var identifier = Reference.ByCodename("codename");
        var response = await client.ModifySpaceAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/spaces/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void ModifySpaceAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" }
        };

        await client.Invoking(x => x.ModifySpaceAsync(Reference.ByExternalId("externalId"), changes)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void ModifySpaceAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" }
        };

        await client.Invoking(x => x.ModifySpaceAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
    }
}
