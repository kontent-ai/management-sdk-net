namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Payload for scheduling publishing and later unpublishing of a language variant in a single request. Each side is optional — omit a side to leave that part of the schedule unchanged.
/// </summary>
public sealed record SchedulePublishAndUnpublishModel
{
    /// <summary>
    /// ISO-8601 date-time of scheduled publishing. Omit to leave the publishing schedule unchanged.
    /// </summary>
    [JsonPropertyName("publish_scheduled_to")]
    public DateTimeOffset? PublishScheduledTo { get; init; }

    /// <summary>
    /// IANA time zone name used to display the scheduled publish date's offset in the UI. Optional.
    /// </summary>
    [JsonPropertyName("publish_display_timezone")]
    public string? PublishDisplayTimeZone { get; init; }

    /// <summary>
    /// ISO-8601 date-time of scheduled unpublishing. Omit to leave the unpublishing schedule unchanged.
    /// </summary>
    [JsonPropertyName("unpublish_scheduled_to")]
    public DateTimeOffset? UnpublishScheduledTo { get; init; }

    /// <summary>
    /// IANA time zone name used to display the scheduled unpublish date's offset in the UI. Optional.
    /// </summary>
    [JsonPropertyName("unpublish_display_timezone")]
    public string? UnpublishDisplayTimeZone { get; init; }
}
