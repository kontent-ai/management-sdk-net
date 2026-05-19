using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Specifies the maximum text length.
/// </summary>
public sealed record MaximumTextLengthModel
{
    /// <summary>
    /// Gets the maximum number of characters or words.
    /// </summary>
    [JsonProperty("value")]
    public int Value { get; init; }

    /// <summary>
    /// Determines whether the value applies to characters or words.
    /// </summary>
    [JsonProperty("applies_to")]
    public TextLengthLimitType AppliesTo { get; init; }
}
