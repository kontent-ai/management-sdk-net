using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a url slug element in content types.
/// </summary>
public sealed record UrlSlugElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets the element's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the element must be filled in.
    /// </summary>
    [JsonPropertyName("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets element is non-localizable
    /// </summary>
    [JsonPropertyName("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// Gets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonPropertyName("guidelines")]
    public string Guidelines { get; init; }

    /// <summary>
    /// Specifies the text element that provides the default value to the URL slug element. The dependent text element can be part of a content type snippet.
    /// </summary>
    [JsonPropertyName("depends_on")]
    public UrlSlugDependency DependsOn { get; init; }

    /// <summary>
    /// Specifies a regular expression pattern used to validate the text element's value.
    /// </summary>
    [JsonPropertyName("validation_regex")]
    public ValidationRegexModel ValidationRegex { get; init; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.UrlSlug;
}
