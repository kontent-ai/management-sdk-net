using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Serialization.Converters;

// Writes a DateTimeOffset as a UTC instant with the "Z" suffix, matching how the Management API stores datetime
// element values — the offset collapses to UTC and any display zone is carried separately. UtcDateTime has Kind=Utc,
// so Utf8JsonWriter emits the trailing "Z" rather than a numeric offset.
internal sealed class UtcDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTimeOffset();

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.UtcDateTime);
}
