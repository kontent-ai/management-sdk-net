using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Models.Assets;

/// <summary>
/// Represents an asset create model.
/// </summary>
public sealed class AssetCreateModel
{
    /// <summary>
    /// Gets or sets the file reference for the asset.
    /// </summary>
    [JsonProperty("file_reference", Required = Required.Always)]
    public FileReference FileReference { get; set; }

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
    /// Gets or sets the external identifier of the asset.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }
    
    /// <summary>
    /// Gets or sets elements of the asset.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<dynamic> Elements { get; set; }
}
