using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents the project user model.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Gets or sets the user's ID.
    /// </summary>
    [JsonProperty("user_id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the language's display name.
    /// </summary>
    [JsonProperty("collection_groups")]
    public IEnumerable<UserCollectionGroup> CollectionGroup { get; set; }
}
