using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Validates the environment synchronously.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/validate")]
    internal Task<IApiResponse<EnvironmentReportModel>> ValidateEnvironmentInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Starts an asynchronous environment validation task.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/validate-async")]
    internal Task<IApiResponse<AsyncValidationTaskModel>> InitiateEnvironmentAsyncValidationTaskInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Gets an asynchronous environment validation task.</summary>
    /// <param name="taskId">The validation task id.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/validate-async/tasks/{taskId}")]
    internal Task<IApiResponse<AsyncValidationTaskModel>> GetAsyncValidationTaskInternalAsync(
        Guid taskId,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of an asynchronous validation task's issues.</summary>
    /// <param name="taskId">The validation task id.</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/validate-async/tasks/{taskId}/issues")]
    internal Task<IApiResponse<AsyncValidationTaskIssuesResponseServerModel>> ListAsyncValidationTaskIssuesInternalAsync(
        Guid taskId,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);
}
