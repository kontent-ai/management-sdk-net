﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the asset folder list.
/// </summary>
public sealed class AssetFoldersModel
{
    /// <summary>
    /// Folder listing (recursive)
    /// </summary>
    [JsonProperty("folders")]
    public IEnumerable<AssetFolderHierarchy> Folders { get; set; }

    /// <summary>
    /// Gets or sets the last modified timestamp of the asset.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; set; }
}
