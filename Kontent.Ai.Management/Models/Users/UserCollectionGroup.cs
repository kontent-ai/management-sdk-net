using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents user's colection group.
/// </summary>
public class UserCollectionGroup
{
    /// <summary>
    /// Gets or sets user's collection.
    /// </summary>
    [JsonProperty("collections")]
    public IEnumerable<Reference> Collections { get; set; }

    /// <summary>
    /// Gets or sets user's roles.
    /// </summary>
    [JsonProperty("roles")]
    public IEnumerable<RoleModel> Roles { get; set; }
}
