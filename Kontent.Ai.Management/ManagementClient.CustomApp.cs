using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<CustomAppModel>> ListCustomAppsAsync()
    {
        var endpointUrl = _urlBuilder.BuildCustomAppUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<CustomAppListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<CustomAppModel>(
            GetNextListingPageAsync<CustomAppListingResponseServerModel, CustomAppModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.CustomApps);
    }

    /// <inheritdoc />
    public async Task<CustomAppModel> GetCustomAppAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildCustomAppUrl(identifier);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<CustomAppModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<CustomAppModel> CreateCustomAppAsync(CustomAppCreateModel customApp)
    {
        ArgumentNullException.ThrowIfNull(customApp);

        var endpointUrl = _urlBuilder.BuildCustomAppUrl();
        return await _actionInvoker.InvokeMethodAsync<CustomAppCreateModel, CustomAppModel>(endpointUrl, HttpMethod.Post, customApp);
    }

    /// <inheritdoc />
    public async Task DeleteCustomAppAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildCustomAppUrl(identifier);
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task<CustomAppModel> ModifyCustomAppAsync(Reference identifier, IEnumerable<CustomAppOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        var endpointUrl = _urlBuilder.BuildCustomAppUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<CustomAppOperationBaseModel>, CustomAppModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }
}