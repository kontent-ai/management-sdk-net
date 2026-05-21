using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<IListingResponseModel<TaxonomyGroupModel>>> ListTaxonomyGroupsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListTaxonomyGroupsInternalAsync(cancellationToken: cancellationToken);
        return await response.ToManagementResultAsync(BuildTaxonomyGroupsPage);
    }

    private IListingResponseModel<TaxonomyGroupModel> BuildTaxonomyGroupsPage(TaxonomyGroupListingResponseServerModel payload)
        => new ListingResponseModel<TaxonomyGroupModel>(
            (continuationToken, _) => GetNextTaxonomyGroupsPageAsync(continuationToken),
            payload.Pagination?.Token,
            url: string.Empty,
            payload.Taxonomies);

    private async Task<IListingResponse<TaxonomyGroupModel>> GetNextTaxonomyGroupsPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListTaxonomyGroupsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> GetTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> CreateTaxonomyGroupAsync(TaxonomyGroupCreateModel taxonomyGroup, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(taxonomyGroup);

        var response = await _managementApi.CreateTaxonomyGroupInternalAsync(taxonomyGroup, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<TaxonomyGroupModel>> ModifyTaxonomyGroupAsync(Reference identifier, IEnumerable<TaxonomyGroupOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.ModifyTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteTaxonomyGroupAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteTaxonomyGroupInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
