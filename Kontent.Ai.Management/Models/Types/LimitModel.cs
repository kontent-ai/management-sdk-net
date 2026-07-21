namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Numeric limit configuration shared by elements that constrain item counts or asset sizes.
/// </summary>
public sealed record LimitModel
{
    /// <summary>
    /// The numeric threshold.
    /// </summary>
    [JsonPropertyName("value")]
    public required int Value { get; init; }

    /// <summary>
    /// How <see cref="Value"/> is compared (at most, at least, exactly).
    /// </summary>
    [JsonPropertyName("condition")]
    public required LimitType Condition { get; init; }
}
