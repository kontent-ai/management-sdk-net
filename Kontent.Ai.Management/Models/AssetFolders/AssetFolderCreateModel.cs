﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetFolders;

/// <summary>
/// Represents the asset folder list.
/// </summary>
public class AssetFolderCreateModel
{
    /// <summary>
    /// Folder listing (recursive)
    /// </summary>
    [JsonProperty("folders")]
    public IEnumerable<AssetFolderHierarchy> Folders { get; set; }
}
