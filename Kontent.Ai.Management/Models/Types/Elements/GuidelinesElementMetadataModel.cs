namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// A guidelines element. Carries authoring instructions only; has no value at the content-item level.
/// </summary>
public sealed record GuidelinesElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// HTML guidelines shown to authors. Required.
    /// </summary>
    [JsonPropertyName("guidelines")]
    public required string Guidelines { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("type")]
    public override ElementMetadataType Type => ElementMetadataType.Guidelines;
}
