using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents user's role.
/// </summary>
public sealed record RoleModel
{
    /// <summary>
    /// Gets id of user's role.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets reference to languages.
    /// </summary>
    [JsonProperty("languages")]
    public IEnumerable<Reference> Languages { get; init; }
}
