using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Represents environment's roles
/// </summary>
public class EnvironmentRolesModel
{
    /// <summary>
    /// Gets or sets the list of environment roles
    /// </summary>
    [JsonProperty("roles")]
    public IEnumerable<EnvironmentRoleModel> Roles { get; set; }
}
