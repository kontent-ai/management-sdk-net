using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the schedule model.
/// </summary>
public class ScheduleModel
{
    /// <summary>
    /// Gets or sets ISO-8601 formatted date-time for scheduled (un)publishing.
    /// If you do not provide this property, the specified language variant is (un)published immediately.
    /// </summary>
    [JsonProperty("scheduled_to")]
    public DateTimeOffset ScheduleTo { get; set; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonProperty("display_timezone")]
    public string DisplayTimeZone { get; set; }
}
