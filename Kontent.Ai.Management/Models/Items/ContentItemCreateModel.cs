using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents the content item create model.
/// </summary>
public sealed record ContentItemCreateModel
{
    /// <summary>
    /// Gets the name of the content item.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the content item.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the type of the content item.
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets the external identifier of the content item.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the collection of the content item.
    /// </summary>
    [JsonProperty("collection")]
    public Reference Collection { get; init; }
}
