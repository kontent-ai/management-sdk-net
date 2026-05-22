using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Webhooks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<IEnumerable<WebhookModel>>> ListWebhooksAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListWebhooksInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<WebhookModel>> GetWebhookAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<WebhookModel>> CreateWebhookAsync(WebhookCreateModel webhook, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(webhook);

        var response = await _managementApi.CreateWebhookInternalAsync(webhook, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteWebhookAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> EnableWebhookAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.EnableWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DisableWebhookAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DisableWebhookInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id), cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
