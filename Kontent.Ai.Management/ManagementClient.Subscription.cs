using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Subscription;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<SubscriptionProjectModel>> ListSubscriptionProjectsAsync()
    {
        var response = EnsureSuccess(await _subscriptionApi.ListSubscriptionProjectsInternalAsync());

        return new ListingResponseModel<SubscriptionProjectModel>(
            (continuationToken, _) => GetNextSubscriptionProjectsPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Projects);
    }

    private async Task<IListingResponse<SubscriptionProjectModel>> GetNextSubscriptionProjectsPageAsync(string continuationToken)
        => EnsureSuccess(await _subscriptionApi.ListSubscriptionProjectsInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<SubscriptionUserModel>> ListSubscriptionUsersAsync()
    {
        var response = EnsureSuccess(await _subscriptionApi.ListSubscriptionUsersInternalAsync());

        return new ListingResponseModel<SubscriptionUserModel>(
            (continuationToken, _) => GetNextSubscriptionUsersPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Users);
    }

    private async Task<IListingResponse<SubscriptionUserModel>> GetNextSubscriptionUsersPageAsync(string continuationToken)
        => EnsureSuccess(await _subscriptionApi.ListSubscriptionUsersInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<SubscriptionUserModel> GetSubscriptionUserAsync(UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _subscriptionApi.GetSubscriptionUserInternalAsync(UserSegment(identifier)));
    }

    /// <inheritdoc />
    public async Task ActivateSubscriptionUserAsync(UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _subscriptionApi.ActivateSubscriptionUserInternalAsync(UserSegment(identifier)));
    }

    /// <inheritdoc />
    public async Task DeactivateSubscriptionUserAsync(UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _subscriptionApi.DeactivateSubscriptionUserInternalAsync(UserSegment(identifier)));
    }
}
