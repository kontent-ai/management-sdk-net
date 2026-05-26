namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents due date model.
/// </summary>
public sealed record DueDateModel
{
    /// <summary>
    /// Gets ISO-8601 formatted date-time for due date.
    /// </summary>
    [JsonPropertyName("value")]
    public DateTime? Value { get; init; }
}