using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Deserializes a <see cref="Reference"/> via its factory methods (<c>ById</c> / <c>ByCodename</c> /
/// <c>ByExternalId</c>) to honour the discriminated-triplet invariant (exactly one of id, codename,
/// external_id is set). System.Text.Json cannot otherwise construct <see cref="Reference"/> — its
/// parameterless constructor is private — and a public ctor would defeat the factory-only invariant.
/// On write, emits the single populated identifier.
/// </summary>
internal sealed class ReferenceJsonConverter : JsonConverter<Reference>
{
    public override Reference Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (TryGetString(root, "id", out var id))
        {
            return Reference.ById(Guid.Parse(id));
        }

        if (TryGetString(root, "codename", out var codename))
        {
            return Reference.ByCodename(codename);
        }

        if (TryGetString(root, "external_id", out var externalId))
        {
            return Reference.ByExternalId(externalId);
        }

        throw new JsonException("Reference object must contain one of 'id', 'codename', or 'external_id'.");
    }

    public override void Write(Utf8JsonWriter writer, Reference value, JsonSerializerOptions options)
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

        writer.WriteEndObject();
    }

    private static bool TryGetString(JsonElement root, string name, out string value)
    {
        if (root.TryGetProperty(name, out var element) && element.ValueKind == JsonValueKind.String)
        {
            value = element.GetString()!;
            return true;
        }

        value = null!;
        return false;
    }
}
