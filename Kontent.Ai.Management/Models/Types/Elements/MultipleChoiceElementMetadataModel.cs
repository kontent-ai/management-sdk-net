using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A multiple-choice element on a content type.
/// </summary>
public sealed record MultipleChoiceElementMetadataModel : ContentElementMetadataBase
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Single-choice (radio buttons) or multiple-choice (checkboxes). Required.
    /// </summary>
    [JsonPropertyName("mode")]
    public required MultipleChoiceMode Mode { get; init; }

    /// <summary>
    /// Options shown to authors. Must contain at least one option.
    /// </summary>
    [JsonPropertyName("options")]
    public required IEnumerable<MultipleChoiceOptionModel> Options { get; init; }

    /// <summary>
    /// Default value applied when authors create a new language variant.
    /// </summary>
    [JsonPropertyName("default")]
    public MultipleChoiceDefaultValueModel? DefaultValue { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.MultipleChoice;
}
