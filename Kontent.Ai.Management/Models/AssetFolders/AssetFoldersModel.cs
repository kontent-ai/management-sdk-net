using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the asset folder list.
/// </summary>
public sealed record AssetFoldersModel
{
    /// <summary>
    /// Folder listing (recursive)
    /// </summary>
    [JsonPropertyName("folders")]
    public IEnumerable<AssetFolderHierarchy> Folders { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the asset.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }
}
