using System.Globalization;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Serializes a decimal zero of any scale as <c>0</c> rather than <c>0.0</c>. System.Text.Json otherwise
/// preserves the decimal's scale, so a scaled zero such as <c>0.0m</c> would serialize as <c>0.0</c>.
/// </summary>
internal sealed class DecimalJsonConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // A custom converter bypasses the global `NumberHandling.AllowReadingFromString`, so re-apply string
        // tolerance here: the Management API sends a number element's default as a string (e.g. `"value": "10"`).
        if (reader.TokenType != JsonTokenType.String) return reader.GetDecimal();

        var text = reader.GetString()!;
        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            ? value
            : throw new JsonException($"'{text}' is not a valid decimal value.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        // Normalize any scaled zero (e.g. 0.0m) to the scale-0 zero so it serializes as `0`, not `0.0`.
        => writer.WriteNumberValue(value == 0m ? 0m : value);
}
