using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Tests.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ById_LanguageId_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ById_LanguageId_UpdatesVariant));

            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ByCodename_LanguageId_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ByCodename_LanguageId_UpdatesVariant));

            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ById_LanguageCodename_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ById_LanguageCodename_UpdatesVariant));

            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ByCodename_LanguageCodename_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ByCodename_LanguageCodename_UpdatesVariant));

            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ByExternalId_LanguageCodename_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ByExternalId_LanguageCodename_UpdatesVariant));

            // Arrange
            var externalId = "fe2e8c24f0794f01b36807919602625d";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };
            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(responseVariant.Language.Id, EXISTING_LANGUAGE_ID);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ByExternalId_LanguageCodename_CreatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ByExternalId_LanguageCodename_CreatesVariant));

            // Arrange
            var externalId = "348052a5ad8c44ddac1e9683923d74a5";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };
            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ByExternalId_LanguageId_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ByExternalId_LanguageId_UpdatesVariant));

            // Arrange
            var externalId = "d5e050980baa43b085b909cdea4c6d2b";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_ByExternalId_LanguageId_CreatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_ByExternalId_LanguageId_CreatesVariant));

            // Arrange
            var externalId = "73e02811b05f429284006ea94c68c8f7";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariantUpsertModel);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_UsingResponseModel_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_UsingResponseModel_UpdatesVariant));

            // Arrange
            var externalId = "4357b71d21eb45369d54a635faf7672b";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var emptyElements = new List<object>();
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, emptyElements, preparedItem);

            // Test
            preparedVariant.Elements = _elements;
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariant: preparedVariant);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertVariant_UsingResponseModel_CreatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertVariant_UsingResponseModel_CreatesVariant));

            // Arrange
            var externalId = "5249f596a8be4d719bc9816e3d416d16";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var emptyElements = new List<object>();
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, emptyElements, preparedItem);

            // Test
            preparedVariant.Elements = _elements;
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(Guid.Empty);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, contentItemVariant: preparedVariant);

            Assert.Equal(Guid.Empty, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task ListContentItemVariants_ById_ListsVariants()
        {
            var client = CreateManagementClient(nameof(ListContentItemVariants_ById_ListsVariants));

            var identifier = Reference.ById(EXISTING_ITEM_ID);

            var responseVariants = await client.ListContentItemVariantsAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants.First().Item.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task ListContentItemVariants_ByCodename_ListsVariants()
        {
            var client = CreateManagementClient(nameof(ListContentItemVariants_ByCodename_ListsVariants));

            var identifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);

            var responseVariants = await client.ListContentItemVariantsAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants.First().Item.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task ListContentItemVariants_ByExternalId_ListsVariants()
        {
            var client = CreateManagementClient(nameof(ListContentItemVariants_ByExternalId_ListsVariants));

            // Arrange
            var externalId = "0220e6ec5b77401ea113b5273c8cdd5e";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var identifier = Reference.ByExternalId(externalId);
            var responseVariants = await client.ListContentItemVariantsAsync(identifier);

            Assert.Single(responseVariants);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetContentItemVariant_ById_LanguageId_GetsVariant()
        {
            var client = CreateManagementClient(nameof(GetContentItemVariant_ById_LanguageId_GetsVariant));

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetContentItemVariant_ById_LanguageCodename_GetsVariant()
        {
            var client = CreateManagementClient(nameof(GetContentItemVariant_ById_LanguageCodename_GetsVariant));

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetContentItemVariant_ByCodename_LanguageId_GetsVariant()
        {
            var client = CreateManagementClient(nameof(GetContentItemVariant_ByCodename_LanguageId_GetsVariant));

            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetContentItemVariant_ByCodename_LanguageCodename_GetsVariant()
        {
            var client = CreateManagementClient(nameof(GetContentItemVariant_ByCodename_LanguageCodename_GetsVariant));

            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetContentItemVariant_ByExternalId_LanguageCodename_GetsVariant()
        {
            var client = CreateManagementClient(nameof(GetContentItemVariant_ByExternalId_LanguageCodename_GetsVariant));

            // Arrange
            var externalId = "f9cfaa3e00f64e22a144fdacf4cba3e5";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(preparedItem.Id, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetContentItemVariant_ByExternalId_ReturnsVariant()
        {
            var client = CreateManagementClient(nameof(GetContentItemVariant_ByExternalId_ReturnsVariant));

            var externalId = "ad66f70ed9bb4b8694116c9119c4a930";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(preparedItem.Id, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task DeleteContentItemVariant_ById_LanguageCodename_DeletesVariant()
        {
            var client = CreateManagementClient(nameof(DeleteContentItemVariant_ById_LanguageCodename_DeletesVariant));

            var itemResponse = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = Reference.ById(itemResponse.Id);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task DeleteContentItemVariant_ById_LanguageId_DeletesVariant()
        {
            var client = CreateManagementClient(nameof(DeleteContentItemVariant_ById_LanguageId_DeletesVariant));

            var itemResponse = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = Reference.ById(itemResponse.Id);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task DeleteContentItemVariant_ByCodename_LanguageId_DeletesVariant()
        {
            var client = CreateManagementClient(nameof(DeleteContentItemVariant_ByCodename_LanguageId_DeletesVariant));

            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = Reference.ByCodename(itemResponse.Codename);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task DeleteContentItemVariant_ByCodename_LanguageCodename_DeletesVariant()
        {
            var client = CreateManagementClient(nameof(DeleteContentItemVariant_ByCodename_LanguageCodename_DeletesVariant));

            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = Reference.ByCodename(itemResponse.Codename);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task DeleteContentItemVariant_ByExternalId_LanguageId_DeletesVariant()
        {
            var client = CreateManagementClient(nameof(DeleteContentItemVariant_ByExternalId_LanguageId_DeletesVariant));

            var externalId = "90285b1a983c43299638c8a835f16b81";
            var itemResponse = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task DeleteContentItemVariant_ByExternalId_LanguageCodename_DeletesVariant()
        {
            var client = CreateManagementClient(nameof(DeleteContentItemVariant_ByExternalId_LanguageCodename_DeletesVariant));

            var externalId = "f4fe87222b6b46739bc673f6e5165c12";
            var itemResponse = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, itemResponse);

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.DeleteContentItemVariantAsync(identifier);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task ListStronglyTypedContentItemVariants_ById_ListsVariants()
        {
            var client = CreateManagementClient(nameof(ListStronglyTypedContentItemVariants_ById_ListsVariants));

            var identifier = Reference.ById(EXISTING_ITEM_ID);

            var responseVariants = await client.ListContentItemVariantsAsync<ComplexTestModel>(identifier);

            Assert.All(responseVariants, x =>
            {
                Assert.NotNull(x.Elements);
            });
            Assert.Equal(EXISTING_ITEM_ID, responseVariants.First().Item.Id);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task GetStronglyTypedContentItemVariantAsync_ById_LanguageId_GetVariant()
        {
            var client = CreateManagementClient(nameof(GetStronglyTypedContentItemVariantAsync_ById_LanguageId_GetVariant));

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetContentItemVariantAsync<ComplexTestModel>(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
            Assert.NotNull(response.Elements);
        }

        [Fact]
        [Trait("Category", "ContentItemVariant")]
        public async Task UpsertStronglyTypedContentItemVariantAsync_ById_LanguageId_UpdatesVariant()
        {
            var client = CreateManagementClient(nameof(UpsertStronglyTypedContentItemVariantAsync_ById_LanguageId_UpdatesVariant));

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(identifier, StronglyTypedElements);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            Assert.NotNull(responseVariant.Elements);
            AssertStronglyTypedResponseElements(responseVariant.Elements);
        }

        //don't make it static..static fields in c# in partial class are initialized in random order
        //so compiler might create _elements before it created EXISTING_ITEM_ID...
        private IReadOnlyList<dynamic> _elements = new object[]
        {
            new
            {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()
                },
                value = "On Roasts",
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                 element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()
                },
                value = new DateTime(2017, 7, 4),
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()
                },
                value = $@"
                        <h1>Light Roasts</h1>
                        <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p>
                        <object type=""application/kenticocloud"" data-type=""component"" data-id=""{RICH_TEXT_COMPONENT_ID}""></object>
                        ",
                components = new[]
                {
                    new
                    {
                        id = RICH_TEXT_COMPONENT_ID,
                        type = new {
                            id = TWEET_TYPE_ID
                        },
                        elements = new dynamic[]
                        {
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.TweetLink)).GetKontentElementId()
                                },
                                value =  "https://twitter.com/ChrastinaOndrej/status/1417105245935706123"
                            },
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.Theme)).GetKontentElementId()
                                },
                                value = new[] {
                                        // TODO - decide whether it is better to use ID instead of codename
                                        NoExternalIdIdentifier.ByCodename(TWEET_THEME_ELEMENT_DARK_OPTION_CODENAME)
                                }
                            },
                            new
                            {
                                element = new {
                                    id = typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId()
                                },
                                value = new[] {
                                    // TODO - decide whether it is better to use ID instead of codename
                                    NoExternalIdIdentifier.ByCodename(TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_CODENAME)
                                }
                            }
                        }
                    }
                },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()
                },
                value = new[] { Reference.ById(EXISTING_ITEM_ID) },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()
                },
                mode = "custom",
                value = "on-roasts",
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()
                },
                // TODO - decide whether it is better to use ID instead of codename
                value = new[] { NoExternalIdIdentifier.ByCodename(EXISTING_TAXONOMY_TERM_CODENAME) } ,
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()
                },
                value = new[]
                {
                    AssetIdentifier.ById(EXISTING_ASSET_ID),
                },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
            new {
                element = new {
                    id = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()
                },
                value = new[]
                {
                    // TODO - decide whether it is better to use ID instead of codename
                    NoExternalIdIdentifier.ByCodename(EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_PAID),
                    NoExternalIdIdentifier.ByCodename(EXISTING_MULTIPLE_CHOICE_OPTION_CODENAME_FEATURED)
                },
                codename = typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
            },
        };

        //don't make it static..static fields in c# in partial class are initialized in random order
        //so compiler might create _elements before it created EXISTING_ITEM_ID...
        private readonly ComplexTestModel StronglyTypedElements = new ComplexTestModel
        {
            Title = new TextElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()),
                Value = "On Roast"
            },
            Rating = new NumberElement
            {
                Value = 3.14m,
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Rating)).GetKontentElementId()),
            },
            SelectedForm = new CustomElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.SelectedForm)).GetKontentElementId()),
                Value = "{\"formId\": 42}"
            },
            PostDate = new DateTimeElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()),
                Value = new DateTime(2017, 7, 4)
            },
            BodyCopy = new RichTextElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()),
                Value = $"<h1>Light Roasts</h1> <p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{RICH_TEXT_COMPONENT_ID}\"></object>",
                Components = new ComponentModel[]
                {
                    new ComponentModel
                    {
                        Id = RICH_TEXT_COMPONENT_ID,
                        Type = Reference.ById(TWEET_TYPE_ID),
                        Elements = new BaseElement[]
                        {
                            // TODO use exact Tweet values like in _elements (unify IDs to constants)
                            new TextElement
                            {
                                Element = ObjectIdentifier.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.TweetLink)).GetKontentElementId()),
                                Value = "https://twitter.com/ChrastinaOndrej/status/1417105245935706123"
                            },
                            new MultipleChoiceElement
                            {
                                Element = ObjectIdentifier.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.Theme)).GetKontentElementId()),
                                Value = new[] { NoExternalIdIdentifier.ById(TWEET_THEME_ELEMENT_DARK_OPTION_ID) },
                            },
                            new MultipleChoiceElement
                            {
                                Element = ObjectIdentifier.ById(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId()),
                                Value = new[] { NoExternalIdIdentifier.ById(TWEET_DISPLAY_OPTIONS_HIDE_THREAD_OPTION_ID) },
                            }
                        }
                    }
                }
            },
            RelatedArticles = new LinkedItemsElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()),
                Value = new[] { Reference.ById(EXISTING_ITEM_ID) }
            },
            UrlPattern = new UrlSlugElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()),
                Value = "on-roasts",
                Mode = "custom"
            },
            Personas = new TaxonomyElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()),
                Value = new[] { NoExternalIdIdentifier.ByCodename(EXISTING_TAXONOMY_TERM_CODENAME) }
            },
            TeaserImage = new AssetElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()),
                Value = new[] { AssetIdentifier.ById(EXISTING_ASSET_ID) },
            },
            Options = new MultipleChoiceElement
            {
                Element = ObjectIdentifier.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()),
                Value = new[]
                {
                    NoExternalIdIdentifier.ById(EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID),
                    NoExternalIdIdentifier.ById(EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED)
                }
            },
        };

        private (dynamic expected, dynamic actual) GetElementByCodename(string codename, IEnumerable<dynamic> actualElements)
        {
            var expected = _elements.Single(x => x.codename == codename);
            var actual = actualElements.Single(x => x.element.id == expected.element.id);

            return (expected, actual);
        }

        private string UnifyWhitespace(string text)
        {
            return new Regex("\\s+", RegexOptions.Multiline).Replace(text, " ").Trim();
        }

        private void AssertResponseElements(ContentItemVariantModel responseVariant)
        {
            var (expected, actual) = GetElementByCodename("title", responseVariant.Elements);
            Assert.Equal(expected.value, actual.value);

            (expected, actual) = GetElementByCodename("post_date", responseVariant.Elements);
            Assert.Equal(expected.value, actual.value);

            (expected, actual) = GetElementByCodename("url_pattern", responseVariant.Elements);
            Assert.Equal(expected.mode, actual.mode);
            Assert.Equal(expected.value, actual.value);

            (expected, actual) = GetElementByCodename("body_copy", responseVariant.Elements);
            Assert.Equal(UnifyWhitespace(expected.value), UnifyWhitespace(actual.value));

            // TODO check component of the rich text element

            (expected, actual) = GetElementByCodename("related_articles", responseVariant.Elements);
            Assert.Equal(EXISTING_ITEM_ID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));

            (expected, actual) = GetElementByCodename("personas", responseVariant.Elements);
            Assert.Equal(EXISTING_TAXONOMY_TERM_ID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));

            (expected, actual) = GetElementByCodename("teaser_image", responseVariant.Elements);
            Assert.Equal(EXISTING_ASSET_ID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));

            (expected, actual) = GetElementByCodename("options", responseVariant.Elements);
            Assert.Equal(EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));
            Assert.Equal(EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED, Guid.Parse((actual.value as IEnumerable<dynamic>)?.Skip(1).First().id));
        }

        private void AssertStronglyTypedResponseElements(ComplexTestModel elements)
        {
            Assert.Equal(StronglyTypedElements.Title.Value, elements.Title.Value);
            Assert.Equal(StronglyTypedElements.PostDate.Value, elements.PostDate.Value);
            // TODO extend for complex elements
            // Assert.Equal(UnifyWhitespace(StronglyTypedElements.BodyCopy), UnifyWhitespace(elements.BodyCopy));
            Assert.Equal(StronglyTypedElements.UrlPattern.Mode, elements.UrlPattern.Mode);
            Assert.Equal(StronglyTypedElements.UrlPattern.Value, elements.UrlPattern.Value);
            Assert.NotNull(elements.TeaserImage.Value);
            Assert.Equal(StronglyTypedElements.TeaserImage.Value.FirstOrDefault()?.Id, elements.TeaserImage.Value.FirstOrDefault()?.Id);
            Assert.NotNull(elements.Options.Value);
            Assert.NotEmpty(elements.Options.Value);
            Assert.Equal(StronglyTypedElements.Options.Value.Select(option => option.Id), elements.Options.Value.Select(option => option.Id));
            Assert.Contains(EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID, elements.Options.Value.Select(option => option.Id));
            Assert.Contains(EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED, elements.Options.Value.Select(option => option.Id));

            Assert.Single(elements.RelatedArticles.Value);
            Assert.Equal(EXISTING_ITEM_ID, elements.RelatedArticles.Value.First().Id);

            Assert.Single(elements.Personas.Value);
            Assert.Equal(EXISTING_TAXONOMY_TERM_ID, elements.Personas.Value.FirstOrDefault()?.Id);
        }
    }
}
