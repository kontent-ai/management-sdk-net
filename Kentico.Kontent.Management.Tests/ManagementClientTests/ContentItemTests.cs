using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "ContentItem")]
        public async void CreateContentItem_CreatesContentItem()
        {
            var client = CreateManagementClient();

            var itemName = "Hooray!";
            var itemCodename = "hooray_codename";
            var type = Reference.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);
            var item = new ContentItemCreateModel
            {
                Codename = itemCodename,
                Name = itemName,
                Type = type
            };

            var responseItem = await client.CreateContentItemAsync(item);

            Assert.Equal(itemName, responseItem.Name);
            Assert.Equal(itemCodename, responseItem.Codename);
            Assert.Equal(EXISTING_CONTENT_TYPE_ID, responseItem.Type.Id);

            // Cleanup
            var itemToClean = Reference.ByCodename(itemCodename);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void ListContentItems_ListsContentItems()
        {
            var client = CreateManagementClient();

            var response = await client.ListContentItemsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault());
        }

        [Fact(Skip = "Pagination does not work properly")]
        [Trait("Category", "ContentItem")]
        public async void ListContentItems_WithContinuation_ListsAllContentItems()
        {
            var client = CreateManagementClient();

            var response = await client.ListContentItemsAsync();
            Assert.NotNull(response);

            while (true)
            {
                foreach (var item in response)
                {
                    Assert.NotNull(item);
                }

                if (!response.HasNextPage())
                {
                    break;
                }
                response = await response.GetNextPage();
                Assert.NotNull(response);
            }
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void UpdateContentItem_ByCodename_UpdatesContentItem()
        {
            var client = CreateManagementClient();

            var itemName = "Hooray!";
            var identifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);

            // Set codename, name and collection
            var item = new ContentItemUpdateModel
            {
                Codename = EXISTING_ITEM_CODENAME,
                Name = itemName,
                Collection = NoExternalIdIdentifier.ById(Guid.Empty)
            };

            var responseItem = await client.UpdateContentItemAsync(identifier, item);

            Assert.Equal(itemName, responseItem.Name);
            Assert.Equal(EXISTING_ITEM_CODENAME, responseItem.Codename);
            Assert.Equal(NoExternalIdIdentifier.ById(Guid.Empty).Id, responseItem.Collection.Id);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void UpdateContentItem_ById_UpdatesContentItem()
        {
            var client = CreateManagementClient();

            var itemName = "Ciao!";
            var itemCodename = "ciao_codename";
            var identifier = Reference.ById(EXISTING_ITEM_ID2);

            var item = new ContentItemUpdateModel
            {
                Codename = itemCodename,
                Name = itemName
            };

            var responseItem = await client.UpdateContentItemAsync(identifier, item);

            Assert.Equal(itemName, responseItem.Name);
            Assert.Equal(itemCodename, responseItem.Codename);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void UpdateContentItemName_CodenameNotSet_RegeneratesCodenameByName()
        {
            var client = CreateManagementClient();

            var itemName = "regenerated_codename";
            var identifier = Reference.ById(EXISTING_ITEM_ID2);

            var item = new ContentItemUpdateModel
            {
                Name = itemName,
            };

            var responseItem = await client.UpdateContentItemAsync(identifier, item);

            Assert.Equal(itemName, responseItem.Name);
            // TODO validate why this have been implemented KCL-3078 https://github.com/Kentico/kontent-management-sdk-net/commit/9d9e6c286c622921da8e638e80d4ca9b7de67ed1
            // Assert.Equal(itemName, responseItem.CodeName);
        }

        [Fact(Skip = "Kentico.Kontent.Management.Exceptions.ManagementException : The request was not processed because the specified object has been modified by another request.")]
        [Trait("Category", "ContentItem")]
        public async void UpdateContentItem_UsingResponseModel_UpdatesContentItem()
        {
            var client = CreateManagementClient();

            // Arrange
            var externalId = "093afb41b0614a908c8734d2bb840210";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            preparedItem.Name = "EditedItem";
            var identifier = Reference.ByExternalId(externalId);
            var item = client.UpdateContentItemAsync(identifier, preparedItem);

            var contentItemResponse = await client.UpdateContentItemAsync(identifier, preparedItem);
            Assert.Equal("EditedItem", contentItemResponse.Name);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void UpsertContentItemByExternalId_UpdatesContentItem()
        {
            var client = CreateManagementClient();

            // Arrange
            var externalId = "753f6e965f4d49e5a120ca9a23551b10";
            var itemName = "Aloha!";
            var itemCodename = "aloha_codename";
            await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var type = Reference.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);
            var item = new ContentItemUpsertModel()
            {
                Codename = itemCodename,
                Name = itemName,
                Type = type,
                Collection = NoExternalIdIdentifier.ById(Guid.Empty)
            };

            var contentItemResponse = await client.UpsertContentItemByExternalIdAsync(externalId, item);
            Assert.Equal(itemName, contentItemResponse.Name);
            Assert.Equal(itemCodename, contentItemResponse.Codename);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void UpsertContentItemByExternalId_CreatesContentItem()
        {
            var client = CreateManagementClient();

            // Test
            var externalId = "9d98959eeac446288992b44b5d366e16";
            var itemName = "Hi!";
            var itemCodename = "hi_codename";
            var type = Reference.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);
            var item = new ContentItemUpsertModel()
            {
                Codename = itemCodename,
                Name = itemName,
                Type = type,
                Collection = NoExternalIdIdentifier.ById(Guid.Empty)
            };

            var contentItemResponse = await client.UpsertContentItemByExternalIdAsync(externalId, item);
            Assert.Equal(itemName, contentItemResponse.Name);
            Assert.Equal(externalId, contentItemResponse.ExternalId);
            Assert.Equal(itemCodename, contentItemResponse.Codename);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void GetContentItem_ById_GetsContentItem()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ById(EXISTING_ITEM_ID);

            var contentItemResponse = await client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID, contentItemResponse.Id);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void GetContentItem_ByCodename_GetsContentItem()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);

            var contentItemResponse = await client.GetContentItemAsync(identifier);
            Assert.Equal(EXISTING_ITEM_ID, contentItemResponse.Id);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void GetContentItem_ByExternalId_GetsContentItem()
        {
            var client = CreateManagementClient();

            // Arrange
            var externalId = "e5a8de5b584f4182b879c78b696dff09";
            await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var identifier = Reference.ByExternalId(externalId);

            var contentItemResponse = await client.GetContentItemAsync(identifier);
            Assert.Equal(externalId, contentItemResponse.ExternalId);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void DeleteContentItem_ById_DeletesContentItem()
        {
            var client = CreateManagementClient();

            var itemToDelete = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME);

            var identifier = Reference.ById(itemToDelete.Id);

            await client.DeleteContentItemAsync(identifier);

            // Check if not available after deletion
            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(() => client.GetContentItemAsync(identifier));
            }
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void DeleteContentItem_ByCodename_DeletesContentItem()
        {
            var client = CreateManagementClient();

            var itemToDelete = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME);

            var identifier = Reference.ByCodename(itemToDelete.Codename);

            await client.DeleteContentItemAsync(identifier);

            // Check if not available after deletion
            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(() => client.GetContentItemAsync(identifier));
            }
        }

        [Fact]
        [Trait("Category", "ContentItem")]
        public async void DeleteContentItem_ByExternalId_DeletesContentItem()
        {
            var client = CreateManagementClient();

            var externalId = "341bcf72988d49729ec34c8682710536";
            await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            var identifier = Reference.ByExternalId(externalId);

            await client.DeleteContentItemAsync(identifier);

            // Check if not available after deletion
            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(() => client.GetContentItemAsync(identifier));
            }
        }
    }
}
