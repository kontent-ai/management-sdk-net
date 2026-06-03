namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Value of a date_time element on a generated content-type record: the instant plus the IANA time zone the UI
/// displays it in. The envelope converter flattens this to a <c>value</c> + sibling <c>display_timezone</c> on the wire.
/// </summary>
public sealed record DateTimeValue
{
    /// <summary>The instant.</summary>
    [JsonPropertyName("value")]
    public required DateTimeOffset Value { get; init; }

    /// <summary>IANA zone name shown in the UI (e.g. "Europe/Prague"); null when unset.</summary>
    [JsonPropertyName("display_timezone")]
    public string? DisplayTimeZone { get; init; }

    /// <summary>Keeps the common "just an instant" authoring path a one-liner.</summary>
    public static implicit operator DateTimeValue(DateTimeOffset value) => new() { Value = value };

    /// <summary>Keeps the common "just a date" authoring path a one-liner.</summary>
    public static implicit operator DateTimeValue(DateTime value) => new() { Value = value };
}
