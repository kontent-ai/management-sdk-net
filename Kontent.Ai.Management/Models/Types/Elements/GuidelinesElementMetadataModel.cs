using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a guidelines element in content types.
/// </summary>
public sealed record GuidelinesElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonProperty("guidelines", Required = Required.Always)]
    public string Guidelines { get; init; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.Guidelines;
}
