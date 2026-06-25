using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<WorkflowModel>>> ListWorkflowsAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.ListWorkflowsInternalAsync(cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<WorkflowModel>> CreateWorkflowAsync(WorkflowUpsertModel workflow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        return _managementApi.CreateWorkflowInternalAsync(workflow, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<WorkflowModel>> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(workflow);

        return _managementApi.UpdateWorkflowInternalAsync(identifier.ToUrlSegment(), workflow, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult> DeleteWorkflowAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.DeleteWorkflowInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }
}
