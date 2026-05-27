using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Models.Types;

/// <summary>
/// A content type definition.
/// </summary>
public sealed record ContentTypeModel
{
    /// <summary>
    /// Content type ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Codename. Auto-generated from the name when not supplied on create.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the most recent change.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }

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
    /// Caller-supplied external ID. Only present when one was specified on create.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Content groups defined on this type. Empty when none.
    /// </summary>
    [JsonPropertyName("content_groups")]
    public required IEnumerable<ContentGroupModel> ContentGroups { get; init; }
}
