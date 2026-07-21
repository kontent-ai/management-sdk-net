namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Regex-based validation configuration for a text-like element.
/// </summary>
public sealed record ValidationRegexModel
{
    /// <summary>
    /// Whether the validation is enabled.
    /// </summary>
    [JsonPropertyName("is_active")]
    public required bool IsActive { get; init; }

    /// <summary>
    /// Regular expression used for validation.
    /// </summary>
    [JsonPropertyName("regex")]
    public string? Regex { get; init; }

    /// <summary>
    /// Regex flags. Only the case-insensitive flag ('i') is supported.
    /// </summary>
    [JsonPropertyName("flags")]
    public string? Flags { get; init; }

    /// <summary>
    /// Custom validation message shown when input does not match the regex.
    /// </summary>
    [JsonPropertyName("validation_message")]
    public string? ValidationMessage { get; init; }
}
