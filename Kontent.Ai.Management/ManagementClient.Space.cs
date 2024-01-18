using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public partial class ManagementClient
{

    /// <inheritdoc />
    public async Task<SpaceModel> CreateSpaceAsync(SpaceCreateModel space)
    {
        ArgumentNullException.ThrowIfNull(space);

        var endpointUrl = _urlBuilder.BuildSpacesUrl();
        return await _actionInvoker.InvokeMethodAsync<SpaceCreateModel, SpaceModel>(endpointUrl, HttpMethod.Post, space);
    }

    /// <inheritdoc />
    public async Task<SpaceModel> GetSpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSpacesUrl(identifier);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<SpaceModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SpaceModel>> ListSpacesAsync()
    {
        var endpointUrl = _urlBuilder.BuildSpacesUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<SpaceModel>>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<SpaceModel> ModifySpaceAsync(Reference identifier, IEnumerable<SpaceOperationReplaceModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var endpointUrl = _urlBuilder.BuildSpacesUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<SpaceOperationReplaceModel>, SpaceModel>(endpointUrl, HttpMethod.Patch, changes);
    }

    /// <inheritdoc />
    public async Task DeleteSpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSpacesUrl(identifier);
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}