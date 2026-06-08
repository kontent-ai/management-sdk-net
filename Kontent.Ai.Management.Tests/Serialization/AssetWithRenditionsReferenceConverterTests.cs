using AwesomeAssertions;
using Kontent.Ai.Management.Serialization.Converters;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.Serialization;

public class AssetWithRenditionsReferenceConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new AssetWithRenditionsReferenceJsonConverter() },
    };

    [Fact]
    public void Read_IdOnly_ReturnsReferenceById()
    {
        var id = Guid.NewGuid();

        var result = JsonSerializer.Deserialize<AssetWithRenditionsReference>($$"""{"id":"{{id}}"}""", Options);

        result!.Id.Should().Be(id);
    }

    [Fact]
    public void Read_MissingId_Throws()
    {
        var act = () => JsonSerializer.Deserialize<AssetWithRenditionsReference>("""{"codename":"x"}""", Options);

        act.Should().Throw<JsonException>().WithMessage("*id*");
    }

    [Fact]
    public void Write_Id_EmitsIdAndRenditions()
    {
        var id = Guid.NewGuid();
        var value = new AssetWithRenditionsReference(Reference.ById(id));

        var node = JsonNode.Parse(JsonSerializer.Serialize(value, Options))!;

        node["id"]!.GetValue<string>().Should().Be(id.ToString());
        node["renditions"]!.AsArray().Should().BeEmpty();
    }

    [Fact]
    public void Write_Codename_EmitsCodename()
    {
        var value = new AssetWithRenditionsReference(Reference.ByCodename("my_asset"));

        var node = JsonNode.Parse(JsonSerializer.Serialize(value, Options))!;

        node["codename"]!.GetValue<string>().Should().Be("my_asset");
        node["id"].Should().BeNull();
    }

    [Fact]
    public void Write_WithRenditions_EmitsRenditionReferences()
    {
        var asset = Guid.NewGuid();
        var rendition = Guid.NewGuid();
        var value = new AssetWithRenditionsReference(Reference.ById(asset), Reference.ById(rendition));

        var node = JsonNode.Parse(JsonSerializer.Serialize(value, Options))!;

        node["renditions"]!.AsArray().Should().HaveCount(1);
        node["renditions"]![0]!["id"]!.GetValue<string>().Should().Be(rendition.ToString());
    }
}
