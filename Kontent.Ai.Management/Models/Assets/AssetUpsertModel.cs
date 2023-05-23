using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents an asset upsert model.
/// </summary>
public sealed class AssetUpsertModel
{
    /// <summary>
    /// Gets or sets the description for the asset.
    /// </summary>
    [JsonProperty("descriptions")]
    public IEnumerable<AssetDescription> Descriptions { get; set; }

    /// <summary>
    /// Gets or sets the title for the asset.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Folder of the asset. If outside of all folders use "id" : "00000000-0000-0000-0000-000000000000".
    /// </summary>
    [JsonProperty("folder")]
    public Reference Folder { get; set; }
    
    /// <summary>
    /// Gets or sets the Collection for the asset.
    /// </summary>
    [JsonProperty("collection")]
    public AssetCollectionReference Collection { get; set; }

    /// <summary>
    /// Gets or sets elements of the asset.
    /// </summary>
    [JsonProperty("elements")]
    public IEnumerable<dynamic> Elements { get; set; }

    /// <summary>
    /// Gets or sets the file reference for the asset.
    /// </summary>
    [JsonProperty("file_reference")]
    public FileReference FileReference { get; set; }
}
