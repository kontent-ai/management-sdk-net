﻿using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<EnvironmentClonedModel> CloneEnvironmentAsync(EnvironmentCloneModel cloneEnvironmentModel)
    {
        if (cloneEnvironmentModel == null)
        {
            throw new ArgumentNullException(nameof(cloneEnvironmentModel));
        }

        var endpointUrl = _urlBuilder.BuildCloneEnvironmentUrl();
        return await _actionInvoker.InvokeMethodAsync<EnvironmentCloneModel, EnvironmentClonedModel>(endpointUrl, HttpMethod.Post, cloneEnvironmentModel);
    }

    /// <inheritdoc />
    public async Task<EnvironmentCloningStateModel> GetEnvironmentCloningStateAsync()
    {
        var endpointUrl = _urlBuilder.BuildGetEnvironmentCloningStateUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<EnvironmentCloningStateModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task DeleteEnvironmentAsync()
    {
        var endpointUrl = _urlBuilder.BuildEnvironmentUrl();
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel)
    {
        if (markAsProductionModel == null)
        {
            throw new ArgumentNullException(nameof(markAsProductionModel));
        }

        var endpointUrl = _urlBuilder.BuildMarkEnvironmentAsProductionUrl();
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, markAsProductionModel);
    }

    /// <inheritdoc />
    public async Task<EnvironmentModel> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes)
    {
        if (changes == null)
        {
            throw new ArgumentNullException(nameof(changes));
        }

        var endpointUrl = _urlBuilder.BuildEnvironmentUrl();
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<EnvironmentOperationBaseModel>, EnvironmentModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }
}
