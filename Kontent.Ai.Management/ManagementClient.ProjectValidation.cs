using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentReportModel>> ValidateEnvironmentAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ValidateEnvironmentInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AsyncValidationTaskModel>> InitiateEnvironmentAsyncValidationTaskAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.InitiateEnvironmentAsyncValidationTaskInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<AsyncValidationTaskModel>> GetAsyncValidationTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetAsyncValidationTaskInternalAsync(taskId, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<AsyncValidationTaskIssueModel>>> ListAsyncValidationTaskIssuesAsync(Guid taskId, CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<AsyncValidationTaskIssuesResponseServerModel, AsyncValidationTaskIssueModel>(
            (token, ct) => _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId, token, ct),
            page => page.Issues,
            page => page.Pagination?.Token,
            cancellationToken);
}
