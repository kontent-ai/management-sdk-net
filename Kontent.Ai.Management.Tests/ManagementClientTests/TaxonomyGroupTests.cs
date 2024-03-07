using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class TaxonomyGroupTests
{
    private readonly Scenario _scenario;

    public TaxonomyGroupTests()
    {
        _scenario = new Scenario(folder: "TaxonomyGroup");
    }

    [Fact]
    public async void ListTaxonomyGroupsAsync_ListsTaxonomyGroups()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroupsPage1.json", "TaxonomyGroupsPage2.json")
            .CreateManagementClient();

        var response = await client.ListTaxonomyGroupsAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies")
            .Validate();
    }

    [Fact]
    public async void GetTaxonomyGroupAsync_ById_GetsTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetTaxonomyGroupAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetTaxonomyGroupAsync_ByCodename_GetsTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        var response = await client.GetTaxonomyGroupAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void GetTaxonomyGroupAsync_ByExternalId_GetsTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

        var identifier = Reference.ByExternalId("external");
        var response = await client.GetTaxonomyGroupAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void GetTaxonomyGroupAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetTaxonomyGroupAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CreateTaxonomyGroupAsync_CreatesTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

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
                    ExternalId = "f04c8552-1b97-a49b-3944-79275622f471",
                    Terms = Array.Empty<TaxonomyTermCreateModel>()
                }
            }
        };

        var response = await client.CreateTaxonomyGroupAsync(createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies")
            .Validate();
    }

    [Fact]
    public async void CreateTaxonomyGroupAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateTaxonomyGroupAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteTaxonomyGroupAsync_ById_DeletesTaxonomyGroup()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByCodename("codename");
        await client.DeleteTaxonomyGroupAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteTaxonomyGroupAsync_ByCodename_DeletesTaxonomyGroup()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        await client.DeleteTaxonomyGroupAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteTaxonomyGroupAsync_ByExternalId_DeletesTaxonomyGroup()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ByExternalId("external");
        await client.DeleteTaxonomyGroupAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/external-id/{identifier.ExternalId}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteTaxonomyGroupAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteTaxonomyGroupAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyTaxonomyGroupAsync_ById_ModifiesTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.ModifyTaxonomyGroupAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/{identifier.Id}")
            .Validate();
    }


    [Fact]
    public async void ModifyTaxonomyGroupAsync_ByCodename_ModifiesTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ByCodename("codename");
        var response = await client.ModifyTaxonomyGroupAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void ModifyTaxonomyGroupAsync_ByExternalId_ModifiesTaxonomyGroup()
    {
        var client = _scenario
            .WithResponses("TaxonomyGroup.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var identifier = Reference.ByExternalId("external");
        var response = await client.ModifyTaxonomyGroupAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/taxonomies/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void ModifyTaxonomyGroupAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        List<TaxonomyGroupOperationBaseModel> changes = new();

        await client.Invoking(x => x.ModifyTaxonomyGroupAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
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
        }
    };
}
