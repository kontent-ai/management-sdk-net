using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
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
        ArgumentNullException.ThrowIfNull(workflow);

        var endpointUrl = _urlBuilder.BuildWorkflowsUrl();
        return await _actionInvoker.InvokeMethodAsync<WorkflowUpsertModel, WorkflowModel>(endpointUrl, HttpMethod.Post, workflow);
    }

    /// <inheritdoc />
    public async Task<WorkflowModel> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(workflow);

        var endpointUrl = _urlBuilder.BuildWorkflowsUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<WorkflowUpsertModel, WorkflowModel>(endpointUrl, HttpMethod.Put, workflow);
    }

    /// <inheritdoc />
    public async Task DeleteWorkflowAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildWorkflowsUrl(identifier);
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
