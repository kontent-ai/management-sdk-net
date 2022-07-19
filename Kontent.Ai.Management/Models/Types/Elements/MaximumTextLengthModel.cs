using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Specifies the maximum text length.
/// </summary>
public class MaximumTextLengthModel
{
    /// <summary>
    /// Gets or sets the maximum number of characters or words.
    /// </summary>
    [JsonProperty("value")]
    public int Value { get; set; }

    /// <summary>
    /// Determines whether the value applies to characters or words.
    /// </summary>
    [JsonProperty("applies_to")]
    public TextLengthLimitType AppliesTo { get; set; }
}
