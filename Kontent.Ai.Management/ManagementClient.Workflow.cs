using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<WorkflowModel>> ListWorkflowsAsync()
        => EnsureSuccess(await _managementApi.ListWorkflowsInternalAsync());

    /// <inheritdoc />
    public async Task<WorkflowModel> CreateWorkflowAsync(WorkflowUpsertModel workflow)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        return EnsureSuccess(await _managementApi.CreateWorkflowInternalAsync(workflow));
    }

    /// <inheritdoc />
    public async Task<WorkflowModel> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(workflow);

        return EnsureSuccess(await _managementApi.UpdateWorkflowInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), workflow));
    }

    /// <inheritdoc />
    public async Task DeleteWorkflowAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteWorkflowInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)));
    }
}
