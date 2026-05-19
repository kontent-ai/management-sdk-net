using Kontent.Ai.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Represents the content type create model.
/// </summary>
public sealed record ContentTypeCreateModel
{
    /// <summary>
    /// Gets the codename of the content type.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the name of the content type.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; init; }

    /// <summary>
    /// Gets elements of the content type.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<ElementMetadataBase> Elements { get; init; }

    /// <summary>
    /// Gets the external identifier of the content type.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets content groups of the content type.
    /// </summary>
    [JsonProperty("content_groups")]
    public IEnumerable<ContentGroupModel> ContentGroups { get; init; }
}
