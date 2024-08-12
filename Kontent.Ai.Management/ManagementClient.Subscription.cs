using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Subscription;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<SubscriptionProjectModel>> ListSubscriptionProjectsAsync()
    {
        var endpointUrl = _urlBuilder.BuildSubscriptionProjectsUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionProjectListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<SubscriptionProjectModel>(
            GetNextListingPageAsync<SubscriptionProjectListingResponseServerModel, SubscriptionProjectModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Projects);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<SubscriptionUserModel>> ListSubscriptionUsersAsync()
    {
        var endpointUrl = _urlBuilder.BuildSubscriptionUsersUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionUserListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<SubscriptionUserModel>(
            GetNextListingPageAsync<SubscriptionUserListingResponseServerModel, SubscriptionUserModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Users);
    }

    /// <inheritdoc />
    public async Task<SubscriptionUserModel> GetSubscriptionUserAsync(UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSubscriptionUserUrl(identifier);

        return await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionUserModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task ActivateSubscriptionUserAsync(UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSubscriptionUserActivateUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task DeactivateSubscriptionUserAsync(UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildSubscriptionUserDeactivateDisableUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }
}
