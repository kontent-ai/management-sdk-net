using System;

namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Represents the schedule interval model.
/// </summary>
public sealed record SchedulePublishAndUnpublishModel
{
    /// <summary>
    /// Gets ISO-8601 formatted date-time for scheduled publishing.
    /// If you do not provide this property, the publishing schedule won't be updated
    /// </summary>
    [JsonPropertyName("publish_scheduled_to")]
    public DateTimeOffset? PublishScheduledTo { get; init; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonPropertyName("publish_display_timezone")]
    public string PublishDisplayTimeZone { get; init; }

    /// <summary>
    /// Gets ISO-8601 formatted date-time for scheduled unpublishing.
    /// If you do not provide this property, the unpublishing schedule won't be updated
    /// </summary>
    [JsonPropertyName("unpublish_scheduled_to")]
    public DateTimeOffset? UnpublishScheduledTo { get; init; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonPropertyName("unpublish_display_timezone")]
    public string UnpublishDisplayTimeZone { get; init; }
}