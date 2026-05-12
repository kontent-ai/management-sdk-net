using Kontent.Ai.Management.Models.Webhooks;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists the webhooks in the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/webhooks-vnext")]
    internal Task<IApiResponse<IEnumerable<WebhookModel>>> ListWebhooksInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single webhook.</summary>
    /// <param name="identifier">The webhook identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/webhooks-vnext/{**identifier}")]
    internal Task<IApiResponse<WebhookModel>> GetWebhookInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new webhook.</summary>
    /// <param name="webhook">The webhook to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/webhooks-vnext")]
    internal Task<IApiResponse<WebhookModel>> CreateWebhookInternalAsync(
        [Body] WebhookCreateModel webhook,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a webhook.</summary>
    /// <param name="identifier">The webhook identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/webhooks-vnext/{**identifier}")]
    internal Task<IApiResponse> DeleteWebhookInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Enables a webhook.</summary>
    /// <param name="identifier">The webhook identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/webhooks-vnext/{**identifier}/enable")]
    internal Task<IApiResponse> EnableWebhookInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Disables a webhook.</summary>
    /// <param name="identifier">The webhook identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/webhooks-vnext/{**identifier}/disable")]
    internal Task<IApiResponse> DisableWebhookInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}
