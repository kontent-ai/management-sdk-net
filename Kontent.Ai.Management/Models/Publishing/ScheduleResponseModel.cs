using System;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Represents the schedule model.
/// </summary>
public sealed record ScheduleResponseModel
{
    /// <summary>
    /// Gets ISO-8601 formatted date-time for scheduled publishing.
    /// </summary>
    [JsonPropertyName("publish_time")]
    public DateTime? PublishTime { get; init; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonPropertyName("publish_display_timezone")]
    public string PublishDisplayTimeZone { get; init; }

    /// <summary>
    /// Gets ISO-8601 formatted date-time for scheduled unpublishing.
    /// </summary>
    [JsonPropertyName("unpublish_time")]
    public DateTime? UnpublishTime { get; init; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled unpublish date in the UI.
    /// </summary>
    [JsonPropertyName("unpublish_display_timezone")]
    public string UnpublishDisplayTimeZone { get; init; }
}
