using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents a digital asset, such as a document or image.
/// </summary>
public sealed record AssetModel
{
    /// <summary>
    /// Gets the id of the asset.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the codename of the asset.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the file name of the asset.
    /// </summary>
    [JsonProperty("file_name")]
    public string FileName { get; init; }

    /// <summary>
    /// Gets the asset size in bytes.
    /// </summary>
    [JsonProperty("size")]
    public long Size { get; init; }

    /// <summary>
    /// Gets the media type of the asset, for example: "image/jpeg".
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; init; }

    /// <summary>
    /// Gets the url to access the asset binary file.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; init; }

    /// <summary>
    /// Gets the file reference of the asset".
    /// </summary>
    [JsonProperty("file_reference")]
    public FileReference FileReference { get; init; }

    /// <summary>
    /// Gets the descriptions of the asset.
    /// </summary>
    [JsonProperty("descriptions")]
    public IEnumerable<AssetDescription> Descriptions { get; init; }

    /// <summary>
    /// Gets the title for the asset.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; init; }

    /// <summary>
    /// Gets the external id of the asset.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the asset.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; init; }

    /// <summary>
    /// Image Height
    /// </summary>
    [JsonProperty("image_height")]
    public int? ImageHeight { get; init; }

    /// <summary>
    /// Image WIdth
    /// </summary>
    [JsonProperty("image_width")]
    public int? ImageWidth { get; init; }

    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder.
    /// </summary>
    [JsonProperty("folder")]
    public AssetFolder Folder { get; init; }

    /// <summary>
    /// Gets the Collection for the asset.
    /// </summary>
    [JsonProperty("collection")]
    public AssetCollectionReference Collection { get; init; }

    /// <summary>
    /// Gets elements of the asset.
    /// </summary>
    [JsonProperty("elements")]
    public IEnumerable<dynamic> Elements { get; init; }
}
