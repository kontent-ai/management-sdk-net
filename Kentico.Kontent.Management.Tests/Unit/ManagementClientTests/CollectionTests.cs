using FluentAssertions;
using Kentico.Kontent.Management.Models.Collections;
using Kentico.Kontent.Management.Models.Collections.Patch;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class CollectionTests : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public CollectionTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("Collection");
        }

        [Fact]
        public async Task ListCollections_ListsCollections()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

            var expectedItems = _fileSystemFixture.GetExpectedResponse<CollectionsModel>("Collections.json");

            var response = await client.ListCollectionsAsync();

            response.Should().BeEquivalentTo(expectedItems);
        }

        
        [Fact]
        public async void ModifyCollection_Remove_ById_RemovesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResposne();

            var identifier = Reference.ById(Guid.NewGuid());

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            Func<Task> deleteCollection = async () => await client.ModifyCollectionAsync(changes);

            await deleteCollection.Should().NotThrowAsync();
        }

        
        [Fact]
        public async void ModifyCollection_Remove_ByCodename_RemovesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResposne();

            var identifier = Reference.ByCodename("codename");

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            Func<Task> deleteCollection = async () => await client.ModifyCollectionAsync(changes);

            await deleteCollection.Should().NotThrowAsync();
        }

        [Fact]
        public async void ModifyCollection_Remove_ByExternalId_RemovesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResposne();

            var identifier = Reference.ByExternalId("externalId");

            var changes = new[] { new CollectionRemovePatchModel
            {
                CollectionIdentifier = identifier
            }};

            Func<Task> deleteCollection = async () => await client.ModifyCollectionAsync(changes);

            await deleteCollection.Should().NotThrowAsync();
        }
        
        [Fact]
        public async void ModifyCollection_Move_After_MovesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

            var expectedItems = _fileSystemFixture.GetExpectedResponse<CollectionsModel>("Collections.json");

            var identifier = Reference.ByExternalId("second_external_id");

            var changes = new[] { new CollectionMovePatchModel
            {
                Reference = identifier,
                After = Reference.ById(Guid.Empty)
            }};

            var response = await client.ModifyCollectionAsync(changes);

            response.Should().BeEquivalentTo(expectedItems, options => options.WithStrictOrderingFor(x => x.Collections));
        }
        
        [Fact]
        public async void ModifyCollection_Move_Before_MovesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

            var expectedItems = _fileSystemFixture.GetExpectedResponse<CollectionsModel>("Collections.json");

            var identifier = Reference.ByExternalId("second_external_id");

            var changes = new[] { new CollectionMovePatchModel
            {
                Reference = identifier,
                Before = Reference.ByExternalId("thrid_external_id")
            }};

            var response = await client.ModifyCollectionAsync(changes);

            response.Should().BeEquivalentTo(expectedItems, options => options.WithStrictOrderingFor(x => x.Collections));
        }
        
        [Fact]
        public async void ModifyCollection_AddInto_MovesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

            var expected = new CollectionCreateModel
            {
                Codename = "second_collection",
                ExternalId = "second_external_id",
                Name = "Second collection"
            };

            var change = new CollectionAddIntoPatchModel { Value = expected , After = Reference.ById(Guid.Empty) };

            var response = await client.ModifyCollectionAsync(new[] { change });

            response.Collections.Should().ContainEquivalentOf(expected);
        }
        
        [Fact]
        public async void ModifyCollection_Replace_ReplacesCollection()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Collections.json");

            var identifier = Reference.ByExternalId("second_external_id");

            var changes = new[] { new CollectionReplacePatchModel
            {
                Reference = identifier,
                PropertyName = PropertyName.Name,
                Value = "Second collection"
            }};

            var response = await client.ModifyCollectionAsync(changes);

            response.Collections.Should().ContainSingle(x => x.Name == "Second collection");
        }
    }
}
