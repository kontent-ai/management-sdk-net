namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Maximum text length configuration for a text element.
/// </summary>
public sealed record MaximumTextLengthModel
{
    /// <summary>
    /// Maximum number of characters or words.
    /// </summary>
    [JsonPropertyName("value")]
    public required int Value { get; init; }

    /// <summary>
    /// Whether <see cref="Value"/> counts characters or words.
    /// </summary>
    [JsonPropertyName("applies_to")]
    public required TextLengthLimitType AppliesTo { get; init; }
}
