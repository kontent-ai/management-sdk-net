namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A project in the subscription (response shape).
/// </summary>
public sealed record SubscriptionProjectModel
{
    /// <summary>
    /// Project ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Project name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Whether the project is active.
    /// </summary>
    [JsonPropertyName("is_active")]
    public required bool IsActive { get; init; }

    /// <summary>
    /// The project's environments.
    /// </summary>
    [JsonPropertyName("environments")]
    public required IEnumerable<SubscriptionProjectEnvironmentModel> Environments { get; init; }
}
