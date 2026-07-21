namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Base shape for the content elements that share author-facing configuration across element kinds
/// (requiredness, localizability, guidelines). The structural
/// <see cref="ContentTypeSnippetElementMetadataModel"/> and <see cref="GuidelinesElementMetadataModel"/>
/// carry none of these and derive from <see cref="ElementMetadataBase"/> directly.
/// </summary>
public abstract record ContentElementMetadataBase : ElementMetadataBase
{
    /// <summary>
    /// Whether authors must fill in the element. Defaults to false.
    /// </summary>
    [JsonPropertyName("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Whether the element is shared across all language variants (non-localizable). Defaults to false.
    /// </summary>
    [JsonPropertyName("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// HTML guidelines shown to authors.
    /// </summary>
    [JsonPropertyName("guidelines")]
    public string? Guidelines { get; init; }
}
