using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<EnvironmentReportModel>> ValidateEnvironmentAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.ValidateEnvironmentInternalAsync(cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<AsyncValidationTaskModel>> InitiateEnvironmentAsyncValidationTaskAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.InitiateEnvironmentAsyncValidationTaskInternalAsync(cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<AsyncValidationTaskModel>> GetAsyncValidationTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return _managementApi.GetAsyncValidationTaskInternalAsync(taskId, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<AsyncValidationTaskIssueModel>>> ListAsyncValidationTaskIssuesAsync(Guid taskId, CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<AsyncValidationTaskIssuesResponseServerModel, AsyncValidationTaskIssueModel>(
            (token, ct) => _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId, token, ct),
            page => page.Issues,
            page => page.Pagination?.Token,
            cancellationToken);
}
