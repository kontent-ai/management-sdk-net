using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Represents the schedule model.
/// </summary>
public class ScheduleResponseModel
{
    /// <summary>
    /// Gets or sets ISO-8601 formatted date-time for scheduled publishing.
    /// </summary>
    [JsonProperty("publish_time")]
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonProperty("publish_display_timezone")]
    public string PublishDisplayTimeZone { get; set; }
    
    /// <summary>
    /// Gets or sets ISO-8601 formatted date-time for scheduled unpublishing.
    /// </summary>
    [JsonProperty("unpublish_time")]
    public DateTime? UnpublishTime { get; set; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled unpublish date in the UI.
    /// </summary>
    [JsonProperty("unpublish_display_timezone")]
    public string UnpublishDisplayTimeZone { get; set; }
}
