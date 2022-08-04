using FluentAssertions;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class CollectionTests
{
    private readonly Scenario _scenario;

    public CollectionTests()
    {
        _scenario = new Scenario(folder: "Collection");
    }

    [Fact]
    public async Task ListCollections_ListsCollections()
    {
        var client = _scenario
            .WithResponses("Collections.json")
            .CreateManagementClient();

        var response = await client.ListCollectionsAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections")
            .Validate();
    }

    
    [Theory]
    [MemberData(nameof(GetIdentifers))]
    public async void ModifyCollection_Remove_RemovesCollection(Reference identifier)
    {
        var client = _scenario
            .WithResponses("Collections.json")
            .CreateManagementClient();

        var changes = new[] { new CollectionRemovePatchModel
        {
            CollectionIdentifier = identifier
        }};

        var response = await client.ModifyCollectionAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections")
            .Validate();
    }

    [Theory]
    [MemberData(nameof(GetIdentifers))]
    public async void ModifyCollection_Move_After_MovesCollection(Reference identifier)
    {
        var client = _scenario
            .WithResponses("Collections.json")
            .CreateManagementClient();

        var changes = new[] { new CollectionMovePatchModel
        {
            Reference = identifier,
            After = Reference.ById(Guid.Empty)
        }};

        var response = await client.ModifyCollectionAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections")
            .Validate();
    }

    [Theory]
    [MemberData(nameof(GetIdentifers))]
    public async void ModifyCollection_Move_Before_MovesCollection(Reference identifier)
    {
        var client = _scenario
            .WithResponses("Collections.json")
            .CreateManagementClient();

        var changes = new[] { new CollectionMovePatchModel
        {
            Reference = identifier,
            Before = Reference.ByExternalId("third_external_id")
        }};

        var response = await client.ModifyCollectionAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections")
            .Validate();
    }

    [Theory]
    [MemberData(nameof(GetIdentifers))]
    public async void ModifyCollection_AddInto_MovesCollection(Reference identifier)
    {
        var client = _scenario
            .WithResponses("Collections.json")
            .CreateManagementClient();

        var expected = new CollectionCreateModel
        {
            Codename = "second_collection",
            ExternalId = "second_external_id",
            Name = "Second collection"
        };

        var changes = new[] { new CollectionAddIntoPatchModel { Value = expected, After = identifier } };

        var response = await client.ModifyCollectionAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections")
            .Validate();
    }

    [Theory]
    [MemberData(nameof(GetIdentifers))]
    public async void ModifyCollection_Replace_ReplacesCollection(Reference identifier)
    {
        var client = _scenario
            .WithResponses("Collections.json")
            .CreateManagementClient();

        var changes = new[] { new CollectionReplacePatchModel
        {
            Reference = identifier,
            PropertyName = PropertyName.Name,
            Value = "Second collection"
        }};

        var response = await client.ModifyCollectionAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections")
            .Validate();
    }

    [Fact]
    public async void ModifyCollection_ChangesAreNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyCollectionAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    public static IEnumerable<object[]> GetIdentifers()
    {
        yield return new object[] { Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")) };
        yield return new object[] { Reference.ByCodename("codename") };
        yield return new object[] { Reference.ByExternalId("external-id") };
    }
}
