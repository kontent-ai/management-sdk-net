using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Reads a boolean that the Management API serializes as a quoted string (e.g. the subscription user role's
/// language <c>is_active</c> field comes back as <c>"true"</c> / <c>"false"</c> rather than a JSON boolean).
/// Tolerates both the string form and a genuine JSON boolean on read; always writes a JSON boolean.
/// </summary>
internal sealed class StringOrBooleanJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.String => bool.Parse(reader.GetString()!),
            _ => throw new JsonException($"Unexpected token {reader.TokenType} when reading a boolean."),
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        => writer.WriteBooleanValue(value);
}
