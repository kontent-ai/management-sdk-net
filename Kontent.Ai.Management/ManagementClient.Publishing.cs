using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Workflow;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public sealed partial class ManagementClient
{
    /// <inheritdoc />
    public async Task ChangeLanguageVariantWorkflowAsync(LanguageVariantIdentifier identifier, WorkflowStepIdentifier workflowStepIdentifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(workflowStepIdentifier);

        var endpointUrl = _urlBuilder.BuildWorkflowChangeUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, workflowStepIdentifier);
    }

    /// <inheritdoc />
    public async Task PublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(scheduleModel);

        var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
    }

    /// <inheritdoc />
    public async Task CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildCancelPublishingVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildCancelUnpublishingVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(scheduleModel);

        var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
    }

    /// <inheritdoc />
    public async Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildNewVersionVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }
}
