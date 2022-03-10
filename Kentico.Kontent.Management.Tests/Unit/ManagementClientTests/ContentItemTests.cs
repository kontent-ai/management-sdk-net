using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Kentico.Kontent.Management.Extensions;
using Xunit;
using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using System;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class ContentItemTests : IClassFixture<FileSystemFixture>
    {
        private readonly FileSystemFixture _fileSystemFixture;

        public ContentItemTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("ContentItem");
        }

        [Fact]
        public async void ListContentItemsAsync_WithContinuation_ListsContentItems()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItemsPage1.json", "ContentItemsPage2.json", "ContentItemsPage3.json");

            var expectedItems = _fileSystemFixture.GetItemsOfExpectedListingResponse<ContentItemModel>("ContentItemsPage1.json", "ContentItemsPage2.json", "ContentItemsPage3.json");

            var response = await client.ListContentItemsAsync().GetAllAsync();

            response.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async void GetContentItemAsync_GetsContentItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItem.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentItemModel>("ContentItem.json");

            var identifier = Reference.ByCodename("ciao_codename");

            var response = await client.GetContentItemAsync(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetContentItemAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => x.GetContentItemAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CreateContentItemAsync_CreatesContentItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItem.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentItemModel>("ContentItem.json");

            var createModel = new ContentItemCreateModel
            {
                Codename = expected.Codename,
                Collection = expected.Collection,
                ExternalId = expected.ExternalId,
                Name = expected.Name,
                Type = expected.Type
            };

            var response = await client.CreateContentItemAsync(createModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void CreateContentItemAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => x.CreateContentItemAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void UpsertContentItemAsync_UpsertsContentItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentItem.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentItemModel>("ContentItem.json");

            var identifier = Reference.ByCodename("codename");

            var upsertModel = new ContentItemUpsertModel
            {
                Codename = expected.Codename,
                Collection = expected.Collection,
                Name = expected.Name,
                Type = expected.Type
            };

            var response = await client.UpsertContentItemAsync(identifier, upsertModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void UpsertContentItemAsyncc_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var upsertModel = new ContentItemUpsertModel
            {
                Codename = "codename",
                Collection = Reference.ById(Guid.Empty),
                Name = "name",
                Type = Reference.ByCodename("type_codename")
            };

            await client.Invoking(x => client.UpsertContentItemAsync(null, upsertModel))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void UpsertContentItemAsyncc_UpsertModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var identifier = Reference.ByCodename("codename");

            await client.Invoking(x => client.UpsertContentItemAsync(identifier, null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void DeleteContentItemAsync_DeletesContentItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var identifier = Reference.ByCodename("codename");

            await client.Invoking(x => client.DeleteContentItemAsync(identifier))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async void DeleteContentItemAsync_IdentifierIsNull_DeletesContentItem()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => client.DeleteContentItemAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}
