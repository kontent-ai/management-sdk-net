using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a multiple-choice element in content types.
/// </summary>
public class MultipleChoiceElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets or sets the element's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a flag determining whether the element must be filled in.
    /// </summary>
    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets element is non-localizable
    /// </summary>
    [JsonProperty("is_non_localizable")]
    public bool IsNonLocalizable { get; set; }

    /// <summary>
    /// Gets or sets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonProperty("guidelines")]
    public string Guidelines { get; set; }

    /// <summary>
    /// Defines whether the multiple-choice element acts as a single choice (shown as radio buttons in the UI) or multiple-choice (shown as checkboxes in the UI).
    /// </summary>
    [JsonProperty("mode")]
    public MultipleChoiceMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the element's multiple-choice options.
    /// </summary>
    [JsonProperty("options")]
    public IEnumerable<MultipleChoiceOptionModel> Options { get; set; }

    /// <summary>
    /// Specifies the default value for the element value.
    /// </summary>
    [JsonProperty("default")]
    public MultipleChoiceDefaultValueModel DefaultValue { get; set; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.MultipleChoice;
}