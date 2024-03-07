using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Represents the environment role model.
/// </summary>
public class EnvironmentRoleModel
{
    /// <summary>
    /// Gets or sets the environment role's ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the environment role's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the environment role's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }
}
