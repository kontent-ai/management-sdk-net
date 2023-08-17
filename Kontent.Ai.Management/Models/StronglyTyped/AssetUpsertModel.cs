using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kontent.Ai.Management.Models.StronglyTyped;

/// <summary>
/// Represents a strongly typed asset upsert model.
/// </summary>
public sealed class AssetUpsertModel<T> where T : new()
{
    /// <summary>
    /// Gets or sets the file reference for the asset.
    /// </summary>
    [JsonProperty("file_reference", Required = Required.Always)]
    public FileReference FileReference { get; set; }
    
    /// <summary>
    /// Gets or sets the codename of the asset.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { set; get; }

    /// <summary>
    /// Gets or sets the description for the asset.
    /// </summary>
    [JsonProperty("descriptions", Required = Required.Always)]
    public IEnumerable<AssetDescription> Descriptions { get; set; } = Enumerable.Empty<AssetDescription>();

    /// <summary>
    /// Gets or sets the title for the asset.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Folder of the asset. If outside of all folders use "id" : "00000000-0000-0000-0000-000000000000".
    /// </summary>
    [JsonProperty("folder", Required = Required.Always)]
    public Reference Folder { get; set; }

    /// <summary>
    /// Gets or sets the Collection for the asset.
    /// </summary>
    [JsonProperty("collection")]
    public AssetCollectionReference Collection { get; set; }

    /// <summary>
    /// Gets or sets elements of the asset.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public T Elements { get; set; }
}
