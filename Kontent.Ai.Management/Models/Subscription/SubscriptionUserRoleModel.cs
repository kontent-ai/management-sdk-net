using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

/// <summary>
/// Represents Set of roles the user is assigned to within the collection.
/// </summary>
public sealed class SubscriptionUserRoleModel
{
    /// <summary>
    /// Gets or sets id of user's role.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets name of user's role.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets codename of user's role.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets reference to languages.
    /// </summary>
    [JsonProperty("languages")]
    public IEnumerable<SubscriptionUserRoleLangaugeModel> Languages { get; set; }
}
