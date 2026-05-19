using System;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Represents the environment role model.
/// </summary>
public sealed record EnvironmentRoleModel
{
    /// <summary>
    /// Gets the environment role's ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the environment role's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the environment role's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }
}
