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
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets reference to languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<Reference> Languages { get; init; }
}
