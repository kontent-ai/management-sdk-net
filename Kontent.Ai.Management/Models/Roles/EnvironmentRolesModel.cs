using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Represents environment's roles
/// </summary>
public sealed record EnvironmentRolesModel
{
    /// <summary>
    /// Gets the list of environment roles
    /// </summary>
    [JsonProperty("roles")]
    public IEnumerable<EnvironmentRoleModel> Roles { get; init; }
}
