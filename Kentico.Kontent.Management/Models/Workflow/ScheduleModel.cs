using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Workflow;

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
}
