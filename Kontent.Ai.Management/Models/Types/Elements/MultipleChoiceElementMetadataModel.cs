using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a multiple-choice element in content types.
/// </summary>
public sealed record MultipleChoiceElementMetadataModel : ElementMetadataBase
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
    /// Defines whether the multiple-choice element acts as a single choice (shown as radio buttons in the UI) or multiple-choice (shown as checkboxes in the UI).
    /// </summary>
    [JsonPropertyName("mode")]
    public MultipleChoiceMode Mode { get; init; }

    /// <summary>
    /// Gets the element's multiple-choice options.
    /// </summary>
    [JsonPropertyName("options")]
    public IEnumerable<MultipleChoiceOptionModel> Options { get; init; }

    /// <summary>
    /// Specifies the default value for the element value.
    /// </summary>
    [JsonPropertyName("default")]
    public MultipleChoiceDefaultValueModel DefaultValue { get; init; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.MultipleChoice;
}