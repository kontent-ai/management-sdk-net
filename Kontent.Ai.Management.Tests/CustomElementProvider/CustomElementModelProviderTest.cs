using FluentAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using Kontent.Ai.Management.Tests.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.CustomElementProvider;

public class CustomElementModelProviderTest : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public CustomElementModelProviderTest(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
    }

    [Fact]
    public async Task GetLanguageVariantAsync_WithStronglyTypedModel_UsingExternalId()
    {
        var internalClient = _fileSystemFixture.CreateMockClientWithResponse(Path.Combine("Integrations", "ContentType.json"));

        var client = _fileSystemFixture.CreateMockClientWithResponse(new CustomModelProvider(internalClient), Path.Combine("LanguageVariant", "LanguageVariant.json"));

        var expected = LanguageVariantsData.GetExpectedComplexTestModel();

        var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        var response = await client.GetLanguageVariantAsync<ComplexTestModel>(identifier);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetLanguageVariantAsync_WithDynamicallyTypedModel_UsingExternalId()
    {
        var internalClient = _fileSystemFixture.CreateMockClientWithResponse(Path.Combine("Integrations", "ContentType.json"));

        var client = _fileSystemFixture.CreateMockClientWithResponse(new CustomModelProvider(internalClient), Path.Combine("LanguageVariant", "LanguageVariant.json"));

        var expected = LanguageVariantsData.GetExpectedLanguageVariantModel();

        var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
        var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

        var response = await client.GetLanguageVariantAsync(identifier);

        response.Should().BeEquivalentTo(expected);
    }
}
