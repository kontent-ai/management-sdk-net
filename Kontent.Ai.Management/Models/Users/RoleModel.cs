using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Users;

/// <summary>
/// Represents user's role.
/// </summary>
public sealed record RoleModel
{
    /// <summary>
    /// Gets id of user's role.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets reference to languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<Reference> Languages { get; init; }
}
