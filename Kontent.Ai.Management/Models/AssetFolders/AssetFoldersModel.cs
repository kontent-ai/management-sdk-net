namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Response shape for retrieving or modifying the asset folder hierarchy of an environment.
/// </summary>
public sealed record AssetFoldersModel
{
    /// <summary>
    /// The recursive asset folder hierarchy.
    /// </summary>
    [JsonPropertyName("folders")]
    public required IReadOnlyList<AssetFolderHierarchy> Folders { get; init; }

    /// <summary>
    /// Timestamp of the most recent folder modification. Populated by the PATCH response; absent in the GET response.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }
}
