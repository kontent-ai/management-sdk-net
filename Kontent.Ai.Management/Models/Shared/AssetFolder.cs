using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Represents an asset folder
/// </summary>
public sealed class AssetFolder
{
    /// <summary>
    /// The referenced folder's ID. Not present if the asset is not in a folder.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
