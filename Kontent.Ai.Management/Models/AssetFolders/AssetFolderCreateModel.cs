namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the body of an asset-folder create request.
/// </summary>
public sealed record AssetFolderCreateModel
{
    /// <summary>
    /// The folder hierarchy to create (recursive).
    /// </summary>
    [JsonPropertyName("folders")]
    public required IEnumerable<AssetFolderHierarchy> Folders { get; init; }
}
