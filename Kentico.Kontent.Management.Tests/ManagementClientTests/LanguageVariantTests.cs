using Kentico.Kontent.Management.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;
using Xunit.Abstractions;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "LanguageVariant")]
    public class LanguageVariantTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public LanguageVariantTests(ITestOutputHelper output)
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
        public async Task UpsertVariant_ById_LanguageId_UpdatesVariant()
        {
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        public async Task UpsertVariant_ByCodename_LanguageId_UpdatesVariant()
        {
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };

            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        public async Task UpsertVariant_ById_LanguageCodename_UpdatesVariant()
        {
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };

            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        public async Task UpsertVariant_ByCodename_LanguageCodename_UpdatesVariant()
        {
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };

            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);
        }

        [Fact]
        public async Task UpsertVariant_ByExternalId_LanguageCodename_UpdatesVariant()
        {
            // Arrange
            var externalId = "fe2e8c24f0794f01b36807919602625d";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };
            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(responseVariant.Language.Id, EXISTING_LANGUAGE_ID);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task UpsertVariant_ByExternalId_LanguageCodename_CreatesVariant()
        {
            // Arrange
            var externalId = "348052a5ad8c44ddac1e9683923d74a5";
            await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };
            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task UpsertVariant_ByExternalId_LanguageId_UpdatesVariant()
        {
            // Arrange
            var externalId = "d5e050980baa43b085b909cdea4c6d2b";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task UpsertVariant_ByExternalId_LanguageId_CreatesVariant()
        {
            // Arrange
            var externalId = "73e02811b05f429284006ea94c68c8f7";
            await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var languageVariantUpsertModel = new LanguageVariantUpsertModel() { Elements = Elements };

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task UpsertVariant_UsingResponseModel_UpdatesVariant()
        {
            // Arrange
            var externalId = "4357b71d21eb45369d54a635faf7672b";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var emptyElements = new List<object>();
            var preparedVariant = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, emptyElements, preparedItem);

            // Test
            preparedVariant.Elements = Elements;
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, LanguageVariant: preparedVariant);

            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task UpsertVariant_UsingResponseModel_CreatesVariant()
        {
            // Arrange
            var externalId = "5249f596a8be4d719bc9816e3d416d16";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var emptyElements = new List<object>();
            var preparedVariant = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, emptyElements, preparedItem);

            // Test
            preparedVariant.Elements = Elements;
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(Guid.Empty);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, LanguageVariant: preparedVariant);

            Assert.Equal(Guid.Empty, responseVariant.Language.Id);
            AssertResponseElements(responseVariant);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task ListLanguageVariantsByItem_ById_ListsVariants()
        {
            var identifier = Reference.ById(EXISTING_ITEM_ID);

            var responseVariants = await _client.ListLanguageVariantsByItemAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants.First().Item.Id);
        }

        [Fact]
        public async Task ListLanguageVariantsByItem_ByCodename_ListsVariants()
        {
            var identifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);

            var responseVariants = await _client.ListLanguageVariantsByItemAsync(identifier);

            Assert.Equal(EXISTING_ITEM_ID, responseVariants.First().Item.Id);
        }

        [Fact]
        public async Task ListLanguageVariantsByItem_ByExternalId_ListsVariants()
        {
            // Arrange
            var externalId = "0220e6ec5b77401ea113b5273c8cdd5e";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var identifier = Reference.ByExternalId(externalId);
            var responseVariants = await _client.ListLanguageVariantsByItemAsync(identifier);

            Assert.Single(responseVariants);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void ListLanguageVariantsByType_WithContinuation_ListsVariants()
        {
            var identifier = Reference.ById(EXISTING_CONTENT_TYPE_ID);

            var responseVariants = await _client.ListLanguageVariantsByTypeAsync(identifier);

            Assert.NotNull(responseVariants);

            while (true)
            {
                foreach (var item in responseVariants)
                {
                    Assert.NotNull(item);
                }

                if (!responseVariants.HasNextPage())
                {
                    break;
                }
                responseVariants = await responseVariants.GetNextPage();
                Assert.NotNull(responseVariants);
            }
        }

        [Fact]
        public async Task ListLanguageVariantsByType_ById_ListsVariants()
        {
            var identifier = Reference.ById(EXISTING_CONTENT_TYPE_ID);

            var responseVariants = await _client.ListLanguageVariantsByTypeAsync(identifier);

            var item = await _client.GetContentItemAsync(Reference.ById(responseVariants.First().Item.Id.Value));

            Assert.Equal(EXISTING_CONTENT_TYPE_ID, item.Type.Id);
        }

        [Fact]
        public async Task ListLanguageVariantsByType_ByCodename_ListsVariants()
        {
            var identifier = Reference.ByCodename(EXISTING_CONTENT_TYPE_CODENAME);

            var responseVariants = await _client.ListLanguageVariantsByTypeAsync(identifier);

            var item = await _client.GetContentItemAsync(Reference.ById(responseVariants.First().Item.Id.Value));

            Assert.Equal(EXISTING_CONTENT_TYPE_ID, item.Type.Id);
        }

        [Fact]
        public async Task ListLanguageVariantsByType_ByExternalId_ListsVariants()
        {
            var identifier = Reference.ByExternalId("b7aa4a53-d9b1-48cf-b7a6-ed0b182c4b89");

            var responseVariants = await _client.ListLanguageVariantsByTypeAsync(identifier);

            var item = await _client.GetContentItemAsync(Reference.ById(responseVariants.First().Item.Id.Value));

            Assert.Equal(EXISTING_CONTENT_TYPE_ID, item.Type.Id);
        }

        [Fact]
        //todo add ByExternalId test
        //todo test pagination
        public async Task ListLanguageVariantsOfContentTypeWithComponents_ByCodename_ListsVariants()
        {
            var identifier = Reference.ByCodename(EXISTING_COMPONENT_TYPE_CODENAME);

            var responseVariants = await _client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);

            Assert.NotEmpty(responseVariants);
        }

        [Fact]
        public async Task ListLanguageVariantsOfContentTypeWithComponents_ById_ListsVariants()
        {
            var identifier = Reference.ById(RICH_TEXT_COMPONENT_TWEET_TYPE_ID);

            var responseVariants = await _client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);

            Assert.NotEmpty(responseVariants);
        }

        [Fact]
        //todo add ByExternalId and ById tests
        //todo test pagination
        public async Task ListLanguageVariantByCollectionAsync_ByCodename_ListsVariants()
        {
            var identifier = Reference.ById(Guid.Empty);

            var responseVariants = await _client.ListLanguageVariantsByCollectionAsync(identifier);

            Assert.NotEmpty(responseVariants);
        }

        [Fact]
        public async Task GetLanguageVariant_ById_LanguageId_GetsVariant()
        {
            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async Task GetLanguageVariant_ById_LanguageCodename_GetsVariant()
        {
            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async Task GetLanguageVariant_ByCodename_LanguageId_GetsVariant()
        {
            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async Task GetLanguageVariant_ByCodename_LanguageCodename_GetsVariant()
        {
            var itemIdentifier = Reference.ByCodename(EXISTING_ITEM_CODENAME);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
        }

        [Fact]
        public async Task GetLanguageVariant_ByExternalId_LanguageCodename_GetsVariant()
        {
            // Arrange
            var externalId = "f9cfaa3e00f64e22a144fdacf4cba3e5";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(preparedItem.Id, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task GetLanguageVariant_ByExternalId_ReturnsVariant()
        {
            var externalId = "ad66f70ed9bb4b8694116c9119c4a930";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync(identifier);

            Assert.NotNull(response);
            Assert.Equal(preparedItem.Id, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async Task DeleteLanguageVariant_ById_LanguageCodename_DeletesVariant()
        {
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, itemResponse);

            var itemIdentifier = Reference.ById(itemResponse.Id);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteLanguageVariantAsync(identifier);
        }

        [Fact]
        public async Task DeleteLanguageVariant_ById_LanguageId_DeletesVariant()
        {
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, itemResponse);

            var itemIdentifier = Reference.ById(itemResponse.Id);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteLanguageVariantAsync(identifier);
        }

        [Fact]
        public async Task DeleteLanguageVariant_ByCodename_LanguageId_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, itemResponse);

            var itemIdentifier = Reference.ByCodename(itemResponse.Codename);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteLanguageVariantAsync(identifier);
        }

        [Fact]
        public async Task DeleteLanguageVariant_ByCodename_LanguageCodename_DeletesVariant()
        {
            // Prepare item
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, itemResponse);

            var itemIdentifier = Reference.ByCodename(itemResponse.Codename);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteLanguageVariantAsync(identifier);
        }

        [Fact]
        public async Task DeleteLanguageVariant_ByExternalId_LanguageId_DeletesVariant()
        {
            var externalId = "90285b1a983c43299638c8a835f16b81";
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, itemResponse);

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteLanguageVariantAsync(identifier);
        }

        [Fact]
        public async Task DeleteLanguageVariant_ByExternalId_LanguageCodename_DeletesVariant()
        {
            var externalId = "f4fe87222b6b46739bc673f6e5165c12";
            var itemResponse = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, itemResponse);

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.DeleteLanguageVariantAsync(identifier);
        }

        [Fact]
        public async Task ListStronglyTypedLanguageVariants_ById_ListsVariants()
        {
            var identifier = Reference.ById(EXISTING_ITEM_ID);

            var responseVariants = await _client.ListLanguageVariantsByItemAsync<ComplexTestModel>(identifier);

            Assert.All(responseVariants, x =>
            {
                Assert.NotNull(x.Elements);
            });
            Assert.Equal(EXISTING_ITEM_ID, responseVariants.First().Item.Id);
        }

        [Fact]
        public async Task GetStronglyTypedLanguageVariantAsync_ById_LanguageId_GetVariant()
        {
            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await _client.GetLanguageVariantAsync<ComplexTestModel>(identifier);

            Assert.NotNull(response);
            Assert.Equal(EXISTING_ITEM_ID, response.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Language.Id);
            Assert.NotNull(response.Elements);
        }

        [Fact]
        public async Task UpsertStronglyTypedLanguageVariantAsync_ById_LanguageId_UpdatesVariant()
        {
            var itemIdentifier = Reference.ById(EXISTING_ITEM_ID);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await _client.UpsertLanguageVariantAsync(identifier, StronglyTypedElements);

            Assert.Equal(EXISTING_ITEM_ID, responseVariant.Item.Id);
            Assert.Equal(EXISTING_LANGUAGE_ID, responseVariant.Language.Id);
            Assert.NotNull(responseVariant.Elements);
            AssertStronglyTypedResponseElements(responseVariant.Elements);
        }

        private static (dynamic expected, dynamic actual) GetElementByPropertyName(string propertyName, IEnumerable<dynamic> actualElements)
        {
            var propertyId = typeof(ComplexTestModel).GetProperty(propertyName).GetKontentElementId();
            var expected = Elements.Single(x => x.element.id == propertyId);
            var actual = actualElements.Single(x => x.element.id == expected.element.id.ToString("d"));

            return (expected, actual);
        }

        private static string UnifyWhitespace(string text)
        {
            return Regex.Replace(text, "\\s+", string.Empty);
        }

        private void AssertResponseElements(LanguageVariantModel responseVariant)
        {
            var (expected, actual) = GetElementByPropertyName(nameof(ComplexTestModel.Title), responseVariant.Elements);
            Assert.Equal(expected.value, actual.value);

            (expected, actual) = GetElementByPropertyName(nameof(ComplexTestModel.PostDate), responseVariant.Elements);
            Assert.Equal(expected.value, actual.value);

            (expected, actual) = GetElementByPropertyName(nameof(ComplexTestModel.UrlPattern), responseVariant.Elements);
            Assert.Equal(expected.mode, actual.mode);
            Assert.Equal(expected.value, actual.value);

            (expected, actual) = GetElementByPropertyName(nameof(ComplexTestModel.BodyCopy), responseVariant.Elements);
            Assert.Equal(UnifyWhitespace(expected.value), UnifyWhitespace(actual.value));

            // TODO check component of the rich text element

            (_, actual) = GetElementByPropertyName(nameof(ComplexTestModel.RelatedArticles), responseVariant.Elements);
            Assert.Equal(EXISTING_ITEM_ID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));

            (_, actual) = GetElementByPropertyName(nameof(ComplexTestModel.Personas), responseVariant.Elements);
            Assert.Equal(EXISTING_TAXONOMY_TERM_ID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));

            (_, actual) = GetElementByPropertyName(nameof(ComplexTestModel.TeaserImage), responseVariant.Elements);
            Assert.Equal(EXISTING_ASSET_ID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));

            (_, actual) = GetElementByPropertyName(nameof(ComplexTestModel.Options), responseVariant.Elements);
            Assert.Equal(EXISTING_MULTIPLE_CHOICE_OPTION_ID_PAID, Guid.Parse((actual.value as IEnumerable<dynamic>)?.First().id));
            Assert.Equal(EXISTING_MULTIPLE_CHOICE_OPTION_ID_FEATURED, Guid.Parse((actual.value as IEnumerable<dynamic>)?.Skip(1).First().id));
        }

        private static void AssertStronglyTypedResponseElements(ComplexTestModel elements)
        {
            Assert.Equal(StronglyTypedElements.Title.Value, elements.Title.Value);

            Assert.Equal(StronglyTypedElements.PostDate.Value, elements.PostDate.Value);

            // TODO extend for complex elements
            Assert.Equal(UnifyWhitespace(StronglyTypedElements.BodyCopy.Value), UnifyWhitespace(elements.BodyCopy.Value));

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

            Assert.Equal("{\"formId\": 42}", elements.SelectedForm.Value);
            Assert.Equal("Almighty form!", elements.SelectedForm.SearchableValue);
        }
    }
}
