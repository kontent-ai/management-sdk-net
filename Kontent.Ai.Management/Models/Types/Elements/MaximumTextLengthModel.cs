using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Specifies the maximum text length.
/// </summary>
public sealed record MaximumTextLengthModel
{
    /// <summary>
    /// Gets the maximum number of characters or words.
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; init; }

    /// <summary>
    /// Determines whether the value applies to characters or words.
    /// </summary>
    [JsonPropertyName("applies_to")]
    public TextLengthLimitType AppliesTo { get; init; }
}
