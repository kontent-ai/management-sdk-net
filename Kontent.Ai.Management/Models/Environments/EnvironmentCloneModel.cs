using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents environment clone settings.
/// </summary>
public class EnvironmentCloneModel
{
    /// <summary>
    /// Gets or sets the name of the new environment.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a collection of RoleIds. All users assigned to these roles will be activated in the cloned environment.
    /// </summary>
    [JsonProperty("roles_to_activate")]
    public ICollection<Guid> RolesToActivate { get; set; }
}
