namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents due date model.
/// </summary>
public sealed record DueDateModel
{
    /// <summary>
    /// The due date, as an ISO-8601 date-time.
    /// </summary>
    [JsonPropertyName("value")]
    public DateTimeOffset? Value { get; init; }
}