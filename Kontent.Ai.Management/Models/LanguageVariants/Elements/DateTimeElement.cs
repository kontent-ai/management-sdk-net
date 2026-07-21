using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a date_time element: an instant plus the optional display time zone.</summary>
/// <remarks>Use this to set a date_time element by hand in the untyped element array; <c>Element</c> says which element it targets. With a generated content-type record, set the element via <see cref="Content.DateTimeValue"/> instead.</remarks>
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
