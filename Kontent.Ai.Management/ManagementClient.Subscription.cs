using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Subscription;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<SubscriptionProjectModel>>> ListSubscriptionProjectsAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<SubscriptionProjectListingResponseServerModel, SubscriptionProjectModel>(
            _subscriptionApi.ListSubscriptionProjectsInternalAsync,
            page => page.Projects,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<SubscriptionUserModel>>> ListSubscriptionUsersAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<SubscriptionUserListingResponseServerModel, SubscriptionUserModel>(
            _subscriptionApi.ListSubscriptionUsersInternalAsync,
            page => page.Users,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public Task<IManagementResult<SubscriptionUserModel>> GetSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _subscriptionApi.GetSubscriptionUserInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult> ActivateSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _subscriptionApi.ActivateSubscriptionUserInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult> DeactivateSubscriptionUserAsync(UserIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _subscriptionApi.DeactivateSubscriptionUserInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }
}
