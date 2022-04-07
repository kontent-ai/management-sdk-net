using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
    public async Task DeleteWorkflowAsync(Reference identifier)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier));
        }

        var endpointUrl = _urlBuilder.BuildWorkflowsUrl(identifier);
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
