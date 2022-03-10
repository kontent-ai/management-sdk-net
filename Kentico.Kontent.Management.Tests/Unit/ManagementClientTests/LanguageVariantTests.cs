using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Kentico.Kontent.Management.Tests.Unit.Data;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Kentico.Kontent.Management.Models.StronglyTyped;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
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
        public async Task GetStronglyTypedLanguageVariantAsync_ById_LanguageId_GetVariant()
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
        public async Task UpsertVariant_ById_LanguageId_UpdatesVariant()
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

        private static LanguageVariantModel GetExpectedLanguageVariantModel()
        {
            return new LanguageVariantModel
            {
                Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
                Language = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024")),
                LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
                WorkflowStep = Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9")),
                Elements = ElementsData.GetExpectedDynamicElements(),
            };
        }

        private static LanguageVariantModel<ComplexTestModel> GetExpectedComplexTestModel() => new()
        {
            Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
            Language = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024")),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            WorkflowStep = Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9")),
            Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
        };
    }
}
