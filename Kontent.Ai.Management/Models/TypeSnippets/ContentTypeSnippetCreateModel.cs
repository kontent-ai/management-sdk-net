using Kontent.Ai.Management.Models.Types.Elements;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TypeSnippets;

/// <summary>
/// Represents content snippet type create model.
/// </summary>
public sealed record ContentTypeSnippetCreateModel
{
    /// <summary>
    /// Gets the codename of the content snippet type.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the name of the content snippet type.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets elements of the content snippet type.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<ElementMetadataBase> Elements { get; init; }

    /// <summary>
    /// Gets the external identifier of the content snippet type.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
