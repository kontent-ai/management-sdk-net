
namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Language-specific alt-text description for an asset.
/// </summary>
public sealed record AssetDescription
{
    /// <summary>
    /// Language reference.
    /// </summary>
    [JsonPropertyName("language")]
    public required Reference Language { get; init; }

    /// <summary>
    /// Description text. Null in responses for languages without an assigned description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
