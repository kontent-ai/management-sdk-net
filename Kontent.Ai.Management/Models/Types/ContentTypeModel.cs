using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// Content type.
/// </summary>
public sealed record ContentTypeModel
{
    /// <summary>
    /// Gets the id of the content type.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the codename of the content type.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the content type.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }

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
