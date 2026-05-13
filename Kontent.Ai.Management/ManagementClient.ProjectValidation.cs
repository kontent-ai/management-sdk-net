using System;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<EnvironmentReportModel> ValidateEnvironmentAsync()
        => EnsureSuccess(await _managementApi.ValidateEnvironmentInternalAsync());

    /// <inheritdoc />
    public async Task<AsyncValidationTaskModel> InitiateEnvironmentAsyncValidationTaskAsync()
        => EnsureSuccess(await _managementApi.InitiateEnvironmentAsyncValidationTaskInternalAsync());

    /// <inheritdoc />
    public async Task<AsyncValidationTaskModel> GetAsyncValidationTaskAsync(Guid taskId)
        => EnsureSuccess(await _managementApi.GetAsyncValidationTaskInternalAsync(taskId));

    /// <inheritdoc />
    public async Task<IListingResponseModel<AsyncValidationTaskIssueModel>> ListAsyncValidationTaskIssuesAsync(Guid taskId)
    {
        var response = EnsureSuccess(await _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId));

        return new ListingResponseModel<AsyncValidationTaskIssueModel>(
            (continuationToken, _) => GetNextAsyncValidationTaskIssuesPageAsync(taskId, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Issues);
    }

    private async Task<IListingResponse<AsyncValidationTaskIssueModel>> GetNextAsyncValidationTaskIssuesPageAsync(Guid taskId, string continuationToken)
        => EnsureSuccess(await _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId, continuationToken));
}
