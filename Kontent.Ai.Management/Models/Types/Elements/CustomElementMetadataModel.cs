namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A custom element on a content type. Hosts a third-party editing experience served from <see cref="SourceUrl"/>.
/// </summary>
public sealed record CustomElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Whether authors must fill in the element. Defaults to false.
    /// </summary>
    [JsonPropertyName("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Whether the element is non-localizable (shared across all languages). Defaults to false.
    /// </summary>
    [JsonPropertyName("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// HTML guidelines shown to authors.
    /// </summary>
    [JsonPropertyName("guidelines")]
    public string? Guidelines { get; init; }

    /// <summary>
    /// Absolute URL that hosts the custom element's UI.
    /// </summary>
    [JsonPropertyName("source_url")]
    public required string SourceUrl { get; init; }

    /// <summary>
    /// Stringified JSON passed to the custom element at runtime. Must be valid JSON when non-null.
    /// </summary>
    [JsonPropertyName("json_parameters")]
    public string? JsonParameters { get; init; }

    /// <summary>
    /// Sibling elements that the custom element can read. Null means no elements are exposed.
    /// </summary>
    [JsonPropertyName("allowed_elements")]
    public IEnumerable<Reference>? AllowedElements { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Custom;
}
