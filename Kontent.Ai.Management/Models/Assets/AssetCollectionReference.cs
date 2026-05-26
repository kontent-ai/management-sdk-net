
namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Wraps the reference to an asset's collection. The inner reference can be null to indicate "no collection assignment" on responses from legacy projects.
/// </summary>
public sealed record AssetCollectionReference
{
    /// <summary>
    /// Reference to the collection. Null when the asset is uncollected (only possible on legacy projects).
    /// </summary>
    [JsonPropertyName("reference")]
    public Reference? Reference { get; init; }
}