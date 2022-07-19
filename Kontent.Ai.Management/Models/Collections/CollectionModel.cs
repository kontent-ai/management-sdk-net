using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Represents collection model.
/// </summary>
public class CollectionModel
{
    /// <summary>
    /// Gets or sets the id of the content collection.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the content collection.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the codename of the collection.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content collection.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; set; }
}
