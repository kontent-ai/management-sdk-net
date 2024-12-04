using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<CollectionsModel> ListCollectionsAsync()
    {
        var endpointUrl = _urlBuilder.BuildCollectionsUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<CollectionsModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<CollectionsModel> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var endpointUrl = _urlBuilder.BuildCollectionsUrl();
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<CollectionOperationBaseModel>, CollectionsModel>(endpointUrl, HttpMethod.Patch, changes);
    }
}
