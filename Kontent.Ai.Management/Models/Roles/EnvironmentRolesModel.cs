namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// The set of roles in an environment (response shape).
/// </summary>
public sealed record EnvironmentRolesModel
{
    /// <summary>
    /// The environment's roles.
    /// </summary>
    [JsonPropertyName("roles")]
    public required IEnumerable<EnvironmentRoleModel> Roles { get; init; }
}
