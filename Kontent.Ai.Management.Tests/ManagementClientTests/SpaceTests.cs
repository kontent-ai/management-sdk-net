using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class SpaceTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public SpaceTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("Space");
    }

    [Fact]
    public async void CreateSpace_CreatesSpace()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Space.json");

        var expected = _fileSystemFixture.GetExpectedResponse<SpaceModel>("Space.json");

        var createModel = new SpaceCreateModel { Codename = expected.Codename, Name = expected.Name };

        var response = await client.CreateSpaceAsync(createModel);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void ListSpaces_ListsSpaces()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Spaces.json");

        var expected = _fileSystemFixture.GetExpectedResponse<IEnumerable<SpaceModel>>("Spaces.json");

        var response = await client.ListSpacesAsync();

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetSpace_ById_GetsSpace()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Space.json");

        var expected = _fileSystemFixture.GetExpectedResponse<SpaceModel>("Space.json");

        var response = await client.GetSpaceAsync(Reference.ById(expected.Id));

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetSpace_ByCodename_GetsSpace()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Space.json");

        var expected = _fileSystemFixture.GetExpectedResponse<SpaceModel>("Space.json");

        var response = await client.GetSpaceAsync(Reference.ByCodename(expected.Codename));

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void ModifySpace_Replace_ModifiesSpace()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("ModifySpace_Replace_ModifiesSpace.json");

        var expected = _fileSystemFixture.GetExpectedResponse<SpaceModel>("ModifySpace_Replace_ModifiesSpace.json");

        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" }
        };

        var response =  await client.ModifySpaceAsync(Reference.ById(expected.Id), changes);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void DeleteSpace_ById_DeletesSpace()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();
        
        var deleteSpace = async () => await client.DeleteSpaceAsync(Reference.ById(Guid.NewGuid()));

        await deleteSpace.Should().NotThrowAsync();
    }

    [Fact]
    public async void DeleteSpace_ByCodename_DeletesSpace()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();
        
        var deleteSpace = async () => await client.DeleteSpaceAsync(Reference.ByCodename("space_1"));

        await deleteSpace.Should().NotThrowAsync();
    }
}
    