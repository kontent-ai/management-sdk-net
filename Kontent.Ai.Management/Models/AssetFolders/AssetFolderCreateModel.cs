using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the asset folder list.
/// </summary>
public sealed record AssetFolderCreateModel
{
    /// <summary>
    /// Folder listing (recursive)
    /// </summary>
    [JsonPropertyName("folders")]
    public required IEnumerable<AssetFolderHierarchy> Folders { get; init; }
}
