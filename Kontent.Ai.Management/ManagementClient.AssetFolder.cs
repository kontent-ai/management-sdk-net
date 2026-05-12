using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<AssetFoldersModel> GetAssetFoldersAsync()
        => EnsureSuccess(await _managementApi.GetAssetFoldersInternalAsync());

    /// <inheritdoc />
    public async Task<AssetFoldersModel> CreateAssetFoldersAsync(AssetFolderCreateModel folder)
    {
        ArgumentNullException.ThrowIfNull(folder);

        return EnsureSuccess(await _managementApi.CreateAssetFoldersInternalAsync(folder));
    }

    /// <inheritdoc />
    public async Task<AssetFoldersModel> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);

        return EnsureSuccess(await _managementApi.ModifyAssetFoldersInternalAsync(changes));
    }
}
