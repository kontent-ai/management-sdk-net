using System.Text.Json;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Content;
using Xunit;

namespace Kontent.Ai.Management.Tests.Content;

public class AssetReferenceTests
{
    private static readonly Guid SampleId = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid SampleRenditionId = new("22222222-2222-2222-2222-222222222222");

    [Fact]
    public void AssetReference_Deserialize_AllFields()
    {
        var json = $$"""
            {
              "id": "{{SampleId}}",
              "codename": "logo",
              "external_id": "my-logo",
              "renditions": [
                { "id": "{{SampleRenditionId}}" }
              ]
            }
            """;

        var asset = JsonSerializer.Deserialize<AssetReference>(json);

        asset.Should().NotBeNull();
        asset!.Id.Should().Be(SampleId);
        asset.Codename.Should().Be("logo");
        asset.ExternalId.Should().Be("my-logo");
        asset.Renditions.Should().ContainSingle().Which.Id.Should().Be(SampleRenditionId);
    }

    [Fact]
    public void AssetReference_Deserialize_IdOnly_OptionalFieldsNull()
    {
        var json = $$"""{ "id": "{{SampleId}}" }""";

        var asset = JsonSerializer.Deserialize<AssetReference>(json);

        asset!.Id.Should().Be(SampleId);
        asset.Codename.Should().BeNull();
        asset.ExternalId.Should().BeNull();
        asset.Renditions.Should().BeNull();
    }

    [Theory]
    [InlineData("""{ "codename": "logo" }""", null, "logo", null)]
    [InlineData("""{ "external_id": "my-logo" }""", null, null, "my-logo")]
    public void AssetReference_Deserialize_AnyIdentifierShape(string json, string? id, string? codename, string? externalId)
    {
        var asset = JsonSerializer.Deserialize<AssetReference>(json)!;

        asset.Id?.ToString().Should().Be(id);
        asset.Codename.Should().Be(codename);
        asset.ExternalId.Should().Be(externalId);
    }

    [Fact]
    public void AssetReference_Serialize_UsesSnakeCaseExternalId()
    {
        var asset = new AssetReference { ExternalId = "my-logo" };

        var json = JsonSerializer.Serialize(asset);

        json.Should().Contain("\"external_id\":\"my-logo\"");
    }

    [Fact]
    public void AssetReference_RoundTrip_PreservesValue()
    {
        var original = new AssetReference
        {
            Id = SampleId,
            Renditions = [new Reference { Id = SampleRenditionId, Codename = "thumb" }],
        };

        var json = JsonSerializer.Serialize(original);
        var roundtripped = JsonSerializer.Deserialize<AssetReference>(json);

        // BeEquivalentTo rather than Be — synthesized record equality compares IReadOnlyList<T> by reference,
        // and the deserialized List<T> is a different instance from the original collection literal.
        roundtripped.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void AssetReference_RecordEqualityHoldsByValue()
    {
        var a = new AssetReference { Id = SampleId, Codename = "logo" };
        var b = new AssetReference { Id = SampleId, Codename = "logo" };

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}
