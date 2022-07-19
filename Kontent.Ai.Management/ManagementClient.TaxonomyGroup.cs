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
            (token, url) => GetNextListingPageAsync<TaxonomyGroupListingResponseServerModel, TaxonomyGroupModel>(token, url),
            response.Pagination?.Token,
            endpointUrl,
            response.Taxonomies);
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> GetTaxonomyGroupAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TaxonomyGroupModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup)
    {
        if (taxonomyGroup == null)
        {
            throw new ArgumentNullException(nameof(taxonomyGroup));
        }

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl();
        return await _actionInvoker.InvokeMethodAsync<TaxonomyGroupCreateModel, TaxonomyGroupModel>(endpointUrl, HttpMethod.Post, taxonomyGroup);
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<TaxonomyGroupOperationBaseModel>, TaxonomyGroupModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }

    /// <inheritdoc />
    public async Task DeleteTaxonomyGroupAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildTaxonomyUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
