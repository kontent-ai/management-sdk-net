using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents an asset create model.
/// </summary>
public sealed record AssetCreateModel
{
    /// <summary>
    /// Gets the file reference for the asset.
    /// </summary>
    [JsonPropertyName("file_reference")]
    public FileReference FileReference { get; init; }

    /// <summary>
    /// Gets the descriptions for the asset.
    /// </summary>
    [JsonPropertyName("descriptions")]
    public IEnumerable<AssetDescription> Descriptions { get; init; } = [];

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
    /// Gets the external identifier of the asset.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the taxonomy elements of the asset.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<AssetElement> Elements { get; init; }

    /// <summary>
    /// Gets the codename of the asset.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }
}
