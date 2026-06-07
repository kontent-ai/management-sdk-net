using AwesomeAssertions;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class TaxonomyGroupTests
{
    private static string TaxonomyGroup => Fixture("TaxonomyGroup.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "TaxonomyGroup", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    [Fact]
    public async Task ListTaxonomyGroupsAsync_PagesThroughAllTaxonomyGroups()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("TaxonomyGroupsPage1.json");
        var page2 = Fixture("TaxonomyGroupsPage2.json");
        var url = $"{MockClientFactory.BaseUrl}/taxonomies";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);

        var listResult = await client.ListTaxonomyGroupsAsync();
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<TaxonomyGroupModel> taxonomyGroups = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        taxonomyGroups.Should().BeEquivalentTo(ConcatPages<TaxonomyGroupModel>(page1, page2));
    }

    [Fact]
    public async Task GetTaxonomyGroupAsync_ById_GetsTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/taxonomies/{identifier.Id}")
            .Respond("application/json", TaxonomyGroup);

        var result = await client.GetTaxonomyGroupAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetTaxonomyGroupAsync_ByCodename_GetsTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/taxonomies/codename/{identifier.Codename}")
            .Respond("application/json", TaxonomyGroup);

        var result = await client.GetTaxonomyGroupAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetTaxonomyGroupAsync_ByExternalId_GetsTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("external");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/taxonomies/external-id/{identifier.ExternalId}")
            .Respond("application/json", TaxonomyGroup);

        var result = await client.GetTaxonomyGroupAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetTaxonomyGroupAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetTaxonomyGroupAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateTaxonomyGroupAsync_CreatesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var createModel = new TaxonomyGroupCreateModel
        {
            Codename = "manufacturer",
            ExternalId = "4ce421e9-c403-eee8-fdc2-74f09392a749",
            Name = "Manufacturer",
            Terms = new TaxonomyTermCreateModel[]
            {
                new TaxonomyTermCreateModel
                {
                    Codename = "aerobie",
                    Name = "Aerobie",
                    ExternalId = "f04c8552-1b97-a49b-3944-79275622f471"
                }
            }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/taxonomies")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", TaxonomyGroup);

        var result = await client.CreateTaxonomyGroupAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<TaxonomyGroupCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupCreateModel>(JsonSerializer.Serialize(createModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateTaxonomyGroupAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateTaxonomyGroupAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteTaxonomyGroupAsync_ById_DeletesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/taxonomies/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteTaxonomyGroupAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaxonomyGroupAsync_ByCodename_DeletesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/taxonomies/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteTaxonomyGroupAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaxonomyGroupAsync_ByExternalId_DeletesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("external");
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/taxonomies/external-id/{identifier.ExternalId}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteTaxonomyGroupAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaxonomyGroupAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteTaxonomyGroupAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyTaxonomyGroupAsync_ById_ModifiesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/taxonomies/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", TaxonomyGroup);

        var result = await client.ModifyTaxonomyGroupAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        // Heterogeneous polymorphic operation list: deep per-field equivalence needed the demolished test-only
        // converter. Assert the part that's behaviourally meaningful and converter-free — the ordered sequence of
        // operation kinds (PATCH order matters), via each element's stable "op" discriminator.
        var sentOps = JsonNode.Parse(capturedBody!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyTaxonomyGroupAsync_ByCodename_ModifiesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByCodename("codename");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/taxonomies/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", TaxonomyGroup);

        var result = await client.ModifyTaxonomyGroupAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        var sentOps = JsonNode.Parse(capturedBody!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyTaxonomyGroupAsync_ByExternalId_ModifiesTaxonomyGroup()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByExternalId("external");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/taxonomies/external-id/{identifier.ExternalId}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", TaxonomyGroup);

        var result = await client.ModifyTaxonomyGroupAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<TaxonomyGroupModel>(TaxonomyGroup, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        var sentOps = JsonNode.Parse(capturedBody!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyTaxonomyGroupAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        List<TaxonomyGroupOperationBaseModel> changes = new();

        await client.Invoking(x => x.ModifyTaxonomyGroupAsync(null!, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    private static List<TaxonomyGroupOperationBaseModel> GetChanges() => new()
    {
        new TaxonomyGroupRemovePatchModel
        {
            Reference = Reference.ByCodename("toBeRemoved"),
        },
        new TaxonomyGroupReplacePatchModel
        {
            PropertyName = PropertyName.Terms,
            Reference = Reference.ByCodename("old"),
            Value = new List<TaxonomyTermCreateModel> {
                new TaxonomyTermCreateModel
                {
                    Name = "Espro",
                    Codename = "espro",
                    ExternalId = "b378225f-6dfc-e261-3848-dd030a6d7883",
                    Terms = Array.Empty<TaxonomyTermCreateModel>()
                }
            }
        },
        new TaxonomyGroupAddIntoPatchModel
        {
            Value = new TaxonomyTermCreateModel
            {
                Name = "Espro",
                Codename = "espro",
                ExternalId = "b378225f-6dfc-e261-3848-dd030a6d7883",
                Terms = Array.Empty<TaxonomyTermCreateModel>()
            }
        },
        new TaxonomyGroupMovePatchModel {
            Reference = Reference.ByCodename("chemex"),
            Under = Reference.ByCodename("hario")
        }
    };
}
