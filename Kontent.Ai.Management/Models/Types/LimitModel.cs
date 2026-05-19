using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Represents the limitation for the number of items.
/// </summary>
public sealed record LimitModel
{
    /// <summary>
    /// Specifies the image size or how many times something can be used within the element.
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; init; }

    /// <summary>
    /// Specifies how to apply the <see cref="Value"/>.
    /// </summary>
    [JsonPropertyName("condition")]
    public LimitType Condition { get; init; }
}
