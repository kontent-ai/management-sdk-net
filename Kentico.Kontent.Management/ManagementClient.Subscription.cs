using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Subscription;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management;

public partial class ManagementClient : IManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<SubscriptionProjectModel>> ListSubscriptionProjectsAsync()
    {
        var endpointUrl = _urlBuilder.BuildSubscriptionProjectsUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionProjectListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<SubscriptionProjectModel>(
            (token, url) => GetNextListingPageAsync<SubscriptionProjectListingResponseServerModel, SubscriptionProjectModel>(token, url),
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
            (token, url) => GetNextListingPageAsync<SubscriptionUserListingResponseServerModel, SubscriptionUserModel>(token, url),
            response.Pagination?.Token,
            endpointUrl,
            response.Users);
    }

    /// <inheritdoc />
    public async Task<SubscriptionUserModel> GetSubscriptionUserAsync(UserIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildSubscriptionUserUrl(identifier);

        return await _actionInvoker.InvokeReadOnlyMethodAsync<SubscriptionUserModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task ActivateSubscriptionUserAsync(UserIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildSubscriptionUserActivateUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task DeactivateSubscriptionUserAsync(UserIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildSubscriptionUserDeactivateDisableUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }
}
