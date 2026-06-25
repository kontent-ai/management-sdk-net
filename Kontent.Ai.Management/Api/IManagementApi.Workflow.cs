using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists the workflows in the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/workflows")]
    internal Task<IApiResponse<IReadOnlyList<WorkflowModel>>> ListWorkflowsInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new workflow.</summary>
    /// <param name="workflow">The workflow to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/workflows")]
    internal Task<IApiResponse<WorkflowModel>> CreateWorkflowInternalAsync(
        [Body] WorkflowUpsertModel workflow,
        CancellationToken cancellationToken = default);

    /// <summary>Updates an existing workflow.</summary>
    /// <param name="identifier">The workflow identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="workflow">The workflow to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/workflows/{**identifier}")]
    internal Task<IApiResponse<WorkflowModel>> UpdateWorkflowInternalAsync(
        string identifier,
        [Body] WorkflowUpsertModel workflow,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a workflow.</summary>
    /// <param name="identifier">The workflow identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/workflows/{**identifier}")]
    internal Task<IApiResponse> DeleteWorkflowInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}
