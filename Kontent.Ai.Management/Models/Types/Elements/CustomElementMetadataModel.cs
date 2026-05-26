namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a custom element in content types.
/// </summary>
public sealed record CustomElementMetadataModel : ElementMetadataBase
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
    /// Gets the absolute URL that hosts your custom element.
    /// </summary>
    [JsonPropertyName("source_url")]
    public string SourceUrl { get; init; }

    /// <summary>
    /// Gets the optional parameters that allow you to use the element in different content types or provide a customizable layout.
    /// The value must be a valid stringified JSON.
    /// </summary>
    [JsonPropertyName("json_parameters")]
    public string JsonParameters { get; init; }

    /// <summary>
    /// Specifies the elements that this custom element can read from.
    /// </summary>
    [JsonPropertyName("allowed_elements")]
    public IEnumerable<Reference> AllowedElements { get; init; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Custom;
}
