using Kontent.Ai.Management.Models.Assets;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's assets.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/assets")]
    internal Task<IApiResponse<AssetListingResponseServerModel>> ListAssetsInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single asset.</summary>
    /// <param name="identifier">The asset identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/assets/{**identifier}")]
    internal Task<IApiResponse<AssetModel>> GetAssetInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new asset.</summary>
    /// <param name="asset">The asset to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/assets")]
    internal Task<IApiResponse<AssetModel>> CreateAssetInternalAsync(
        [Body] AssetCreateModel asset,
        CancellationToken cancellationToken = default);

    /// <summary>Creates or updates an asset.</summary>
    /// <param name="identifier">The asset identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="asset">The asset to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/assets/{**identifier}")]
    internal Task<IApiResponse<AssetModel>> UpsertAssetInternalAsync(
        string identifier,
        [Body] AssetUpsertModel asset,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes an asset.</summary>
    /// <param name="identifier">The asset identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/assets/{**identifier}")]
    internal Task<IApiResponse> DeleteAssetInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Uploads a binary file to be later referenced by an asset.</summary>
    /// <param name="fileName">The file name.</param>
    /// <param name="fileContent">The file body, with its <c>Content-Type</c> set to the file's media type.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/files/{fileName}")]
    internal Task<IApiResponse<FileReference>> UploadFileInternalAsync(
        string fileName,
        [Body] HttpContent fileContent,
        CancellationToken cancellationToken = default);
}
