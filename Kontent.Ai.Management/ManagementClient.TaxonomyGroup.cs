using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<TaxonomyGroupModel>> ListTaxonomyGroupsAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListTaxonomyGroupsInternalAsync());

        return new ListingResponseModel<TaxonomyGroupModel>(
            (continuationToken, _) => GetNextTaxonomyGroupsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Taxonomies);
    }

    private async Task<IListingResponse<TaxonomyGroupModel>> GetNextTaxonomyGroupsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListTaxonomyGroupsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> GetTaxonomyGroupAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetTaxonomyGroupInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup)
    {
        ArgumentNullException.ThrowIfNull(taxonomyGroup);

        return EnsureSuccess(await _managementApi.CreateTaxonomyGroupInternalAsync(taxonomyGroup));
    }

    /// <inheritdoc />
    public async Task<TaxonomyGroupModel> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.ModifyTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), changes));
    }

    /// <inheritdoc />
    public async Task DeleteTaxonomyGroupAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteTaxonomyGroupInternalAsync(identifier.ToUrlSegment()));
    }
}
