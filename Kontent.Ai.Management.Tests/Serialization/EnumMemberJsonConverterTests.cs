using System.Text.Json;
using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Tests.Serialization;

public class EnumMemberJsonConverterTests
{
    private static JsonSerializerOptions Options() => new() { Converters = { new EnumMemberJsonConverterFactory() } };

    [Fact]
    public void Write_UsesEnumMemberValue_NotMemberName()
    {
        var options = Options();

        JsonSerializer.Serialize(ElementMetadataType.LinkedItems, options).Should().Be("\"modular_content\"");
        JsonSerializer.Serialize(ElementMetadataType.RichText, options).Should().Be("\"rich_text\"");
        JsonSerializer.Serialize(ElementMetadataType.ContentTypeSnippet, options).Should().Be("\"snippet\"");
    }

    [Fact]
    public void Read_FromEnumMemberValue_CaseInsensitive()
    {
        var options = Options();

        JsonSerializer.Deserialize<ElementMetadataType>("\"modular_content\"", options).Should().Be(ElementMetadataType.LinkedItems);
        JsonSerializer.Deserialize<ElementMetadataType>("\"MODULAR_CONTENT\"", options).Should().Be(ElementMetadataType.LinkedItems);
    }

    [Fact]
    public void Read_ToleratesInteger()
        => JsonSerializer.Deserialize<ElementMetadataType>("7", Options()).Should().Be(ElementMetadataType.LinkedItems);

    [Fact]
    public void Read_UnknownString_Throws()
    {
        var act = () => JsonSerializer.Deserialize<ElementMetadataType>("\"not_a_type\"", Options());

        act.Should().Throw<JsonException>().WithMessage("*not_a_type*");
    }

    [Fact]
    public void NullableEnum_RoundTrips()
    {
        var options = Options();

        JsonSerializer.Serialize<ElementMetadataType?>(null, options).Should().Be("null");
        JsonSerializer.Serialize<ElementMetadataType?>(ElementMetadataType.Asset, options).Should().Be("\"asset\"");
        JsonSerializer.Deserialize<ElementMetadataType?>("null", options).Should().BeNull();
        JsonSerializer.Deserialize<ElementMetadataType?>("\"asset\"", options).Should().Be(ElementMetadataType.Asset);
    }

    [Fact]
    public void EnumDictionaryKey_UsesEnumMemberValue()
    {
        var options = Options();
        var json = JsonSerializer.Serialize(new Dictionary<ElementMetadataType, int> { [ElementMetadataType.RichText] = 1 }, options);

        json.Should().Be("{\"rich_text\":1}");
        JsonSerializer.Deserialize<Dictionary<ElementMetadataType, int>>(json, options)!
            .Should().ContainKey(ElementMetadataType.RichText);
    }

    [Fact]
    public void Registered_InRefitSettingsProviderOptions()
    {
        var options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

        JsonSerializer.Serialize(ElementMetadataType.UrlSlug, options).Should().Be("\"url_slug\"");
        JsonSerializer.Deserialize<ElementMetadataType>("\"url_slug\"", options).Should().Be(ElementMetadataType.UrlSlug);
    }
}
