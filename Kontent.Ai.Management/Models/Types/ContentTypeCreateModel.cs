using Kontent.Ai.Management.Models.Types.Elements;
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
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the name of the content type.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets elements of the content type.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<ElementMetadataBase> Elements { get; init; }

    /// <summary>
    /// Gets the external identifier of the content type.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets content groups of the content type.
    /// </summary>
    [JsonPropertyName("content_groups")]
    public IEnumerable<ContentGroupModel> ContentGroups { get; init; }
}
