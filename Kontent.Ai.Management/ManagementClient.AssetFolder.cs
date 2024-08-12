using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Models.AssetFolders;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<AssetFoldersModel> GetAssetFoldersAsync()
    {
        var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AssetFoldersModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<AssetFoldersModel> CreateAssetFoldersAsync(AssetFolderCreateModel folder)
    {
        ArgumentNullException.ThrowIfNull(folder);

        var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
        var response = await _actionInvoker.InvokeMethodAsync<AssetFolderCreateModel, AssetFoldersModel>(endpointUrl, HttpMethod.Post, folder);

        return response;
    }

    /// <inheritdoc />
    public async Task<AssetFoldersModel> ModifyAssetFoldersAsync(IEnumerable<AssetFolderOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var endpointUrl = _urlBuilder.BuildAssetFoldersUrl();
        var response = await _actionInvoker.InvokeMethodAsync<IEnumerable<AssetFolderOperationBaseModel>, AssetFoldersModel>(endpointUrl, new HttpMethod("PATCH"), changes);

        return response;
    }
}
