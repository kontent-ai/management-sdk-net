using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Request payload for creating a new content type.
/// </summary>
public sealed record ContentTypeCreateModel
{
    /// <summary>
    /// Caller-supplied codename. When omitted, the CMS generates one from the name.
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Elements that make up this content type.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IEnumerable<ElementMetadataBase> Elements { get; init; }

    /// <summary>
    /// Caller-supplied external ID.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Content groups to define on the type.
    /// </summary>
    [JsonPropertyName("content_groups")]
    public IEnumerable<ContentGroupModel>? ContentGroups { get; init; }
}
