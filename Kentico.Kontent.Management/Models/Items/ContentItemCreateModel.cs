using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Items;

/// <summary>
/// Represents the content item create model.
/// </summary>
public sealed class ContentItemCreateModel
{
    /// <summary>
    /// Gets or sets the name of the content item.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the codename of the content item.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the type of the content item.
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public Reference Type { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content item.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the collection of the content item.
    /// </summary>
    [JsonProperty("collection")]
    public Reference Collection { get; set; }
}
