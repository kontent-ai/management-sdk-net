namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Publishing/unpublishing schedule on a language variant (response shape). All fields are null when nothing is scheduled.
/// </summary>
public sealed record ScheduleResponseModel
{
    /// <summary>
    /// ISO-8601 date-time of scheduled publishing. Null when no publish is scheduled.
    /// </summary>
    [JsonPropertyName("publish_time")]
    public DateTimeOffset? PublishTime { get; init; }

    /// <summary>
    /// IANA time zone name used to display the scheduled publish date's offset in the UI. Null when no publish is scheduled.
    /// </summary>
    [JsonPropertyName("publish_display_timezone")]
    public string? PublishDisplayTimeZone { get; init; }

    /// <summary>
    /// ISO-8601 date-time of scheduled unpublishing. Null when no unpublish is scheduled.
    /// </summary>
    [JsonPropertyName("unpublish_time")]
    public DateTimeOffset? UnpublishTime { get; init; }

    /// <summary>
    /// IANA time zone name used to display the scheduled unpublish date's offset in the UI. Null when no unpublish is scheduled.
    /// </summary>
    [JsonPropertyName("unpublish_display_timezone")]
    public string? UnpublishDisplayTimeZone { get; init; }
}
