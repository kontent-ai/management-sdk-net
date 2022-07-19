using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Represents the limitation for the number of items.
/// </summary>
public class LimitModel
{
    /// <summary>
    /// Specifies the image size or how many times something can be used within the element.
    /// </summary>
    [JsonProperty("value")]
    public int Value { get; set; }

    /// <summary>
    /// Specifies how to apply the <see cref="Value"/>.
    /// </summary>
    [JsonProperty("condition")]
    public LimitType Condition { get; set; }
}
