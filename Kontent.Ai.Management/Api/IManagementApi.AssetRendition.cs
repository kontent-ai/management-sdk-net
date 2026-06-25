using Kontent.Ai.Management.Models.AssetRenditions;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of an asset's renditions.</summary>
    /// <param name="assetIdentifier">The asset identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/assets/{**assetIdentifier}/renditions")]
    internal Task<IApiResponse<AssetRenditionsListingResponseServerModel>> ListAssetRenditionsInternalAsync(
        string assetIdentifier,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single asset rendition.</summary>
    /// <param name="path">The full <c>{asset}/renditions/{rendition}</c> path segment.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/assets/{**path}")]
    internal Task<IApiResponse<AssetRenditionModel>> GetAssetRenditionInternalAsync(
        string path,
        CancellationToken cancellationToken = default);

    /// <summary>Updates an asset rendition.</summary>
    /// <param name="path">The full <c>{asset}/renditions/{rendition}</c> path segment.</param>
    /// <param name="updateModel">The rendition to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/assets/{**path}")]
    internal Task<IApiResponse<AssetRenditionModel>> UpdateAssetRenditionInternalAsync(
        string path,
        [Body] AssetRenditionUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>Creates an asset rendition.</summary>
    /// <param name="assetIdentifier">The asset identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="createModel">The rendition to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/assets/{**assetIdentifier}/renditions")]
    internal Task<IApiResponse<AssetRenditionModel>> CreateAssetRenditionInternalAsync(
        string assetIdentifier,
        [Body] AssetRenditionCreateModel createModel,
        CancellationToken cancellationToken = default);
}
