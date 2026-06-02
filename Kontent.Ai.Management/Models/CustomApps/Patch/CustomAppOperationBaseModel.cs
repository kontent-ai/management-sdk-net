namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Base shape for a custom app PATCH operation. Concrete subtypes specialize the verb (<c>replace</c>, <c>addInto</c>, <c>remove</c>).
/// </summary>
public abstract record CustomAppOperationBaseModel
{
    /// <summary>
    /// Operation verb. Pinned by each concrete subtype.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// Property to operate on. <c>addInto</c> / <c>remove</c> target the <c>allowed_roles</c> collection; <c>replace</c> applies to any property.
    /// </summary>
    [JsonPropertyName("property_name")]
    public required PropertyName PropertyName { get; init; }

    /// <summary>
    /// Value for the operation. Type depends on <see cref="PropertyName"/>. May be <c>null</c> when replacing a nullable property (e.g. clearing <c>config</c>).
    /// </summary>
    [JsonPropertyName("value")]
    public required object? Value { get; init; }
}
