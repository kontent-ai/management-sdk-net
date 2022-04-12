using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;

namespace Kentico.Kontent.Management;

/// <summary>
/// Executes requests against the Kontent Management API.
/// </summary>
public sealed partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<WorkflowModel>> ListWorkflowsAsync()
    {
        var endpointUrl = _urlBuilder.BuildWorkflowsUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<WorkflowModel>>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<WorkflowModel> CreateWorkflowAsync(WorkflowUpsertModel workflow)
    {
        var endpointUrl = _urlBuilder.BuildWorkflowsUrl();
        return await _actionInvoker.InvokeMethodAsync<WorkflowUpsertModel, WorkflowModel>(endpointUrl, HttpMethod.Post, workflow);
    }

    /// <inheritdoc />
    public async Task<WorkflowModel> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow)
    {
        var endpointUrl = _urlBuilder.BuildWorkflowsUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<WorkflowUpsertModel, WorkflowModel>(endpointUrl, HttpMethod.Put, workflow);
    }

    /// <inheritdoc />
    public async Task DeleteWorkflowAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildWorkflowsUrl(identifier);
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }

    /// <inheritdoc />
    public async Task ChangeLanguageVariantWorkflowAsync(LanguageVariantIdentifier identifier, WorkflowStepIdentifier workflowStepIdentifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        if (workflowStepIdentifier == null)
        {
            throw new ArgumentNullException(nameof(workflowStepIdentifier));
        }

        var endpointUrl = _urlBuilder.BuildWorkflowChangeUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, workflowStepIdentifier);
    }

    /// <inheritdoc />
    public async Task PublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task SchedulePublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildPublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
    }

    /// <inheritdoc />
    public async Task CancelPublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildCancelPublishingVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task UnpublishLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task CancelUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildCancelUnpublishingVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }

    /// <inheritdoc />
    public async Task ScheduleUnpublishingOfLanguageVariantAsync(LanguageVariantIdentifier identifier, ScheduleModel scheduleModel)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildUnpublishVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put, scheduleModel);
    }

    /// <inheritdoc />
    public async Task CreateNewVersionOfLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildNewVersionVariantUrl(identifier);

        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Put);
    }
}
