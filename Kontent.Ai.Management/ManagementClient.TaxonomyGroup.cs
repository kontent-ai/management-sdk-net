using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;
public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<TaxonomyGroupModel>> ListTaxonomyGroupsAsync()
    {
        var endpointUrl = _urlBuilder.BuildTaxonomyUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TaxonomyGroupListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<TaxonomyGroupModel>(
            GetNextListingPageAsync<TaxonomyGroupListingResponseServerModel, TaxonomyGroupModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Taxonomies);
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> GetTaxonomyGroupAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TaxonomyGroupModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup)
    {
        ArgumentNullException.ThrowIfNull(taxonomyGroup);

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl();
        return await _actionInvoker.InvokeMethodAsync<TaxonomyGroupCreateModel, TaxonomyGroupModel>(endpointUrl, HttpMethod.Post, taxonomyGroup);
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<TaxonomyGroupOperationBaseModel>, TaxonomyGroupModel>(endpointUrl, HttpMethod.Patch, changes);
    }

    /// <inheritdoc />
    public async Task DeleteTaxonomyGroupAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
