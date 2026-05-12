using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Webhooks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<WebhookModel>> ListWebhooksAsync()
        => EnsureSuccess(await _managementApi.ListWebhooksInternalAsync());

    /// <inheritdoc />
    public async Task<WebhookModel> GetWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id)));
    }

    /// <inheritdoc />
    public async Task<WebhookModel> CreateWebhookAsync(WebhookCreateModel webhook)
    {
        ArgumentNullException.ThrowIfNull(webhook);

        return EnsureSuccess(await _managementApi.CreateWebhookInternalAsync(webhook));
    }

    /// <inheritdoc />
    public async Task DeleteWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id)));
    }

    /// <inheritdoc />
    public async Task EnableWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.EnableWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id)));
    }

    /// <inheritdoc />
    public async Task DisableWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DisableWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id)));
    }
}
