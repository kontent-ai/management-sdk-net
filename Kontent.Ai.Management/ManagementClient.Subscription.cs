using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Subscription;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<SubscriptionProjectModel>>> EnumerateSubscriptionProjectPagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<SubscriptionProjectListingResponseServerModel, SubscriptionProjectModel>(
            _subscriptionApi.ListSubscriptionProjectsInternalAsync,
            page => page.Projects,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<SubscriptionUserModel>>> EnumerateSubscriptionUserPagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<SubscriptionUserListingResponseServerModel, SubscriptionUserModel>(
            _subscriptionApi.ListSubscriptionUsersInternalAsync,
            page => page.Users,
            page => page.Pagination?.Token,
            cancellationToken);

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
