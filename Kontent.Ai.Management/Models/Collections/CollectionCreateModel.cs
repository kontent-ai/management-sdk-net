using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Represents collection the create model.
/// </summary>
public sealed record CollectionCreateModel
{
    /// <summary>
    /// Gets the name of the content collection.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the codename of the collection.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the external identifier of the content collection.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; init; }
}
