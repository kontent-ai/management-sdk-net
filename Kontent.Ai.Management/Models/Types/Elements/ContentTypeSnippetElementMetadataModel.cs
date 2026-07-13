namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A content type snippet placeholder. Inlines a reusable group of elements into a content type.
/// </summary>
public sealed record ContentTypeSnippetElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Reference to the snippet that supplies the inlined elements.
    /// </summary>
    [JsonPropertyName("snippet")]
    public required Reference Snippet { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.ContentTypeSnippet;
}
