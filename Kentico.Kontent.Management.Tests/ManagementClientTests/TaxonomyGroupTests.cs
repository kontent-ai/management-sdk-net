using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "TaxonomyGroups")]
    public class TaxonomyGroupTests
    {
        private readonly IManagementClient _client;
        private readonly Scenario _scenario;

        public TaxonomyGroupTests(ITestOutputHelper output)
        {
            //this magic can be replace once new xunit is delivered
            //https://github.com/xunit/xunit/issues/621
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);

            _scenario = new Scenario(test.TestCase.TestMethod.Method.Name);
            _client = _scenario.Client;
        }

        [Fact]
        public async void ListTaxonomyGroups_ListsTaxonomyGroups()
        {
            var response = await _client.ListTaxonomyGroupsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Codename == EXISTING_TAXONOMY_GROUP_CODENAME));
        }

        [Fact]
        public async void GetTaxonomyGroup_ById_GetsTaxonomyGroup()
        {
            var identifier = Reference.ById(EXISTING_TAXONOMY_GROUP_ID);

            var response = await _client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(EXISTING_TAXONOMY_GROUP_ID, response.Id);
        }

        [Fact]
        public async void GetTaxonomyGroup_ByCodename_GetsTaxonomyGroup()
        {
            var identifier = Reference.ByCodename(EXISTING_TAXONOMY_GROUP_CODENAME);

            var response = await _client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(EXISTING_TAXONOMY_GROUP_CODENAME, response.Codename);
        }

        [Fact]
        public async void GetTaxonomyGroup_ByExternalId_GetsTaxonomyGroup()
        {
            var externalid = "4ce421e9-c403-eee8-fdc2-74f09392a749";
            

            var identifier = Reference.ByExternalId(externalid);

            var response = await _client.GetTaxonomyGroupAsync(identifier);
            Assert.Equal(externalid, response.ExternalId);
        }

        [Fact]
        public async void CreateTaxonomyGroup_CreatesTaxonomyGroup()
        {
            var group = new TaxonomyGroupCreateModel
            {
                Name = "taxonomiesCreate!",
                Codename = "taxonomies_codename_create",
                ExternalId = "taxonomies_codename_external_id__create",
                Terms = new List<TaxonomyTermCreateModel>
                {
                    new TaxonomyTermCreateModel
                    {
                        Codename = "taxonomies_term_codename",
                        Name = "name",
                        ExternalId = "taxonomies_term_external_id",
                        Terms = Array.Empty<TaxonomyTermCreateModel>()
                    }
                }
            };

            var responseGroup = await _client.CreateTaxonomyGroupAsync(group);

            Assert.Equal(group.Name, responseGroup.Name);
            Assert.Equal(group.Codename, responseGroup.Codename);
            Assert.Equal(group.ExternalId, responseGroup.ExternalId);
            Assert.Single(responseGroup.Terms);

            // Cleanup
            var groupToClean = Reference.ByCodename(group.Codename);
            await _client.DeleteTaxonomyGroupAsync(groupToClean);
        }

        [Fact]
        public async void DeleteTaxonomyGroup_ByCodename_DeletesTaxonomyGroup()
        {
            var responseGroup = await CreateTaxonomyGroup();

            var identifier = Reference.ByCodename(responseGroup.Codename);


            var exception = await Record.ExceptionAsync(async () => await _client.DeleteTaxonomyGroupAsync(identifier));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteTaxonomyGroupAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteTaxonomyGroup_ById_DeletesTaxonomyGroup()
        {
            var responseGroup = await CreateTaxonomyGroup();

            var identifier = Reference.ById(responseGroup.Id);


            var exception = await Record.ExceptionAsync(async () => await _client.DeleteTaxonomyGroupAsync(identifier));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteTaxonomyGroupAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteTaxonomyGroup_ByExternalId_DeletesTaxonomyGroup()
        {
            var responseGroup = await CreateTaxonomyGroup();

            var identifier = Reference.ByExternalId(responseGroup.ExternalId);

            var exception = await Record.ExceptionAsync(async () => await _client.DeleteTaxonomyGroupAsync(identifier));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteTaxonomyGroupAsync(identifier));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void ModifyTaxonomyGroup_AddInto_ModifiesTaxonomyGroup()
        {
            //Arrange
            var group = await CreateTaxonomyGroup();

            var termName = "New taxonomy term";
            var changes = new TaxonomyGroupAddIntoPatchModel
            {
                Value = new TaxonomyTermCreateModel
                {
                    Name = termName,
                    ExternalId = "my_new_term_addinto",
                    Terms = Array.Empty<TaxonomyTermCreateModel>()
                }
            };

            //act
            var modifiedType = await _client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupAddIntoPatchModel> { changes });

            //assert
            Assert.Equal(3, modifiedType.Terms.Count());
            Assert.Single(modifiedType.Terms.Where(x => x.Name == termName));

            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await _client.DeleteTaxonomyGroupAsync(groupClean);
        }

        [Fact]
        public async void ModifyTaxonomyGroup_Replace_ModifiesTaxonomyGroup()
        {
            //Arrange
            var group = await CreateTaxonomyGroup();

            var termName = "New taxonomy term name";
            var changes = new TaxonomyGroupReplacePatchModel
            {
                PropertyName = PropertyName.Terms,
                Reference = Reference.ByCodename(group.Codename),
                Value = new List<TaxonomyGroupCreateModel> {
                    new TaxonomyGroupCreateModel
                    {
                        Name = termName,
                        Terms = Array.Empty<TaxonomyTermCreateModel>()
                    }
                }
            };

            //act
            var modifiedType = await _client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupReplacePatchModel> { changes });

            //assert
            Assert.Single(modifiedType.Terms.Where(x => x.Name == termName));

            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await _client.DeleteTaxonomyGroupAsync(groupClean);
        }

        [Fact]
        public async void ModifyTaxonomyGroup_Remove_ModifiesTaxonomyGroup()
        {
            //Arrange
            var group = await CreateTaxonomyGroup();

            var changes = new TaxonomyGroupRemovePatchModel
            {
                Reference = Reference.ByCodename(group.Terms.First().Codename),
            };

            //act
            var modifiedType = await _client.ModifyTaxonomyGroupAsync(Reference.ByCodename(group.Codename), new List<TaxonomyGroupRemovePatchModel> { changes });

            //assert
            Assert.Single(modifiedType.Terms);

            // Cleanup
            var groupClean = Reference.ByCodename(group.Codename);
            await _client.DeleteTaxonomyGroupAsync(groupClean);
        }

        private async Task<TaxonomyGroupModel> CreateTaxonomyGroup([CallerMemberName] string memberName = "")
        {
            var suffix = $"{memberName.ToLower().Substring(0, 40)}_{memberName.ToLower().Substring(40, Math.Min(memberName.Length - 40, 9))}";

            var group = new TaxonomyGroupCreateModel
            {
                Name = $"{suffix}",
                Codename = $"c_{suffix}",
                ExternalId = $"eid_{suffix}",
                Terms = new List<TaxonomyTermCreateModel>
                {
                    new TaxonomyTermCreateModel
                    {
                        Codename = $"t_c_1{suffix}",
                        Name = $"name1_{suffix}",
                        ExternalId = $"eid1_{suffix}",
                        Terms = Array.Empty<TaxonomyTermCreateModel>()
                    },
                    new TaxonomyTermCreateModel
                    {
                        Codename = $"t_c_2{suffix}",
                        Name = $"name2_{suffix}",
                        ExternalId = $"eid2_{suffix}",
                        Terms = Array.Empty<TaxonomyTermCreateModel>()
                    }
                }
            };

            return await _client.CreateTaxonomyGroupAsync(group);
        }
    }
}
