
namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Reference to an asset's containing folder, as returned by the Management API. Only the folder's ID is populated in responses.
/// </summary>
public sealed class AssetFolder
{
    /// <summary>
    /// Folder ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}
