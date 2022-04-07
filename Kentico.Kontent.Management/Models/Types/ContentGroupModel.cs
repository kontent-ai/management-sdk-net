using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Types;

/// <summary>
/// Content group.
/// </summary>
public class ContentGroupModel
{
    /// <summary>
    /// Gets or sets the id of the content group.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the content group.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the codename of the content group.
    /// </summary>
    [JsonProperty("codename")]
    public string CodeName { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content group.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; set; }
}
