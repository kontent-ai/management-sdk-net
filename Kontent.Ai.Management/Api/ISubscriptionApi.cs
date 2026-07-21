using Kontent.Ai.Management.Models.Subscription;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Refit definition of the subscription-scoped Management API v2 endpoints (<c>/v2/subscriptions/{subscriptionId}/…</c>).
/// Built by <c>ManagementApiFactory.CreateSubscription</c> with the subscription base address; the environment-scoped
/// endpoints live on <see cref="IManagementApi"/>.
/// </summary>
internal interface ISubscriptionApi
{
    /// <summary>Lists one page of the subscription's projects.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/projects")]
    internal Task<IApiResponse<SubscriptionProjectListingResponseServerModel>> ListSubscriptionProjectsInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of the subscription's users.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/users")]
    internal Task<IApiResponse<SubscriptionUserListingResponseServerModel>> ListSubscriptionUsersInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single subscription user.</summary>
    /// <param name="userIdentifier">The user identifier path segment (<c>{id}</c> or <c>email/{email}</c>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/users/{**userIdentifier}")]
    internal Task<IApiResponse<SubscriptionUserModel>> GetSubscriptionUserInternalAsync(
        string userIdentifier,
        CancellationToken cancellationToken = default);

    /// <summary>Activates a subscription user.</summary>
    /// <param name="userIdentifier">The user identifier path segment (<c>{id}</c> or <c>email/{email}</c>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/users/{**userIdentifier}/activate")]
    internal Task<IApiResponse> ActivateSubscriptionUserInternalAsync(
        string userIdentifier,
        CancellationToken cancellationToken = default);

    /// <summary>Deactivates a subscription user.</summary>
    /// <param name="userIdentifier">The user identifier path segment (<c>{id}</c> or <c>email/{email}</c>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/users/{**userIdentifier}/deactivate")]
    internal Task<IApiResponse> DeactivateSubscriptionUserInternalAsync(
        string userIdentifier,
        CancellationToken cancellationToken = default);
}
