using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Models.TypeSnippets;

/// <summary>
/// A content type snippet (response shape). Snippets are reusable groups of elements that can be inlined into content types.
/// </summary>
public sealed record ContentTypeSnippetModel
{
    /// <summary>
    /// Server-generated snippet ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the last change to the snippet.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Elements defined on the snippet. Snippets cannot contain <c>url_slug</c>, <c>subpages</c>, or <c>content_type_snippet</c> elements — those are rejected by the API.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IReadOnlyList<ElementMetadataBase> Elements { get; init; }

    /// <summary>
    /// Caller-supplied external ID. Only present when one was specified on create.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }
}
