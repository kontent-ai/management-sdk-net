using Kontent.Ai.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TypeSnippets;

/// <summary>
/// Represents content snippet type create model.
/// </summary>
public class ContentTypeSnippetCreateModel
{
    /// <summary>
    /// Gets or sets the codename of the content snippet type.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the name of the content snippet type.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets elements of the content snippet type.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<ElementMetadataBase> Elements { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the content snippet type.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }
}
