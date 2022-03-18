using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Base;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Kentico.Kontent.Management.Models.StronglyTyped;
using System.Linq;
using Kentico.Kontent.Management.Extensions;
using Kentico.Kontent.Management.Models.Workflow;
using Kentico.Kontent.Management.Tests.Data;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public class LanguageVariantTests : IClassFixture<FileSystemFixture>
    {
        private readonly FileSystemFixture _fileSystemFixture;

        public LanguageVariantTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("LanguageVariant");
        }

        [Fact]
        public async Task ListLanguageVariantsByItemAsync_StronglyTyped_ListsVariants()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            var expected = new[] { "00000000-0000-0000-0000-000000000000", "10000000-0000-0000-0000-000000000000" }
            .Select(GetExpectedComplexTestModel);

            var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));

            var response = await client.ListLanguageVariantsByItemAsync<ComplexTestModel>(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListLanguageVariantsByItemAsync_StronglyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.ListLanguageVariantsByItemAsync<ComplexTestModel>(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ListLanguageVariantsByItemAsync_DynamicallyTyped_ListsVariants()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            var expected = new[] { "00000000-0000-0000-0000-000000000000", "10000000-0000-0000-0000-000000000000" }
            .Select(x => GetExpectedLanguageVariantModel(languageId: x));

            var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));

            var response = await client.ListLanguageVariantsByItemAsync(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListLanguageVariantsByItemAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.ListLanguageVariantsByItemAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void ListLanguageVariantsByTypeAsync_DynamicallyTyped_WithContinuation_ListsVariants()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariantsPage1.json", "LanguageVariantsPage2.json", "LanguageVariantsPage3.json");

            var expected = new[]
            {
                (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
                (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
                (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
            }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

            var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));

            var response = await client.ListLanguageVariantsByTypeAsync(identifier).GetAllAsync();

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListLanguageVariantsByTypeAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.ListLanguageVariantsByTypeAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_DynamicallyTyped_WithContinuation_ListsVariants()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariantsPage1.json", "LanguageVariantsPage2.json", "LanguageVariantsPage3.json");

            var expected = new[]
            {
                (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
                (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
                (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
            }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

            var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));

            var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier).GetAllAsync();

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.ListLanguageVariantsOfContentTypeWithComponentsAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void ListLanguageVariantsByCollectionAsync_DynamicallyTyped_WithContinuation_ListsVariants()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariantsPage1.json", "LanguageVariantsPage2.json", "LanguageVariantsPage3.json");

            var expected = new[]
            {
                (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
                (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
                (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
                (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
            }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

            var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));

            var response = await client.ListLanguageVariantsByCollectionAsync(identifier).GetAllAsync();

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListLanguageVariantsByCollectionAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.ListLanguageVariantsByCollectionAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetLanguageVariantAsync_StronglyTyped_GetsVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedComplexTestModel();

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetLanguageVariantAsync<ComplexTestModel>(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.GetLanguageVariantAsync<ComplexTestModel>(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetLanguageVariantAsync_DynamicallyTyped_GetsVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedLanguageVariantModel();

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetLanguageVariantAsync(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.GetLanguageVariantAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_StronglyTyped_UpsertsVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedComplexTestModel();

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.UpsertLanguageVariantAsync(identifier, GetExpectedComplexTestModel().Elements);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.UpsertLanguageVariantAsync(null, new ComplexTestModel()))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_StronglyTyped_LanguageVariantUpsertModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (ComplexTestModel)null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_DynamicallyTyped_UpsertsVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedLanguageVariantModel();

            var upsertModel = new LanguageVariantUpsertModel { Elements = expected.Elements };

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.UpsertLanguageVariantAsync(null, new LanguageVariantUpsertModel()))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_DynamicallyTyped_LanguageVariantUpsertModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantUpsertModel)null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_UpsertsVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedLanguageVariantModel();

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.UpsertLanguageVariantAsync(identifier, expected);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            await client.Invoking(x => x.UpsertLanguageVariantAsync(null, new LanguageVariantModel()))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_LanguageVariantModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariants.json");

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantModel)null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteLanguageVariantAsync_DeletesVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var identifier = new LanguageVariantIdentifier(Reference.ByCodename("ItemCodename"), Reference.ByCodename("languageCodename"));

            Func<Task> deleteTaxonomy = async () => await client.DeleteLanguageVariantAsync(identifier);

            await deleteTaxonomy.Should().NotThrowAsync();
        }

        private static LanguageVariantModel GetExpectedLanguageVariantModel(
            string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024",
            string itemId = "4b628214-e4fe-4fe0-b1ff-955df33e1515") => new LanguageVariantModel
            {
                Item = Reference.ById(Guid.Parse(itemId)),
                Language = Reference.ById(Guid.Parse(languageId)),
                LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
                Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
                Elements = ElementsData.GetExpectedDynamicElements(),
            };

        private static LanguageVariantModel<ComplexTestModel> GetExpectedComplexTestModel(string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024") => new()
        {
            Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
            Language = Reference.ById(Guid.Parse(languageId)),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
            Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
        };
    }
}
