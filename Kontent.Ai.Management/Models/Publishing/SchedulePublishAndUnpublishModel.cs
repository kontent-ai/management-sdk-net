using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Represents the schedule interval model.
/// </summary>
public class SchedulePublishAndUnpublishModel
{
    /// <summary>
    /// Gets or sets ISO-8601 formatted date-time for scheduled publishing.
    /// If you do not provide this property, the publishing schedule won't be updated
    /// </summary>
    [JsonProperty(PropertyName = "publish_scheduled_to")]
    public DateTimeOffset? PublishScheduledTo { get; set; }
    
    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonProperty(PropertyName = "publish_display_timezone")]
    public string PublishDisplayTimeZone { get; set; }

    /// <summary>
    /// Gets or sets ISO-8601 formatted date-time for scheduled unpublishing.
    /// If you do not provide this property, the unpublishing schedule won't be updated
    /// </summary>
    [JsonProperty(PropertyName = "unpublish_scheduled_to")]
    public DateTimeOffset? UnpublishScheduledTo { get; set; }

    /// <summary>
    /// IANA time zone name used to display time offset of the scheduled publish date in the UI.
    /// </summary>
    [JsonProperty(PropertyName = "unpublish_display_timezone")]
    public string UnpublishDisplayTimeZone { get; set; }
}