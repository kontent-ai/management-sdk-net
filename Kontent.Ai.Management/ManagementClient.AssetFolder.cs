using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<AssetFoldersModel>> GetAssetFoldersAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.GetAssetFoldersInternalAsync(cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<AssetFoldersModel>> CreateAssetFoldersAsync(AssetFolderCreateModel folders, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(folders);

        return _managementApi.CreateAssetFoldersInternalAsync(folders, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<AssetFoldersModel>> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        return _managementApi.ModifyAssetFoldersInternalAsync(changes, cancellationToken).ToManagementResultAsync();
    }
}
