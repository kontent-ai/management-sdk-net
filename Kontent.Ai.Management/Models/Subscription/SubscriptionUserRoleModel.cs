using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents Set of roles the user is assigned to within the collection.
/// </summary>
public sealed record SubscriptionUserRoleModel
{
    /// <summary>
    /// Gets id of user's role.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets name of user's role.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets codename of user's role.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets reference to languages.
    /// </summary>
    [JsonProperty("languages")]
    public IEnumerable<SubscriptionUserRoleLangaugeModel> Languages { get; init; }
}
