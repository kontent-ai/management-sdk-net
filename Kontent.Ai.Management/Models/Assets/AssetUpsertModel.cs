using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents an asset upsert model.
/// </summary>
public sealed record AssetUpsertModel
{
    /// <summary>
    /// Gets the descriptions for the asset.
    /// </summary>
    [JsonPropertyName("descriptions")]
    public IEnumerable<AssetDescription> Descriptions { get; init; }

    /// <summary>
    /// Gets the title for the asset.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; init; }

    /// <summary>
    /// Folder of the asset. If outside of all folders use "id" : "00000000-0000-0000-0000-000000000000".
    /// </summary>
    [JsonPropertyName("folder")]
    public Reference Folder { get; init; }

    /// <summary>
    /// Gets the Collection for the asset.
    /// </summary>
    [JsonPropertyName("collection")]
    public AssetCollectionReference Collection { get; init; }

    /// <summary>
    /// Gets the taxonomy elements of the asset.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<AssetElement> Elements { get; init; }

    /// <summary>
    /// Gets the file reference for the asset.
    /// </summary>
    [JsonPropertyName("file_reference")]
    public FileReference FileReference { get; init; }

    /// <summary>
    /// Gets the codename of the asset.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }
}
