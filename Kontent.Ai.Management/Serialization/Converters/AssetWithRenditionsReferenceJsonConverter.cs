using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Reads an asset-with-renditions reference from its <c>id</c> only (legacy parity — codename /
/// external id are not accepted on read), and writes the single populated identifier plus the
/// rendition list.
/// </summary>
internal sealed class AssetWithRenditionsReferenceJsonConverter : JsonConverter<AssetWithRenditionsReference>
{
    public override AssetWithRenditionsReference Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        if (!document.RootElement.TryGetStringProperty("id", out var id))
        {
            throw new JsonException("Object does not contain a string 'id' property.");
        }

        return new AssetWithRenditionsReference(Reference.ById(Guid.Parse(id)));
    }

    public override void Write(Utf8JsonWriter writer, AssetWithRenditionsReference value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.Id is { } guid)
        {
            writer.WriteString("id", guid);
        }
        else if (value.Codename is { } codename)
        {
            writer.WriteString("codename", codename);
        }
        else if (value.ExternalId is { } externalId)
        {
            writer.WriteString("external_id", externalId);
        }

        writer.WritePropertyName("renditions");
        JsonSerializer.Serialize(writer, value.Renditions, options);

        writer.WriteEndObject();
    }
}
