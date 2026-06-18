using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<AssetFoldersModel>> GetAssetFoldersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetAssetFoldersInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetFoldersModel>> CreateAssetFoldersAsync(AssetFolderCreateModel folder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(folder);

        var response = await _managementApi.CreateAssetFoldersInternalAsync(folder, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AssetFoldersModel>> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyAssetFoldersInternalAsync(changes, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
