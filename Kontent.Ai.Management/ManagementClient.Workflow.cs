using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<IReadOnlyList<WorkflowModel>>> ListWorkflowsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListWorkflowsInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<WorkflowModel>> CreateWorkflowAsync(WorkflowUpsertModel workflow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        var response = await _managementApi.CreateWorkflowInternalAsync(workflow, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<WorkflowModel>> UpdateWorkflowAsync(Reference identifier, WorkflowUpsertModel workflow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(workflow);

        var response = await _managementApi.UpdateWorkflowInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), workflow, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteWorkflowAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteWorkflowInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
