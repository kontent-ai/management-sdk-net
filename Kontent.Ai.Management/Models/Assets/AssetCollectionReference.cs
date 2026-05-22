
namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Contains the reference to the asset's collection.
/// </summary>
public sealed record AssetCollectionReference
{
    /// <summary>
    /// Gets the reference.
    /// </summary>
    public Reference Reference { get; init; }
}