using System.Text.Json.Serialization;
using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a date_time element: an instant plus the optional display time zone.</summary>
public sealed record DateTimeElement : BaseElement
{
    /// <summary>The instant, serialized as a UTC "Z" value to match the API's storage.</summary>
    [JsonPropertyName("value")]
    [JsonConverter(typeof(UtcDateTimeOffsetJsonConverter))]
    public DateTimeOffset? Value { get; init; }

    /// <summary>IANA zone name shown in the UI (e.g. "Europe/Prague"); omitted when null.</summary>
    [JsonPropertyName("display_timezone")]
    public string? DisplayTimeZone { get; init; }
}
