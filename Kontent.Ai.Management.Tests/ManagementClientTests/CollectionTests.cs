using AwesomeAssertions;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class CollectionTests
{
    private static string Collections => Fixture("Collections.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Collection", name));

    public static TheoryData<Reference> Identifiers =>
    [
        Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
        Reference.ByCodename("codename"),
        Reference.ByExternalId("external-id"),
    ];

    [Fact]
    public async Task ListCollections_ListsCollections()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/collections")
            .Respond("application/json", Collections);

        var result = await client.ListCollectionsAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CollectionsModel>(Collections, SharedTestJsonOptions.Default));
    }

    [Theory]
    [MemberData(nameof(Identifiers))]
    public async Task ModifyCollection_Remove_RemovesCollection(Reference identifier)
    {
        var changes = new[] { new CollectionRemovePatchModel { Reference = identifier } };
        await AssertModifyCollection(changes);
    }

    [Theory]
    [MemberData(nameof(Identifiers))]
    public async Task ModifyCollection_Move_After_MovesCollection(Reference identifier)
    {
        var changes = new[] { new CollectionMovePatchModel { Reference = identifier, After = Reference.ById(Guid.Empty) } };
        await AssertModifyCollection(changes);
    }

    [Theory]
    [MemberData(nameof(Identifiers))]
    public async Task ModifyCollection_Move_Before_MovesCollection(Reference identifier)
    {
        var changes = new[] { new CollectionMovePatchModel { Reference = identifier, Before = Reference.ByExternalId("third_external_id") } };
        await AssertModifyCollection(changes);
    }

    [Theory]
    [MemberData(nameof(Identifiers))]
    public async Task ModifyCollection_AddInto_MovesCollection(Reference identifier)
    {
        var value = new CollectionCreateModel
        {
            Codename = "second_collection",
            ExternalId = "second_external_id",
            Name = "Second collection"
        };
        var changes = new[] { new CollectionAddIntoPatchModel { Value = value, After = identifier } };
        await AssertModifyCollection(changes);
    }

    [Theory]
    [MemberData(nameof(Identifiers))]
    public async Task ModifyCollection_Replace_ReplacesCollection(Reference identifier)
    {
        var changes = new[] { new CollectionReplacePatchModel
        {
            Reference = identifier,
            PropertyName = PropertyName.Name,
            Value = "Second collection"
        }};
        await AssertModifyCollection(changes);
    }

    [Fact]
    public async Task ModifyCollection_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyCollectionAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    private static async Task AssertModifyCollection<T>(T[] changes) where T : CollectionOperationBaseModel
    {
        var (client, mock) = MockClientFactory.Create();
        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/collections")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Collections);

        var result = await client.ModifyCollectionAsync(changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CollectionsModel>(Collections, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<T[]>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<T[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default), opt => opt.WithStrictOrdering());
    }
}
