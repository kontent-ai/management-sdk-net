namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A URL slug element on a content type.
/// </summary>
public sealed record UrlSlugElementMetadataModel : ElementMetadataBase
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
    /// Reference to the text element that feeds the slug's default value. The text element may live in a snippet.
    /// </summary>
    [JsonPropertyName("depends_on")]
    public required UrlSlugDependency DependsOn { get; init; }

    /// <summary>
    /// Regex used to validate the generated slug.
    /// </summary>
    [JsonPropertyName("validation_regex")]
    public ValidationRegexModel? ValidationRegex { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.UrlSlug;
}
