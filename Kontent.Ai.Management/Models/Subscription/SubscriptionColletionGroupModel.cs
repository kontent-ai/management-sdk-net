using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents set of collections user is assigned to with a set of roles.
/// </summary>
public sealed record SubscriptionColletionGroupModel
{
    /// <summary>
    /// Gets references to internal identifiers of collections that the user is assigned to.
    /// If the array is empty, the user can access any collection.
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference> Collections { get; init; }

    /// <summary>
    /// Gets roles the user is assigned to within the collection.
    /// </summary>
    [JsonPropertyName("roles")]
    public IEnumerable<SubscriptionUserRoleModel> Roles { get; init; }
}
