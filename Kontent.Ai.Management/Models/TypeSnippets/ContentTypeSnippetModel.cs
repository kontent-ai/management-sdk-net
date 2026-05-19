using Kontent.Ai.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TypeSnippets;

/// <summary>
/// Represents content snippet type model.
/// </summary>
public sealed record ContentTypeSnippetModel
{
    /// <summary>
    /// Gets the id of the content snippet type.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the codename of the content snippet type.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the content snippet type.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; init; }

    /// <summary>
    /// Gets the name of the content snippet type.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets elements of the content snippet type.
    /// </summary>
    [JsonProperty("elements")]
    public IEnumerable<ElementMetadataBase> Elements { get; init; }

    /// <summary>
    /// Gets the external identifier of the content snippet type.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; init; }
}
