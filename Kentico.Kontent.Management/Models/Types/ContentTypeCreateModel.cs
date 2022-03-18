using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types;

/// <summary>
/// Represents the content type create model.
/// </summary>
public class ContentTypeCreateModel
{
    /// <summary>
    /// Gets or sets the codename of the content type.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the name of the content type.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets elements of the content type.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<ElementMetadataBase> Elements { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content type.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets content groups of the content type.
    /// </summary>
    [JsonProperty("content_groups")]
    public IEnumerable<ContentGroupModel> ContentGroups { get; set; }
}
