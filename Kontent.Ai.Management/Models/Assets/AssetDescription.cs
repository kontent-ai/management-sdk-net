
namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents the language specific description for the asset.
/// </summary>
public sealed record AssetDescription
{
    /// <summary>
    /// Gets the identifier of the language.
    /// </summary>
    [JsonPropertyName("language")]
    public Reference Language { get; init; }

    /// <summary>
    /// Gets the description of the asset.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; init; }
}
