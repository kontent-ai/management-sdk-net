using System.Reflection;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Serializes enums by their <see cref="EnumMemberAttribute"/> value (falling back to the member
/// name), reproducing the legacy Newtonsoft <c>StringEnumConverter</c> + <c>[EnumMember]</c>
/// behaviour. System.Text.Json's built-in converter writes member names and ignores
/// <c>[EnumMember]</c> on .NET 8 (per-member naming via <c>[JsonStringEnumMemberName]</c> is .NET 9+),
/// so this is required for the wire strings to match the Management API.
/// </summary>
internal sealed class EnumMemberJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (JsonConverter)Activator.CreateInstance(typeof(EnumMemberJsonConverter<>).MakeGenericType(typeToConvert))!;
}

internal sealed class EnumMemberJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    private static readonly Dictionary<TEnum, string> ValueToName = BuildValueToName();
    private static readonly Dictionary<string, TEnum> NameToValue = BuildNameToValue();

    private static Dictionary<TEnum, string> BuildValueToName()
    {
        var map = new Dictionary<TEnum, string>();
        foreach (var name in Enum.GetNames<TEnum>())
        {
            var wire = typeof(TEnum).GetField(name)!.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? name;
            map[Enum.Parse<TEnum>(name)] = wire;
        }
        return map;
    }

    private static Dictionary<string, TEnum> BuildNameToValue()
    {
        var map = new Dictionary<string, TEnum>(StringComparer.OrdinalIgnoreCase);
        foreach (var (value, wire) in ValueToName)
        {
            map[wire] = value;
        }
        return map;
    }

    private static TEnum ParseString(string value)
    {
        if (NameToValue.TryGetValue(value, out var mapped))
        {
            return mapped;
        }

        if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var parsed))
        {
            return parsed;
        }

        throw new JsonException($"'{value}' is not a valid {typeof(TEnum).Name} value.");
    }

    private static void Write(Utf8JsonWriter writer, TEnum value, bool asPropertyName)
    {
        var name = ValueToName.TryGetValue(value, out var mapped) ? mapped : value.ToString();
        if (asPropertyName)
        {
            writer.WritePropertyName(name);
        }
        else
        {
            writer.WriteStringValue(name);
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.String => ParseString(reader.GetString()!),
            // Newtonsoft's StringEnumConverter tolerates integer values on read (AllowIntegerValues).
            JsonTokenType.Number => (TEnum)Enum.ToObject(typeof(TEnum), reader.GetInt64()),
            _ => throw new JsonException($"Unexpected token {reader.TokenType} when reading {typeof(TEnum).Name}."),
        };

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        => Write(writer, value, asPropertyName: false);

    public override TEnum ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => ParseString(reader.GetString()!);

    public override void WriteAsPropertyName(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        => Write(writer, value, asPropertyName: true);
}
