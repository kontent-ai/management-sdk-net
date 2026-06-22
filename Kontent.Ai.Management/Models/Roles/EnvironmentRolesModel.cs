namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Wire envelope for the roles listing (<c>{ "roles": [...] }</c>); unwrapped to the bare list at the client surface.
/// </summary>
internal sealed record EnvironmentRolesModel
{
    /// <summary>
    /// The environment's roles.
    /// </summary>
    [JsonPropertyName("roles")]
    public required IReadOnlyList<EnvironmentRoleModel> Roles { get; init; }
}
