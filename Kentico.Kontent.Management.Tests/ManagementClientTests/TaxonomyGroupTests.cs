using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void ListTaxonomyGroups_ListsTaxonomyGroups()
        {
            var client = CreateManagementClient();

            var response = await client.ListTaxonomyGroupsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Codename == EXISTING_TAXONOMY_GROUP_CODENAME));
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void GetTaxonomyGroup_ById_GetsTaxonomyGroup()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ById(EXISTING_TAXONOMY_GROUP_ID);

            var response = await client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(EXISTING_TAXONOMY_GROUP_ID, response.Id);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void GetTaxonomyGroup_ByCodename_GetsTaxonomyGroup()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ByCodename(EXISTING_TAXONOMY_GROUP_CODENAME);

            var response = await client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(EXISTING_TAXONOMY_GROUP_CODENAME, response.Codename);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void GetTaxonomyGroup_ByExternalId_GetsTaxonomyGroup()
        {
            var externalid = "4ce421e9-c403-eee8-fdc2-74f09392a749";
            var client = CreateManagementClient();

            var identifier = Reference.ByExternalId(externalid);

            var response = await client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(externalid, response.ExternalId);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void CreateTaxonomyGroup_CreatesTaxonomyGroup()
        {
            var client = CreateManagementClient();

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesCreate!",
                Codename = "taxonomies_codename_create",
                ExternalId = "taxonomies_codename_external_id__create",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "taxonomies_term_codename",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            var responseGroup = await client.CreateTaxonomyGroupAsync(group);

            Assert.Equal(group.Name, responseGroup.Name);
            Assert.Equal(group.Codename, responseGroup.Codename);
            Assert.Equal(group.ExternalId, responseGroup.ExternalId);
            Assert.Single(responseGroup.Terms);

            // Cleanup
            var groupToClean = Reference.ByCodename(group.Codename);
            await client.DeleteTaxonomyGroupAsync(groupToClean);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void DeleteTaxonomyGroup_ByCodename_DeletesTaxonomyGroup()
        {
            var client = CreateManagementClient();

            var responseGroup = await CreateTaxonomyGroup(client);

            var identifier = Reference.ByCodename(responseGroup.Codename);


            var exception = await Record.ExceptionAsync(async () => await client.DeleteTaxonomyGroupAsync(identifier));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteTaxonomyGroupAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void DeleteTaxonomyGroup_ById_DeletesTaxonomyGroup()
        {
            var client = CreateManagementClient();

            var responseGroup = await CreateTaxonomyGroup(client);

            var identifier = Reference.ById(responseGroup.Id);


            var exception = await Record.ExceptionAsync(async () => await client.DeleteTaxonomyGroupAsync(identifier));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteTaxonomyGroupAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void DeleteTaxonomyGroup_ByExternalId_DeletesTaxonomyGroup()
        {
            var client = CreateManagementClient();

            var responseGroup = await CreateTaxonomyGroup(client);

            var identifier = Reference.ByExternalId(responseGroup.ExternalId);

            var exception = await Record.ExceptionAsync(async () => await client.DeleteTaxonomyGroupAsync(identifier));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteTaxonomyGroupAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void ModifyTaxonomyGroup_AddInto_ModifiesTaxonomyGroup()
        {
            //Arrange
            var client = CreateManagementClient();

            var group = await CreateTaxonomyGroup(client);

            var termName = "New taxonomy term";
            var changes = new TaxonomyGroupAddIntoPatchModel
            {
                Value = new TaxonomyGroupCreateModel
                {
                    Name = termName,
                    ExternalId = "my_new_term_addinto",
                    Terms = new TaxonomyGroupCreateModel[0]
                }
            };

            //act
            var modifiedType = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupAddIntoPatchModel> { changes });

            //assert
            Assert.Equal(3, modifiedType.Terms.Count());
            Assert.Single(modifiedType.Terms.Where(x => x.Name == termName));

            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await client.DeleteTaxonomyGroupAsync(groupClean);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void ModifyTaxonomyGroup_Replace_ModifiesTaxonomyGroup()
        {
            //Arrange
            var client = CreateManagementClient();

            var group = await CreateTaxonomyGroup(client);

            var termName = "New taxonomy term name";
            var changes = new TaxonomyGroupReplacePatchModel
            {
                PropertyName = PropertyName.Terms,
                Reference = Reference.ByCodename(group.Codename),
                Value = new List<TaxonomyGroupCreateModel> {
                    new TaxonomyGroupCreateModel
                    {
                        Name = termName,
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            //act
            var modifiedType = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupReplacePatchModel> { changes });

            //assert
            Assert.Single(modifiedType.Terms.Where(x => x.Name == termName));

            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await client.DeleteTaxonomyGroupAsync(groupClean);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void ModifyTaxonomyGroup_Remove_ModifiesTaxonomyGroup()
        {
            //Arrange
            var client = CreateManagementClient();

            var group = await CreateTaxonomyGroup(client);

            var changes = new TaxonomyGroupRemovePatchModel
            {
                Reference = Reference.ByCodename(group.Terms.First().Codename),
            };

            //act
            var modifiedType = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupRemovePatchModel> { changes });

            //assert
            Assert.Single(modifiedType.Terms);

            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await client.DeleteTaxonomyGroupAsync(groupClean);
        }

        private async Task<TaxonomyGroupModel> CreateTaxonomyGroup(ManagementClient client, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var suffix = $"{memberName.ToLower().Substring(0, 40)}_{sourceLineNumber:d}";

            var group = new TaxonomyGroupCreateModel
            {
                Name = $"{suffix}",
                Codename = $"c_{suffix}",
                ExternalId = $"eid_{suffix}",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = $"t_c_1{suffix}",
                        Name = $"name1_{suffix}",
                        ExternalId = $"eid1_{suffix}",
                        Terms = new TaxonomyGroupCreateModel[0]
                    },
                    new TaxonomyGroupCreateModel
                    {
                        Codename = $"t_c_2{suffix}",
                        Name = $"name2_{suffix}",
                        ExternalId = $"eid2_{suffix}",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            return await client.CreateTaxonomyGroupAsync(group);
        }
    }
}
