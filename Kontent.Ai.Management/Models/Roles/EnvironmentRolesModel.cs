using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Represents environment's roles
/// </summary>
public sealed record EnvironmentRolesModel
{
    /// <summary>
    /// Gets the list of environment roles
    /// </summary>
    [JsonPropertyName("roles")]
    public IEnumerable<EnvironmentRoleModel> Roles { get; init; }
}
