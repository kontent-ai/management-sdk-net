using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Roles;

/// <summary>
/// Represents the project role model.
/// </summary>
public class ProjectRoleModel
{
    /// <summary>
    /// Gets or sets the project role's ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the project role's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the project role's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }
}
