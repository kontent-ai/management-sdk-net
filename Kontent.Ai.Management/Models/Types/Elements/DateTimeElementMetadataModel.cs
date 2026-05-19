using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a date and time element in content types.
/// </summary>
public sealed record DateTimeElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets the element's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets a flag determining whether the element must be filled in.
    /// </summary>
    [JsonProperty("is_required")]
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets element is non-localizable
    /// </summary>
    [JsonProperty("is_non_localizable")]
    public bool IsNonLocalizable { get; init; }

    /// <summary>
    /// Gets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonProperty("guidelines")]
    public string Guidelines { get; init; }

    /// <summary>
    /// Specifies the default value for the element value.
    /// </summary>
    [JsonProperty("default")]
    public DateElementDefaultValueModel DefaultValue { get; init; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.DateTime;
}