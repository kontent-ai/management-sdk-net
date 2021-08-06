using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void ListTaxonomyGroups_ListsTaxonomyGroups()
        {
            var client = CreateManagementClient(nameof(ListTaxonomyGroups_ListsTaxonomyGroups));

            var response = await client.ListTaxonomyGroupsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Codename == EXISTING_TAXONOMY_GROUP_CODENAME));
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void GetTaxonomyGroup_ById_GetsTaxonomyGroup()
        {
            var client = CreateManagementClient(nameof(GetTaxonomyGroup_ById_GetsTaxonomyGroup));

            var identifier = Reference.ById(EXISTING_TAXONOMY_GROUP_ID);

            var response = await client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(EXISTING_TAXONOMY_GROUP_ID, response.Id);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void GetTaxonomyGroup_ByCodename_GetsTaxonomyGroup()
        {
            var client = CreateManagementClient(nameof(GetTaxonomyGroup_ByCodename_GetsTaxonomyGroup));

            var identifier = Reference.ByCodename(EXISTING_TAXONOMY_GROUP_CODENAME);

            var response = await client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(EXISTING_TAXONOMY_GROUP_CODENAME, response.Codename);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void GetTaxonomyGroup_ByExternalId_GetsTaxonomyGroup()
        {
            var externalid = "4ce421e9-c403-eee8-fdc2-74f09392a749";
            var client = CreateManagementClient(nameof(GetTaxonomyGroup_ByExternalId_GetsTaxonomyGroup));

            var identifier = Reference.ByExternalId(externalid);

            var response = await client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(externalid, response.ExternalId);
        }

        [Fact]
        [Trait("Category", "TaxonomyGroups")]
        public async void CreateTaxonomyGroup_CreatesTaxonomyGroup()
        {
            var client = CreateManagementClient(nameof(CreateContentType_CreatesContentType));

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
            var client = CreateManagementClient(nameof(DeleteTaxonomyGroup_ByCodename_DeletesTaxonomyGroup));

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesCreate!",
                Codename = "taxonomies_codename_create",
                ExternalId = "taxonomies_codename_external_id_deletecodename",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "taxonomies_term_codename_deletecodename",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id_deletecodename",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            var responseGroup = await client.CreateTaxonomyGroupAsync(group);


            var identifier = Reference.ByCodename(group.Codename);


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
            var client = CreateManagementClient(nameof(DeleteTaxonomyGroup_ById_DeletesTaxonomyGroup));

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesCreate!",
                Codename = "taxonomies_codename_create",
                ExternalId = "taxonomies_codename_external_id_deleteid",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "taxonomies_term_codename_deleteid",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id_deleteid",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            var responseGroup = await client.CreateTaxonomyGroupAsync(group);


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
            var client = CreateManagementClient(nameof(DeleteTaxonomyGroup_ByExternalId_DeletesTaxonomyGroup));

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesCreate!",
                Codename = "taxonomies_codename_create",
                ExternalId = "taxonomies_codename_external_id_deleteid",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "taxonomies_term_codename_deleteid",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id_deleteid",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            var responseGroup = await client.CreateTaxonomyGroupAsync(group);


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
            var client = CreateManagementClient(nameof(ModifyTaxonomyGroup_AddInto_ModifiesTaxonomyGroup));

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesAddInto!",
                Codename = "taxonomies_codename_addinto",
                ExternalId = "taxonomies_codename_external_id_addinto",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "taxonomies_term_codename_addinto",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id_addinto",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            _ = await client.CreateTaxonomyGroupAsync(group);

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
            Assert.Equal(2, modifiedType.Terms.Count());
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
            var client = CreateManagementClient(nameof(ModifyTaxonomyGroup_Replace_ModifiesTaxonomyGroup));

            var taxonomyTermCodename = "taxonomies_term_codename_replace";

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesReplace!",
                Codename = "taxonomies_codename_replace",
                ExternalId = "taxonomies_external_id_replace",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = taxonomyTermCodename,
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id_replace",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            _ = await client.CreateTaxonomyGroupAsync(group);

            var termName = "New taxonomy term name";
            var changes = new TaxonomyGroupPatchReplaceModel
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
            var modifiedType = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupPatchReplaceModel> { changes });


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
            var client = CreateManagementClient(nameof(ModifyTaxonomyGroup_Remove_ModifiesTaxonomyGroup));

            var taxonomyTermCodename = "taxonomies_term_codename_remove";

            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesRemove!",
                Codename = "taxonomies_codename_remove",
                ExternalId = "taxonomies_codename_external_id_remove",
                Terms = new List<TaxonomyGroupCreateModel>
                {
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "taxonomies_term_codename_remove",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id_remove",
                        Terms = new TaxonomyGroupCreateModel[0]
                    },
                    new TaxonomyGroupCreateModel
                    {
                        Codename = "second_term_codename",
                        Name = "name13254",
                        ExternalId = "taxonomies_term_external_id_remove2",
                        Terms = new TaxonomyGroupCreateModel[0]
                    }
                }
            };

            _ = await client.CreateTaxonomyGroupAsync(group);

            var changes = new TaxonomyGroupPatchRemoveModel
            {
                Reference = Reference.ByCodename(taxonomyTermCodename),
            };


            //act
            var modifiedType = await client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupPatchRemoveModel> { changes });


            //assert
            Assert.Single(modifiedType.Terms);


            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await client.DeleteTaxonomyGroupAsync(groupClean);
        }
    }
}
