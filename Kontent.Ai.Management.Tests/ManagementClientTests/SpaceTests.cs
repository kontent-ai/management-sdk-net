using AwesomeAssertions;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class SpaceTests
{
    private static string SpacesUrl => $"{MockClientFactory.BaseUrl}/spaces";

    private static string Space => Fixture("Space.json");
    private static string Spaces => Fixture("Spaces.json");
    private static string ModifySpaceReplace => Fixture("ModifySpace_Replace_ModifiesSpace.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Space", name));

    [Fact]
    public async Task CreateSpace_CreatesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<SpaceModel>(Space, SharedTestJsonOptions.Default)!;
        var createModel = new SpaceCreateModel
        {
            Codename = expected.Codename,
            Name = expected.Name,
            RootItem = expected.RootItem,
            Collections = expected.Collections
        };

        mock.Expect(HttpMethod.Post, SpacesUrl)
            .CaptureBody(out var capturedBody)
            .Respond("application/json", Space);

        var result = await client.CreateSpaceAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<SpaceModel>(Space, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(createModel);
    }

    [Fact]
    public async Task CreateSpace_ModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateSpaceAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListSpaces_ListsSpaces()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, SpacesUrl)
            .Respond("application/json", Spaces);

        var result = await client.ListSpacesAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<IReadOnlyList<SpaceModel>>(Spaces, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetSpace_ById_GetsSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{SpacesUrl}/{identifier.Id}")
            .Respond("application/json", Space);

        var result = await client.GetSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<SpaceModel>(Space, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetSpace_ByCodename_GetsSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("space_1");
        mock.Expect(HttpMethod.Get, $"{SpacesUrl}/codename/{identifier.Codename}")
            .Respond("application/json", Space);

        var result = await client.GetSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<SpaceModel>(Space, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetSpace_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetSpaceAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifySpace_Replace_ModifiesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        var changes = new SpaceReplacePatchModel[]
        {
            new() { PropertyName = SpacePropertyName.Name, Value = "New space name" },
            new() { PropertyName = SpacePropertyName.Codename, Value = "new_space_codename" },
            new() { PropertyName = SpacePropertyName.RootItem, Value = identifier },
            new() { PropertyName = SpacePropertyName.Collections, Value = new[] {
                    Reference.ByCodename("collection_codename"),
                    Reference.ById(Guid.NewGuid()) }
            }
        };

        mock.Expect(HttpMethod.Patch, $"{SpacesUrl}/{identifier.Id}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ModifySpaceReplace);

        var result = await client.ModifySpaceAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<SpaceModel>(ModifySpaceReplace, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        JsonSerializer.Deserialize<SpaceReplacePatchModel[]>(capturedBody.Value!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<SpaceReplacePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifySpace_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var changes = new SpaceReplacePatchModel[]
        {
            new() { PropertyName = SpacePropertyName.Name, Value = "New space name" }
        };

        await client.Invoking(x => x.ModifySpaceAsync(null!, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifySpace_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.Invoking(x => x.ModifySpaceAsync(identifier, null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteSpace_ById_DeletesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{SpacesUrl}/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteSpace_ByCodename_DeletesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("space_1");
        mock.Expect(HttpMethod.Delete, $"{SpacesUrl}/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteSpace_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteSpaceAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }
}
