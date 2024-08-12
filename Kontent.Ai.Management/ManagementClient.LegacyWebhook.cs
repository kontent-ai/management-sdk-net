using Kontent.Ai.Management.Models.LegacyWebhooks;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<LegacyWebhookModel>> ListLegacyWebhooksAsync()
    {
        var endpointUrl = _urlBuilder.BuildLegacyWebhooksUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<LegacyWebhookModel>>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<LegacyWebhookModel> GetLegacyWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildLegacyWebhooksUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LegacyWebhookModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<LegacyWebhookModel> CreateLegacyWebhookAsync(LegacyWebhookCreateModel webhook)
    {
        ArgumentNullException.ThrowIfNull(webhook);

        var endpointUrl = _urlBuilder.BuildLegacyWebhooksUrl();
        return await _actionInvoker.InvokeMethodAsync<LegacyWebhookCreateModel, LegacyWebhookModel>(endpointUrl, HttpMethod.Post, webhook);
    }

    /// <inheritdoc />
    public async Task DeleteLegacyWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildLegacyWebhooksUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task EnableLegacyWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildLegacyWebhooksEnableUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task DisableLegacyWebhookAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildLegacyWebhooksDisableUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }
}