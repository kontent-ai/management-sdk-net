using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.StronglyTyped;

/// <summary>
/// Represents a strongly typed digital asset, such as a document or image.
/// </summary>
public sealed record AssetModel<T> where T : new()
{
    /// <summary>
    /// Gets the id of the asset.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the codename of the asset.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the file name of the asset.
    /// </summary>
    [JsonPropertyName("file_name")]
    public string FileName { get; init; }

    /// <summary>
    /// Gets the asset size in bytes.
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; init; }

    /// <summary>
    /// Gets the media type of the asset, for example: "image/jpeg".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; }

    /// <summary>
    /// Gets the url to access the asset binary file.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; init; }

    /// <summary>
    /// Gets the file reference of the asset".
    /// </summary>
    [JsonPropertyName("file_reference")]
    public FileReference FileReference { get; init; }

    /// <summary>
    /// Gets the descriptions of the asset.
    /// </summary>
    [JsonPropertyName("descriptions")]
    public IEnumerable<AssetDescription> Descriptions { get; init; }

    /// <summary>
    /// Gets the title for the asset.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; init; }

    /// <summary>
    /// Gets the external id of the asset.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the asset.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }

    /// <summary>
    /// Image Height
    /// </summary>
    [JsonPropertyName("image_height")]
    public int? ImageHeight { get; init; }

    /// <summary>
    /// Image WIdth
    /// </summary>
    [JsonPropertyName("image_width")]
    public int? ImageWidth { get; init; }

    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder.
    /// </summary>
    [JsonPropertyName("folder")]
    public AssetFolder Folder { get; init; }

    /// <summary>
    /// Gets the Collection for the asset.
    /// </summary>
    [JsonPropertyName("collection")]
    public AssetCollectionReference Collection { get; init; }

    /// <summary>
    /// Gets elements of the asset.
    /// </summary>
    [JsonPropertyName("elements")]
    public T Elements { get; init; }
}
