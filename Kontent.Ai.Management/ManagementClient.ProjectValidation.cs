using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;

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
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<AsyncValidationTaskIssueModel>>> EnumerateAsyncValidationTaskIssuePagesAsync(Guid taskId, CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<AsyncValidationTaskIssuesResponseServerModel, AsyncValidationTaskIssueModel>(
            (token, ct) => _managementApi.ListAsyncValidationTaskIssuesInternalAsync(taskId, token, ct),
            page => page.Issues,
            page => page.Pagination?.Token,
            cancellationToken);
}
