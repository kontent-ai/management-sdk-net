using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models.Collections;
using Kentico.Kontent.Management.Models.Collections.Patch;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Collections")]
        public async Task ListCollections_ListsCollections()
        {
            var client = CreateManagementClient();

            var response = await client.ListCollectionsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.Collections.Where(x => x.Codename == EXISTING_COLLECTION_CODENAME));
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_Remove_ById_RemovesCollection()
        {
            var client = CreateManagementClient();

            var newCollection = await CreateCollection(client);

            var identifier = Reference.ById(newCollection.Id);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            var exception = await Record.ExceptionAsync(async () => await client.ModifyCollectionAsync(changes));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.ModifyCollectionAsync(changes));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_Remove_ByCodename_RemovesCollection()
        {
            var client = CreateManagementClient();

            var newCollection = await CreateCollection(client);

            var identifier = Reference.ByCodename(newCollection.Codename);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            var exception = await Record.ExceptionAsync(async () => await client.ModifyCollectionAsync(changes));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.ModifyCollectionAsync(changes));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_Remove_ByExternalId_RemovesCollection()
        {
            var client = CreateManagementClient();

            var newCollection = await CreateCollection(client);

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            var exception = await Record.ExceptionAsync(async () => await client.ModifyCollectionAsync(changes));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.ModifyCollectionAsync(changes));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_Move_After_MovesCollection()
        {
            var client = CreateManagementClient();

            var newCollection = await CreateCollection(client);

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionMovePatchModel
            {
                CollectionIdentifier = identifier,
                After = Reference.ById(Guid.Empty)
            }};

            var collections = await client.ModifyCollectionAsync(changes);

            //check that new collection is second it the list - index 1
            Assert.Equal(newCollection.Codename, collections.Collections.ToList()[1].Codename);

            //clean up
            await RemoveCollection(client, newCollection.ExternalId);
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_Move_Before_MovesCollection()
        {
            var client = CreateManagementClient();

            var newCollection = await CreateCollection(client);

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionMovePatchModel
            {
                CollectionIdentifier = identifier,
                Before = Reference.ById(Guid.Empty)
            }};

            var collections = await client.ModifyCollectionAsync(changes);

            //check that new collection is second it the list - index 0
            Assert.Equal(newCollection.Codename, collections.Collections.ToList()[0].Codename);

            //clean up
            await RemoveCollection(client, newCollection.ExternalId);
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_AddInto_MovesCollection()
        {
            var client = CreateManagementClient();

            var codename = "new_collection";

            var expected = new CollectionCreateModel
            {
                Codename = codename,
                ExternalId = "new_collection_external_id",
                Name = "new_collection_name"
            };

            var change = new CollectionAddIntoPatchModel { Value = expected };

            var collections = await client.ModifyCollectionAsync(new[] { change });

            var newCollection = collections.Collections.FirstOrDefault(x => x.Codename == codename);

            Assert.Equal(expected.Codename, newCollection.Codename);
            Assert.Equal(expected.ExternalId, newCollection.ExternalId);
            Assert.Equal(expected.Name, newCollection.Name);

            //clean up
            await RemoveCollection(client, newCollection.ExternalId);
        }

        [Fact]
        [Trait("Category", "Collections")]
        public async void ModifyCollection_Replace_ReplacesCollection()
        {
            var client = CreateManagementClient();

            var newCollection = await CreateCollection(client);

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionReplacePatchModel
            {
                CollectionIdentifier = identifier,
                PropertyName = PropertyName.Name,
                Value = "newName"
            }};

            var collections = await client.ModifyCollectionAsync(changes);

            var modifiedCollection = collections.Collections.FirstOrDefault(x => x.ExternalId == newCollection.ExternalId);

            Assert.Equal("newName", modifiedCollection.Name);

            //clean up
            await RemoveCollection(client, newCollection.ExternalId);
        }

        private async Task<CollectionModel> CreateCollection(ManagementClient client, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var suffix = $"{memberName.ToLower().Substring(0, 40)}_{sourceLineNumber:d}";

            var externalId = $"eid_{suffix}";

            var change = new CollectionAddIntoPatchModel
            {
                Value = new CollectionCreateModel
                {
                    Codename = $"c_{suffix}",
                    ExternalId = externalId,
                    Name = suffix
                },
                After = Reference.ByCodename(EXISTING_COLLECTION_CODENAME)
            };

            return (await client.ModifyCollectionAsync(new[] { change })).Collections.FirstOrDefault(x => x.ExternalId == externalId);
        }

        private async Task RemoveCollection(ManagementClient client, string externalId)
        {
            var identifier = Reference.ByExternalId(externalId);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            await client.ModifyCollectionAsync(changes);
        }
    }
}
