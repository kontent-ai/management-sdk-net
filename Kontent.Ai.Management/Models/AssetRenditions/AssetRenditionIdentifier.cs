using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Represents the identifier of the asset rendition.
/// </summary>
public sealed record AssetRenditionIdentifier
{
    /// <summary>
    /// The identifier of the asset.
    /// </summary>
    public required Reference AssetIdentifier { get; init; }

    /// <summary>
    /// The identifier of the rendition.
    /// </summary>
    public required Reference RenditionIdentifier { get; init; }

    /// <summary>
    /// Creates an identifier from the asset and rendition references.
    /// </summary>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <param name="renditionIdentifier">The identifier of the rendition.</param>
    [SetsRequiredMembers]
    public AssetRenditionIdentifier(Reference assetIdentifier, Reference renditionIdentifier)
    {
        AssetIdentifier = assetIdentifier;
        RenditionIdentifier = renditionIdentifier;
    }
}
