namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// A role a subscription user holds within a collection group, scoped to a set of languages.
/// </summary>
public sealed record SubscriptionUserRoleModel
{
    /// <summary>
    /// Role ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Role display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Role codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Languages the role is scoped to. An empty array represents the remaining languages not assigned in any other role.
    /// </summary>
    [JsonPropertyName("languages")]
    public required IEnumerable<SubscriptionUserRoleLanguageModel> Languages { get; init; }
}
