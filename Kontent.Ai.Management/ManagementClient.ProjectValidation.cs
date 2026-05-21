using System;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentReportModel>> ValidateEnvironmentAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ValidateEnvironmentInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AsyncValidationTaskModel>> InitiateEnvironmentAsyncValidationTaskAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.InitiateEnvironmentAsyncValidationTaskInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AsyncValidationTaskModel>> GetAsyncValidationTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetAsyncValidationTaskInternalAsync(taskId, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<IListingResponseModel<AsyncValidationTaskIssueModel>>> ListAsyncValidationTaskIssuesAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId, cancellationToken: cancellationToken);
        return await response.ToManagementResultAsync(payload => BuildAsyncValidationTaskIssuesPage(taskId, payload));
    }

    private IListingResponseModel<AsyncValidationTaskIssueModel> BuildAsyncValidationTaskIssuesPage(Guid taskId, AsyncValidationTaskIssuesResponseServerModel payload)
        => new ListingResponseModel<AsyncValidationTaskIssueModel>(
            (continuationToken, _) => GetNextAsyncValidationTaskIssuesPageAsync(taskId, continuationToken),
            payload.Pagination?.Token,
            url: string.Empty,
            payload.Issues);

    private async Task<IListingResponse<AsyncValidationTaskIssueModel>> GetNextAsyncValidationTaskIssuesPageAsync(Guid taskId, string continuationToken)
        => EnsureSuccess(await _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId, continuationToken));
}
