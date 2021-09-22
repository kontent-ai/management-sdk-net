using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models.Collections;
using Kentico.Kontent.Management.Models.Collections.Patch;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Collections")]
    public class ClollectionTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public ClollectionTests(ITestOutputHelper output)
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

        public async Task ListCollections_ListsCollections()
        {
            var response = await _client.ListCollectionsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.Collections.Where(x => x.Codename == EXISTING_COLLECTION_CODENAME));
        }

        [Fact]
        public async void ModifyCollection_Remove_ById_RemovesCollection()
        {
            var newCollection = await CreateCollection();

            var identifier = Reference.ById(newCollection.Id);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            var exception = await Record.ExceptionAsync(async () => await _client.ModifyCollectionAsync(changes));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.ModifyCollectionAsync(changes));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void ModifyCollection_Remove_ByCodename_RemovesCollection()
        {
            var newCollection = await CreateCollection();

            var identifier = Reference.ByCodename(newCollection.Codename);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            var exception = await Record.ExceptionAsync(async () => await _client.ModifyCollectionAsync(changes));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.ModifyCollectionAsync(changes));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void ModifyCollection_Remove_ByExternalId_RemovesCollection()
        {
            var newCollection = await CreateCollection();

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            var exception = await Record.ExceptionAsync(async () => await _client.ModifyCollectionAsync(changes));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.ModifyCollectionAsync(changes));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void ModifyCollection_Move_After_MovesCollection()
        {
            var newCollection = await CreateCollection();

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionMovePatchModel
            {
                Reference = identifier,
                After = Reference.ById(Guid.Empty)
            }};

            var collections = await _client.ModifyCollectionAsync(changes);

            //check that new collection is second it the list - index 1
            Assert.Equal(newCollection.Codename, collections.Collections.ToList()[1].Codename);

            //clean up
            await RemoveCollection(newCollection.ExternalId);
        }

        [Fact]
        public async void ModifyCollection_Move_Before_MovesCollection()
        {
            var newCollection = await CreateCollection();

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionMovePatchModel
            {
                Reference = identifier,
                Before = Reference.ById(Guid.Empty)
            }};

            var collections = await _client.ModifyCollectionAsync(changes);

            //check that new collection is second it the list - index 0
            Assert.Equal(newCollection.Codename, collections.Collections.ToList()[0].Codename);

            //clean up
            await RemoveCollection(newCollection.ExternalId);
        }

        [Fact]
        public async void ModifyCollection_AddInto_MovesCollection()
        {
            var codename = "new_collection";

            var expected = new CollectionCreateModel
            {
                Codename = codename,
                ExternalId = "new_collection_external_id",
                Name = "new_collection_name"
            };

            var change = new CollectionAddIntoPatchModel { Value = expected };

            var collections = await _client.ModifyCollectionAsync(new[] { change });

            var newCollection = collections.Collections.FirstOrDefault(x => x.Codename == codename);

            Assert.Equal(expected.Codename, newCollection.Codename);
            Assert.Equal(expected.ExternalId, newCollection.ExternalId);
            Assert.Equal(expected.Name, newCollection.Name);

            //clean up
            await RemoveCollection(newCollection.ExternalId);
        }

        [Fact]
        public async void ModifyCollection_Replace_ReplacesCollection()
        {
            var newCollection = await CreateCollection();

            var identifier = Reference.ByExternalId(newCollection.ExternalId);

            var changes = new[] { new CollectionReplacePatchModel
            {
                Reference = identifier,
                PropertyName = PropertyName.Name,
                Value = "newName"
            }};

            var collections = await _client.ModifyCollectionAsync(changes);

            var modifiedCollection = collections.Collections.FirstOrDefault(x => x.ExternalId == newCollection.ExternalId);

            Assert.Equal("newName", modifiedCollection.Name);

            //clean up
            await RemoveCollection(newCollection.ExternalId);
        }

        private async Task<CollectionModel> CreateCollection([CallerMemberName] string memberName = "")
        {
            var suffix = $"{memberName.ToLower().Substring(0, 40)}_{memberName.ToLower().Substring(40, Math.Min(memberName.Length - 40, 9))}";

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

            return (await _client.ModifyCollectionAsync(new[] { change })).Collections.FirstOrDefault(x => x.ExternalId == externalId);
        }

        private async Task RemoveCollection(string externalId)
        {
            var identifier = Reference.ByExternalId(externalId);

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            await _client.ModifyCollectionAsync(changes);
        }
    }
}
