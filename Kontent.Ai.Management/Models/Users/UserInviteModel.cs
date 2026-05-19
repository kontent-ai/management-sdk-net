using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents user's invitation model.
/// </summary>
public sealed record UserInviteModel
{
    /// <summary>
    /// Gets the email of user that is to be invited.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; init; }

    /// <summary>
    /// Gets the language's display name.
    /// </summary>
    [JsonProperty("collection_groups")]
    public IEnumerable<UserCollectionGroup> CollectionGroup { get; init; }
}
