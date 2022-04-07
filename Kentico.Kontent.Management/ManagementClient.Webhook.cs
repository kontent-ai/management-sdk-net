using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Webhooks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management;

public partial class ManagementClient : IManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<WebhookModel>> ListWebhooksAsync()
    {
        var endpointUrl = _urlBuilder.BuildWebhooksUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<WebhookModel>>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<WebhookModel> GetWebhookAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildWebhooksUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<WebhookModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<WebhookModel> CreateWebhookAsync(WebhookCreateModel webhook)
    {
        if (webhook == null)
        {
            throw new ArgumentNullException(nameof(webhook));
        }

        var endpointUrl = _urlBuilder.BuildWebhooksUrl();
        return await _actionInvoker.InvokeMethodAsync<WebhookCreateModel, WebhookModel>(endpointUrl, HttpMethod.Post, webhook);
    }

    /// <inheritdoc />
    public async Task DeleteWebhookAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildWebhooksUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task EnableWebhookAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildWebhooksEnableUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task DisableWebhookAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildWebhooksDisableUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }
}
