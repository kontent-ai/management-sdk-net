using System;

namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Represents the schedule model.
/// </summary>
public sealed record ScheduleModel
{
    /// <summary>
    /// Gets ISO-8601 formatted date-time for scheduled (un)publishing.
    /// If you do not provide this property, the specified language variant is (un)published immediately.
    /// </summary>
    [JsonPropertyName("scheduled_to")]
    public DateTimeOffset ScheduleTo { get; init; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonPropertyName("display_timezone")]
    public string DisplayTimeZone { get; init; }
}
