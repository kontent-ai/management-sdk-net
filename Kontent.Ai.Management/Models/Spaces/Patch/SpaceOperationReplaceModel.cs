namespace Kontent.Ai.Management.Models.Spaces.Patch;

/// <summary>
/// A <c>replace</c> operation in the modify-space patch payload.
/// </summary>
public sealed record SpaceOperationReplaceModel
{
    /// <summary>
    /// Operation verb. Always <c>replace</c> — the only verb supported on the spaces patch endpoint.
    /// </summary>
    [JsonPropertyName("op")]
    public string Op => "replace";

    /// <summary>
    /// Property to replace.
    /// </summary>
    [JsonPropertyName("property_name")]
    public required PropertyName PropertyName { get; init; }

    /// <summary>
    /// New value. Type depends on <see cref="PropertyName"/>. Set to <c>null</c> to unset the root item.
    /// </summary>
    [JsonPropertyName("value")]
    public required object? Value { get; init; }
}
