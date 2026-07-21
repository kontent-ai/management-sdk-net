using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Gets the environment's asset folder hierarchy.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/folders")]
    internal Task<IApiResponse<AssetFoldersModel>> GetAssetFoldersInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Creates asset folders.</summary>
    /// <param name="folders">The folders to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/folders")]
    internal Task<IApiResponse<AssetFoldersModel>> CreateAssetFoldersInternalAsync(
        [Body] AssetFolderCreateModel folders,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to the environment's asset folders.</summary>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/folders")]
    internal Task<IApiResponse<AssetFoldersModel>> ModifyAssetFoldersInternalAsync(
        [Body] IEnumerable<AssetFolderOperationBaseModel> changes,
        CancellationToken cancellationToken = default);
}
