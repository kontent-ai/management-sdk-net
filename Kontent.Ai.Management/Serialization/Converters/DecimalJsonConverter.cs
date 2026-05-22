using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Serializes a decimal zero of any scale as <c>0</c> rather than <c>0.0</c> (legacy parity,
/// issue #29). System.Text.Json otherwise preserves the decimal's scale, so a scaled zero such
/// as <c>0.0m</c> would serialize as <c>0.0</c>.
/// </summary>
internal sealed class DecimalJsonConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        // Mirror Newtonsoft + the global `NumberHandling.AllowReadingFromString` policy: tolerate
        // string-encoded decimals (e.g. MAPI's element default `"value": "10"` for a number element).
        => reader.TokenType == JsonTokenType.String
            ? decimal.Parse(reader.GetString()!, System.Globalization.CultureInfo.InvariantCulture)
            : reader.GetDecimal();

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        if (value == 0m)
        {
            writer.WriteNumberValue(0);
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }
}
