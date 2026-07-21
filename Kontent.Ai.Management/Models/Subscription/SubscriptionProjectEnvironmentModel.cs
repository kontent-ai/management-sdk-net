namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents project's environment.
/// </summary>
public sealed record SubscriptionProjectEnvironmentModel
{
    /// <summary>
    /// Gets the environment's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the environment's name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
