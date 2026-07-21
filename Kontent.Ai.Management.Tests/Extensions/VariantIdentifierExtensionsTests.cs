using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Tests.Base;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Extensions;

public class VariantIdentifierExtensionsTests
{
    [Fact]
    public void ToIdentifier_LanguageVariant_UsesItemAndLanguage()
    {
        var variant = JsonSerializer.Deserialize<LanguageVariantModel>(
            File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "LanguageVariant", "LanguageVariant.json")),
            SharedTestJsonOptions.Default)!;

        var identifier = variant.ToIdentifier();

        identifier.ItemIdentifier.Should().Be(variant.Item);
        identifier.LanguageIdentifier.Should().Be(variant.Language);
    }

    [Fact]
    public void ToIdentifier_FilterResult_UsesItemAndLanguage()
    {
        var model = new ItemWithVariantFilterResultModel
        {
            Item = Reference.ById(Guid.NewGuid()),
            Language = Reference.ById(Guid.NewGuid()),
        };

        var identifier = model.ToIdentifier();

        identifier.ItemIdentifier.Should().Be(model.Item);
        identifier.LanguageIdentifier.Should().Be(model.Language);
    }

    [Fact]
    public void ToVariantIdentifier_FilterResult_UsesItemAndLanguage()
    {
        var model = new ItemWithVariantFilterResultModel
        {
            Item = Reference.ByCodename("on_roasts"),
            Language = Reference.ByCodename("en-US"),
        };

        var identifier = model.ToVariantIdentifier();

        identifier.Item.Should().Be(model.Item);
        identifier.Language.Should().Be(model.Language);
    }
}
