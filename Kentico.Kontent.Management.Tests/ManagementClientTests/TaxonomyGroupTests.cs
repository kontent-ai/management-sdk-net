using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using Kentico.Kontent.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests;

public class TaxonomyGroupTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public TaxonomyGroupTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("TaxonomyGroup");
    }

    [Fact]
    public async void ListTaxonomyGroups_ListsTaxonomyGroups()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroups.json");

        var expectedItems = _fileSystemFixture.GetItemsOfExpectedListingResponse<TaxonomyGroupModel>("TaxonomyGroups.json");

        var response = await client.ListTaxonomyGroupsAsync();

        response.ToList().Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async void GetTaxonomyGroup_ById_GetsTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var expected = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var response = await client.GetTaxonomyGroupAsync(Reference.ById(expected.Id));

        response.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async void GetTaxonomyGroup_ByCodename_GetsTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var expected = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var response = await client.GetTaxonomyGroupAsync(Reference.ByCodename(expected.Codename));

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetTaxonomyGroup_ByExternalId_GetsTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var expected = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var response = await client.GetTaxonomyGroupAsync(Reference.ByExternalId(expected.ExternalId));

        response.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async void CreateTaxonomyGroup_CreatesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var expected = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var createModel = ToCreateModel(expected);

        var response = await client.CreateTaxonomyGroupAsync(createModel);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void DeleteTaxonomyGroup_ByCodename_DeletesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var identifier = Reference.ByCodename("codename");

        Func<Task> deleteTaxonomy = async () => await client.DeleteTaxonomyGroupAsync(identifier);

        await deleteTaxonomy.Should().NotThrowAsync();
    }

    [Fact]
    public async void DeleteTaxonomyGroup_ById_DeletesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var identifier = Reference.ById(Guid.NewGuid());

        Func<Task> deleteTaxonomy = async () => await client.DeleteTaxonomyGroupAsync(identifier);

        await deleteTaxonomy.Should().NotThrowAsync();
    }

    [Fact]
    public async void DeleteTaxonomyGroup_ByExternalId_DeletesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var identifier = Reference.ByExternalId("externalId");

        Func<Task> deleteTaxonomy = async () => await client.DeleteTaxonomyGroupAsync(identifier);

        await deleteTaxonomy.Should().NotThrowAsync();
    }


    [Fact]
    public async void ModifyTaxonomyGroup_AddInto_ModifiesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var taxonomy = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var changes = new TaxonomyGroupAddIntoPatchModel
        {
            Value = new TaxonomyTermCreateModel
            {
                Name = "Espro",
                Codename = "espro",
                ExternalId = "b378225f-6dfc-e261-3848-dd030a6d7883",
                Terms = Array.Empty<TaxonomyTermCreateModel>()
            }
        };

        var response = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(taxonomy.Codename), new List<TaxonomyGroupAddIntoPatchModel> { changes });

        response.Terms.Should().ContainEquivalentOf(changes.Value);
    }


    [Fact]
    public async void ModifyTaxonomyGroup_Replace_ModifiesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var taxonomy = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var newTerm = new TaxonomyTermCreateModel
        {
            Name = "Espro",
            Codename = "espro",
            ExternalId = "b378225f-6dfc-e261-3848-dd030a6d7883",
            Terms = Array.Empty<TaxonomyTermCreateModel>()
        };

        var changes = new TaxonomyGroupReplacePatchModel
        {
            PropertyName = PropertyName.Terms,
            Reference = Reference.ByCodename("old"),
            Value = new List<TaxonomyTermCreateModel> { newTerm }
        };

        var response = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(taxonomy.Codename), new List<TaxonomyGroupReplacePatchModel> { changes });

        response.Terms.Should().ContainEquivalentOf(newTerm);
    }

    [Fact]
    public async void ModifyTaxonomyGroup_Remove_ModifiesTaxonomyGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("TaxonomyGroup.json");

        var taxonomy = _fileSystemFixture.GetExpectedResponse<TaxonomyGroupModel>("TaxonomyGroup.json");

        var changes = new TaxonomyGroupRemovePatchModel
        {
            Reference = Reference.ByCodename("toBeRemoved"),
        };

        var response = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(taxonomy.Codename), new List<TaxonomyGroupRemovePatchModel> { changes });

        response.Terms.Should().NotContain(term => term.Codename == "toBeRemoved");
    }

    private TaxonomyGroupCreateModel ToCreateModel(TaxonomyGroupModel source) => new()
    {
        Codename = source.Codename,
        ExternalId = source.ExternalId,
        Name = source.Name,
        Terms = ToCreateModel(source.Terms),
    };

    private IEnumerable<TaxonomyTermCreateModel> ToCreateModel(IEnumerable<TaxonomyTermModel> terms)
    {
        var result = new List<TaxonomyTermCreateModel>();

        foreach (var term in terms)
        {
            result.Add(new TaxonomyTermCreateModel
            {
                Codename = term.Codename,
                ExternalId = term.ExternalId,
                Name = term.Name,
                Terms = ToCreateModel(term.Terms)
            });
        }

        return result;
    }

}
