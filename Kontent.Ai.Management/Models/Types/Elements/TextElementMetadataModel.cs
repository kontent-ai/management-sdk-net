using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A text element on a content type.
/// </summary>
public sealed record TextElementMetadataModel : ElementMetadataBase
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
    /// Maximum length of the entered text, in characters or words.
    /// </summary>
    [JsonPropertyName("maximum_text_length")]
    public MaximumTextLengthModel? MaximumTextLength { get; init; }

    /// <summary>
    /// Regex used to validate entered text.
    /// </summary>
    [JsonPropertyName("validation_regex")]
    public ValidationRegexModel? ValidationRegex { get; init; }

    /// <summary>
    /// Default value applied when authors create a new language variant.
    /// </summary>
    [JsonPropertyName("default")]
    public TextElementDefaultValueModel? DefaultValue { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Text;
}
