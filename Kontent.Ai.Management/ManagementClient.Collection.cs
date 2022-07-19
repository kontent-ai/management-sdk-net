﻿using Kontent.Ai.Management.Models.Collections;
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
        if (changes == null)
        {
            throw new ArgumentNullException(nameof(changes));
        }

        var endpointUrl = _urlBuilder.BuildCollectionsUrl();
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<CollectionOperationBaseModel>, CollectionsModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }
}
