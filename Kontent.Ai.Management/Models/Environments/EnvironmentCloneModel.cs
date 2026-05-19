using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents environment clone settings.
/// </summary>
public sealed record EnvironmentCloneModel
{
    /// <summary>
    /// Gets the name of the new environment.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a collection of RoleIds. All users assigned to these roles will be activated in the cloned environment.
    /// </summary>
    [JsonProperty("roles_to_activate")]
    public ICollection<Guid> RolesToActivate { get; init; }

    /// <sumary>
    /// Gets <see cref="Kontent.Ai.Management.Models.Environments.CopyDataOptions"/> for copying entities.
    /// </sumary>
    [JsonProperty("copy_data_options")]
    public CopyDataOptions CopyDataOptions { get; init; }
}
