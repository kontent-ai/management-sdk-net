namespace Kontent.Ai.Management.Models.Publishing;

/// <summary>
/// Payload for scheduling publishing or unpublishing of a language variant. Use the immediate publish/unpublish methods instead when no schedule is needed.
/// </summary>
public sealed record ScheduleModel
{
    /// <summary>
    /// ISO-8601 date-time at which the variant should be (un)published.
    /// </summary>
    [JsonPropertyName("scheduled_to")]
    public required DateTimeOffset ScheduledTo { get; init; }

    /// <summary>
    /// IANA time zone name used to display the scheduled date's offset in the UI. Optional.
    /// </summary>
    [JsonPropertyName("display_timezone")]
    public string? DisplayTimeZone { get; init; }
}
