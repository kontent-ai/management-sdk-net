namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A project a subscription user belongs to or has been invited to.
/// </summary>
public sealed record SubscriptionUserProjectModel
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
    /// The project's environments the user belongs to.
    /// </summary>
    [JsonPropertyName("environments")]
    public required IReadOnlyList<SubscriptionUserProjectEnvironmentModel> Environments { get; init; }
}
