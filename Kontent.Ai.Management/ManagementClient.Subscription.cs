using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Subscription;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<IListingResponseModel<SubscriptionProjectModel>>> ListSubscriptionProjectsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _subscriptionApi.ListSubscriptionProjectsInternalAsync(cancellationToken: cancellationToken);
        return await response.ToManagementResultAsync(BuildSubscriptionProjectsPage);
    }

    private IListingResponseModel<SubscriptionProjectModel> BuildSubscriptionProjectsPage(SubscriptionProjectListingResponseServerModel payload)
        => new ListingResponseModel<SubscriptionProjectModel>(
            (continuationToken, _) => GetNextSubscriptionProjectsPageAsync(continuationToken),
            payload.Pagination?.Token,
            url: string.Empty,
            payload.Projects);

    private async Task<IListingResponse<SubscriptionProjectModel>> GetNextSubscriptionProjectsPageAsync(string continuationToken)
        => EnsureSuccess(await _subscriptionApi.ListSubscriptionProjectsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<IManagementResult<IListingResponseModel<SubscriptionUserModel>>> ListSubscriptionUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _subscriptionApi.ListSubscriptionUsersInternalAsync(cancellationToken: cancellationToken);
        return await response.ToManagementResultAsync(BuildSubscriptionUsersPage);
    }

    private IListingResponseModel<SubscriptionUserModel> BuildSubscriptionUsersPage(SubscriptionUserListingResponseServerModel payload)
        => new ListingResponseModel<SubscriptionUserModel>(
            (continuationToken, _) => GetNextSubscriptionUsersPageAsync(continuationToken),
            payload.Pagination?.Token,
            url: string.Empty,
            payload.Users);

    private async Task<IListingResponse<SubscriptionUserModel>> GetNextSubscriptionUsersPageAsync(string continuationToken)
        => EnsureSuccess(await _subscriptionApi.ListSubscriptionUsersInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<IManagementResult<SubscriptionUserModel>> GetSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _subscriptionApi.GetSubscriptionUserInternalAsync(UserSegment(identifier), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> ActivateSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _subscriptionApi.ActivateSubscriptionUserInternalAsync(UserSegment(identifier), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeactivateSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _subscriptionApi.DeactivateSubscriptionUserInternalAsync(UserSegment(identifier), cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
