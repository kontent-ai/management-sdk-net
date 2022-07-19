using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management.Models.AssetRenditions;

/// <summary>
/// Represents the identifier of the asset rendition.
/// </summary>
public sealed class AssetRenditionIdentifier
{
    /// <summary>
    /// Represents the identifier of the asset rendition.
    /// </summary>
    public Reference AssetIdentifier { get; private set; }

    /// <summary>
    /// Represents the identifier of the language.
    /// </summary>
    public Reference RenditionIdentifier { get; private set; }

    /// <summary>
    /// Creates an instance of asset rendition identifier.
    /// </summary>
    /// <param name="assetIdentifier">The identifier of the asset.</param>
    /// <param name="renditionIdentifier">The identifier of the rendition.</param>
    public AssetRenditionIdentifier(Reference assetIdentifier, Reference renditionIdentifier)
    {
        AssetIdentifier = assetIdentifier;
        RenditionIdentifier = renditionIdentifier;
    }
}
